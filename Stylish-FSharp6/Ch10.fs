module Ch10

let description = "Asynchronous and parallel programming"

module Log =
    open System
    open System.Threading

    // let message (color: ConsoleColor) (message: string) =
    //     Console.ForegroundColor <- color
    //     printfn "%s (thread ID: %i)" message Thread.CurrentThread.ManagedThreadId
    //     Console.ResetColor()

    // NOTE: F# lock
    let message =
        let lockObj = obj ()

        fun (color: ConsoleColor) (message: string) ->
            lock lockObj (fun () ->
                Console.ForegroundColor <- color
                printfn "%s (thread ID: %i)" message Thread.CurrentThread.ManagedThreadId
                Console.ResetColor())

    let red = message ConsoleColor.Red

    let green = message ConsoleColor.Green

    let yellow = message ConsoleColor.Yellow

    let cyan = message ConsoleColor.Cyan

module Download =
    open System
    open System.IO
    open System.Net
    open System.Text.RegularExpressions
    open FSharp.Data
    open Result

    let private absoluteUri (pageUri: Uri) (filePath: string) =
        if
            filePath.StartsWith("http:")
            || filePath.StartsWith("https:")
        then
            Uri(filePath)
        else
            let sep = '/'

            filePath.TrimStart(sep)
            |> (sprintf "%O%c%s" pageUri sep)
            |> Uri

    let private getLinks (pageUri: Uri) (filePattern: string) =
        Log.cyan "Getting names..."
        let re = Regex(filePattern)
        let html = HtmlDocument.Load(pageUri.AbsoluteUri)

        let links =
            html.Descendants [ "a" ]
            |> Seq.choose (fun node ->
                node.TryGetAttribute("href")
                |> Option.map (fun att -> att.Value()))
            |> Seq.filter re.IsMatch
            |> Seq.map (absoluteUri pageUri)
            |> Seq.distinct
            |> Array.ofSeq

        links

    // NOTE: Exceptions with try prefix https://fsharpforfunandprofit.com/posts/exceptions/
    let private tryDownload (localPath: string) (fileUri: Uri) =
        let fileName = fileUri.Segments |> Array.last
        Log.yellow (sprintf "%s - starting download..." fileName)
        let filePath = Path.Combine(localPath, fileName)
        use client = new WebClient()

        try
            client.DownloadFile(fileUri, filePath)
            Log.green $"{fileName} - download complete"
            Ok fileName
        with
        | e ->
            let message =
                e.InnerException
                |> Option.ofObj
                |> Option.map (fun ie -> ie.Message)
                |> Option.defaultValue e.Message

            Log.red $"{fileName} - error: {message}"
            Result.Error e.Message

    let getFiles (pageUri: Uri) (filePattern: string) (localPath: string) =
        let links = getLinks pageUri filePattern
        let downloadResults = links |> Array.map (tryDownload localPath)

        let isOK =
            function
            | Ok _ -> true
            | Error _ -> false

        let successCounts = downloadResults |> Seq.filter isOK |> Seq.length

        let errorCounts =
            downloadResults
            |> Seq.filter (isOK >> not)
            |> Seq.length

        {| SuccessCount = successCounts
           ErrorCount = errorCounts |}

    let private asyncGetLinks (pageUri: Uri) (filePattern: string) =
        async {
            Log.cyan "Getting names..."
            let re = Regex(filePattern)
            // let! html = HtmlDocument.AsyncLoad(pageUri.AbsoluteUri)
            let! html = HtmlDocument.AsyncLoad(pageUri.AbsoluteUri)

            let links =
                html.Descendants [ "a" ]
                |> Seq.choose (fun node ->
                    node.TryGetAttribute("href")
                    |> Option.map (fun att -> att.Value()))
                |> Seq.filter re.IsMatch
                |> Seq.map (absoluteUri pageUri)
                |> Seq.distinct
                |> Array.ofSeq

            return links
        }

    let private asyncTryDownload (localPath: string) (fileUri: Uri) =
        async {
            let fileName = fileUri.Segments |> Array.last
            Log.yellow (sprintf "%s - starting download..." fileName)
            let filePath = Path.Combine(localPath, fileName)
            use client = new WebClient()

            try
                do!
                    client.DownloadFileTaskAsync(fileUri, filePath)
                    |> Async.AwaitTask

                Log.green $"{fileName} - download complete"
                return (Ok fileName)
            with
            | e ->
                let message =
                    e.InnerException
                    |> Option.ofObj
                    |> Option.map (fun ie -> ie.Message)
                    |> Option.defaultValue e.Message

                Log.red $"{fileName} - error: {message}"
                return (Result.Error e.Message)
        }

    let asyncGetFiles (pageUri: Uri) (filePattern: string) (localPath: string) =
        async {
            let! links = asyncGetLinks pageUri filePattern

            let! downloadResults =
                links
                |> Array.map (asyncTryDownload localPath)
                |> Async.Parallel

            let isOK =
                function
                | Ok _ -> true
                | Error _ -> false

            let successCounts = downloadResults |> Seq.filter isOK |> Seq.length

            let errorCounts =
                downloadResults
                |> Seq.filter (isOK >> not)
                |> Seq.length

            return
                {| SuccessCount = successCounts
                   ErrorCount = errorCounts |}
        }

    let asyncGetFilesBatch (pageUri: Uri) (filePattern: string) (localPath: string) =
        let batchSize = 5

        async {
            let! links = asyncGetLinks pageUri filePattern

            let downloadResults =
                links
                |> Seq.map (asyncTryDownload localPath)
                |> Seq.chunkBySize batchSize
                |> Seq.collect (Async.Parallel >> Async.RunSynchronously)
                |> Array.ofSeq

            let isOK =
                function
                | Ok _ -> true
                | Error _ -> false

            let successCounts = downloadResults |> Seq.filter isOK |> Seq.length

            let errorCounts =
                downloadResults
                |> Seq.filter (isOK >> not)
                |> Seq.length

            return
                {| SuccessCount = successCounts
                   ErrorCount = errorCounts |}
        }

    let asyncGetFilesThrottle (pageUri: Uri) (filePattern: string) (localPath: string) =
        let throttle = 5

        async {
            let! links = asyncGetLinks pageUri filePattern

            let! downloadResults =
                links
                |> Seq.map (asyncTryDownload localPath)
                |> (fun items -> Async.Parallel(items, throttle))

            let isOK =
                function
                | Ok _ -> true
                | Error _ -> false

            let successCounts = downloadResults |> Seq.filter isOK |> Seq.length

            let errorCounts =
                downloadResults
                |> Seq.filter (isOK >> not)
                |> Seq.length

            return
                {| SuccessCount = successCounts
                   ErrorCount = errorCounts |}
        }

open System
open System.Diagnostics

type AsyncGetFiles = Uri -> string -> string -> Async<{| ErrorCount: int; SuccessCount: int |}>

let runSynchronous uri pattern localPath (sw: Stopwatch) =
    let result = Download.getFiles uri pattern localPath

    Log.cyan (
        sprintf
            "%i files downloaded in %0.1fs, %i failed"
            result.SuccessCount
            (sw.ElapsedMilliseconds / 1000L |> float)
            result.ErrorCount
    )

let runAsynchronous (getFiles: AsyncGetFiles) uri pattern localPath (sw: Stopwatch) =
    async {
        let! result = getFiles uri pattern localPath

        Log.cyan (
            sprintf
                "%i files downloaded in %0.1fs, %i failed"
                result.SuccessCount
                (sw.ElapsedMilliseconds / 1000L |> float)
                result.ErrorCount
        )
    }

let fast =
    {| uri = Uri "https://minorplanetcenter.net/data"
       pattern = @"neam.*\.json\.gz$"
       localPath = "/tmp/ch10" |}

let slow =
    {| uri = Uri "http://storage.googleapis.com/books/ngrams/books/datasetsv2.html"
       pattern = @"eng\-1M\-2gram.*\.zip$"
       localPath = "/tmp/ch10" |}

let run =
    printfn $"{description}..."

    let sw = Stopwatch()
    sw.Start()
    // runSynchronous fast.uri fast.pattern fast.localPath sw
    // sw.Stop()
    // runAsynchronous fast.uri fast.pattern fast.localPath sw
    // runAsynchronous Download.asyncGetFiles slow.uri slow.pattern slow.localPath sw
    // runAsynchronous Download.asyncGetFilesBatch slow.uri slow.pattern slow.localPath sw
    runAsynchronous Download.asyncGetFilesThrottle slow.uri slow.pattern slow.localPath sw
