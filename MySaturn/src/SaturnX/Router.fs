module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open Giraffe.HttpStatusCodeHandlers
open Giraffe.ModelBinding
open FSharp.Control.Tasks.V2.ContextInsensitive
open Microsoft.AspNetCore.Http

type MyDTO =
    { question: string
      answer: int
      options: bool option }

let browser =
    pipeline {
        plug acceptHtml
        plug putSecureBrowserHeaders
        plug fetchSession
        set_header "x-pipeline-type" "Browser"
    }

let defaultView =
    router {
        get "/" (htmlView Index.layout)
        get "/index.html" (redirectTo false "/")
        get "/default.html" (redirectTo false "/")
    }

let browserRouter =
    router {
        not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
        pipe_through browser //Use the default browser pipeline
        forward "" defaultView
        forward "/books" Books.Controller.resource
    }

let api =
    pipeline {
        plug acceptJson
        set_header "x-pipeline-type" "Api"
    }

let addId (obj: MyDTO) id =
    Successful.ok (json ({| obj with id = id |}))

let addPayload (obj: MyDTO): HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let! dto = ctx.BindJsonAsync<MyDTO>()
            printfn "DEBUG: %A" dto
            return! Successful.ok (json {| obj with payload = dto |}) next ctx
        }

let obj1 =
    { question = "universe"
      answer = 42
      options = None }

let apiRouter =
    router {
        not_found_handler (text "Api 404")
        pipe_through api
        get "/test1" (Successful.OK "Test 1 OK")
        get "/test2" (Successful.ok (json obj1))
        getf "/test3/%s" (addId obj1)
        getf "/test4/%i" (addId obj1)
        post "/test5" (addPayload obj1)
    }

let appRouter =
    router {
        forward "" browserRouter
        forward "/api" apiRouter
    }
