// From https://fsharpforfunandprofit.com/posts/monoids-without-tears/
namespace Monoids

module OrdersUsingFold1 =
    type OrderLine =
        { ProductCode: string
          Qty: int
          Total: float }

    let orderLine1 =
        { ProductCode = "AAA"
          Qty = 2
          Total = 19.98 }

    let orderline2 =
        { ProductCode = "BBB"
          Qty = 1
          Total = 1.99 }

    let orderLine3 =
        { ProductCode = "CCC"
          Qty = 3
          Total = 3.99 }

    let orderLines = [ orderLine1; orderline2; orderLine3 ]

    let newLine =
        { ProductCode = "DDD"
          Qty = 1
          Total = 29.98 }

    let zero =
        { ProductCode = ""
          Qty = 0
          Total = 0.0 }

    let addLine1 orderLine1 orderLine2 =
        { ProductCode = "TOTAL"
          Qty = orderLine1.Qty + orderLine2.Qty
          Total = orderLine1.Total + orderLine2.Total }

    let addLine orderLine1 orderLine2 =
        match orderLine1.ProductCode, orderline2.ProductCode with
        | "", _ -> orderline2
        | _, "" -> orderLine1
        | _, _ -> addLine1 orderLine1 orderLine2

    let printLine { ProductCode = p; Qty = q; Total = t } = printfn "%-10s %5i %6g" p q t

    let printReceipt lines =
        lines |> List.iter printLine
        printfn "----------"
        lines |> List.reduce addLine |> printLine

    let run () =
        printfn "=> OrderUsingFold1"
        // printLine orderLine1
        // addLine orderLine1 orderline2 |> printLine
        // orderLines |> List.reduce addLine |> printLine
        // printReceipt orderLines
        let subTotal = orderLines |> List.reduce addLine
        let newSubTotal = subTotal |> addLine newLine
        newSubTotal |> printLine

module OrdersUsingFold2 =
    type ProductLine =
        { ProductCode: string
          Qty: int
          Price: float
          LineTotal: float }

    type TotalLine = { Qty: int; OrderTotal: float }

    type OrderLine =
        | Product of ProductLine
        | Total of TotalLine
        | EmptyOrder

    let addLine orderLine1 orderLine2 =
        match orderLine1, orderLine2 with
        | EmptyOrder, _ -> orderLine2
        | _, EmptyOrder -> orderLine1
        | Product p1, Product p2 ->
            Total
                { Qty = p1.Qty + p2.Qty
                  OrderTotal = p1.LineTotal + p2.LineTotal }
        | Product p, Total t ->
            Total
                { Qty = p.Qty + t.Qty
                  OrderTotal = p.LineTotal + t.OrderTotal }
        | Total t, Product p ->
            Total
                { Qty = t.Qty + p.Qty
                  OrderTotal = t.OrderTotal + p.LineTotal }
        | Total t1, Total t2 ->
            Total
                { Qty = t1.Qty + t2.Qty
                  OrderTotal = t1.OrderTotal + t2.OrderTotal }


    // NOTE: pattern matching function
    let printLine =
        function
        | EmptyOrder -> printfn ""
        | Product { ProductCode = p
                    Qty = q
                    Price = pr
                    LineTotal = lt } -> printfn "%-10s %5i %6g %6g" p q pr lt
        | Total { Qty = q; OrderTotal = t } -> printfn "%-10s %5i %6g" "TOTAL" q t

    let orderLine1 =
        Product
            { ProductCode = "AAA"
              Qty = 2
              Price = 9.99
              LineTotal = 19.98 }

    let orderline2 =
        Product
            { ProductCode = "BBB"
              Qty = 1
              Price = 1.99
              LineTotal = 1.99 }

    let zero = EmptyOrder

    let orderLines = [ orderLine1; orderline2 ]

    let printReceipt lines =
        lines |> List.iter printLine
        printfn "----------"
        lines |> List.reduce addLine |> printLine

    let run () =
        printfn "=> OrderUsingFold2"
        // orderLines |> printReceipt
        addLine orderLine1 zero |> printLine

module StringMonoid =
    type System.String with
        static member Zero = ""

    let l = [ "a"; "b"; "c" ]

    let run () =
        printfn "=> StringMonoid"
        printfn "Using reduce: %s" (l |> List.reduce (+))
        printfn "Using fold: %s" (l |> List.fold (+) System.String.Zero)
// NOTE: Error because String.Zero extension method is not visible
// printfn "Using sum: %s" (l |> List.sum)

module MappingDifferentStructure =
    type Customer =
        { Name: string
          LastActive: System.DateTime
          TotalSpent: float }

    type CustomerStats =
        { Count: int
          TotalInactiveDays: int
          TotalSpent: float }

    let add stat1 stat2 =
        { Count = stat1.Count + stat2.Count
          TotalInactiveDays = stat1.TotalInactiveDays + stat2.TotalInactiveDays
          TotalSpent = stat1.TotalSpent + stat2.TotalSpent }

    let (++) a b = add a b

    let toStats cust =
        let inactiveDays = System.DateTime.Now.Subtract(cust.LastActive).Days

        { Count = 1
          TotalInactiveDays = inactiveDays
          TotalSpent = cust.TotalSpent }

    let c1 =
        { Name = "Alice"
          LastActive = System.DateTime(2005, 1, 1)
          TotalSpent = 100.0 }

    let c2 =
        { Name = "Bob"
          LastActive = System.DateTime(2010, 2, 2)
          TotalSpent = 45.0 }

    let c3 =
        { Name = "Charlie"
          LastActive = System.DateTime(2011, 3, 3)
          TotalSpent = 42.0 }

    let customers = [ c1; c2; c3 ]

    let run () =
        printfn "=> StringMonoid"

        customers
        |> List.map toStats
        |> List.reduce (++)
        |> printfn "%A"

module MonoidHomorphism =
    type Text = Text of string

    let addText (Text s1) (Text s2) = Text(s1 + s2)

    let t1 = Text "Hello"
    let t2 = Text " World"
    let t3 = addText t1 t2

    let wordCount (Text s) = s.Split(' ').Length

    let run () =
        // 42
        printfn "=> MonoidHomorphism"

        Text "Hello world"
        |> wordCount
        |> printfn "Word count is: %i"

module WordCountTest =
    type Text = Text of string

    let addText (Text s1) (Text s2) = Text(s1 + " " + s2)

    let wordCount (Text s) =
        System
            .Text
            .RegularExpressions
            .Regex
            .Matches(
                s,
                @"\S+"
            )
            .Count

    let page () =
        List.replicate 1000 "hello"
        |> List.reduce (+)
        |> Text

    let document () = page () |> List.replicate 1000

    // NOTE: System.Diagnostics
    let time f msg =
        let stopwatch = System.Diagnostics.Stopwatch()
        stopwatch.Start()
        f ()
        stopwatch.Stop()
        printfn "Time taken for %s was %ims" msg stopwatch.ElapsedMilliseconds

    let wordCountViaAddText () =
        document ()
        |> List.reduce addText
        |> wordCount
        |> printfn "Word count is: %i"

    let wordCountViaMap () =
        document ()
        |> List.map wordCount
        |> List.reduce (+)
        |> printfn "Word count is: %i"

    let wordCountViaParallelAddCounts () =
        document ()
        |> List.toArray
        |> Array.Parallel.map wordCount
        |> Array.reduce (+)
        |> printfn "Word count is: %i"

    let run () =
        printfn "=> WordCountTest"
        time wordCountViaAddText "reduce then count"
        time wordCountViaMap "map then reduce"
        time wordCountViaParallelAddCounts "parallel map then reduce"

// Non-monoid homorphism
module FrequentWordTest =
    type Text = Text of string

    let addText (Text s1) (Text s2) = Text(s1 + " " + s2)

    let mostFrequenWord (Text s) =
        System.Text.RegularExpressions.Regex.Matches(s, @"\S+")
        |> Seq.cast
        |> Seq.map (fun m -> m.ToString())
        |> Seq.groupBy id
        |> Seq.map (fun (k, v) -> k, Seq.length v)
        |> Seq.sortBy (fun (_, v) -> v)
        |> Seq.head
        |> fst

    let page1 () =
        List.replicate 1000 "hello world "
        |> List.reduce (+)
        |> Text

    let page2 () =
        List.replicate 1000 "goodbye world "
        |> List.reduce (+)
        |> Text

    let page3 () =
        List.replicate 1000 "foobar "
        |> List.reduce (+)
        |> Text

    let document () = [ page1 (); page2 (); page3 () ]

    let run () =
        // 42
        printfn "=> FrequentWordTest"
        // page1 () |> mostFrequenWord |> printfn "%A"
        document ()
        |> List.reduce addText
        |> mostFrequenWord
        |> printfn "using add first, the most frequent word is %s"

        document ()
        |> List.map mostFrequenWord
        |> List.reduce (+)
        |> printfn "using map, the most frequent word is %s"
