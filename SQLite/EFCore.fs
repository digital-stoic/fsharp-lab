module EFCore

open Microsoft.EntityFrameworkCore

// TODO: Try from DTO type
// TODO: Try with list and options
// TODO: automatic primary keys
[<CLIMutable>]
type MyType =
    { Id: System.Nullable<int64>
      Question: string
      Answer: int }

type MyContext =
    inherit DbContext

    new() = { inherit DbContext() }

    new(options: DbContextOptions<MyContext>) = { inherit DbContext(options) }

    // override __.OnModelCreating ModelBuilder =
    //     let myDataConvert = ValueConverter
    //     42

    [<DefaultValue>]
    val mutable myData: DbSet<MyType>

    member x.MyData
        with get () = x.myData
        and set v = x.myData <- v

let makeSqliteContext dbFilename () =
    let optionsBuilder = new DbContextOptionsBuilder<MyContext>()
    let connectionStr = sprintf "Data Source=%s;" dbFilename
    optionsBuilder.UseSqlite(connectionStr) |> ignore
    new MyContext(optionsBuilder.Options)

let saveMyData (context: MyContext) (entity: MyType) =
    async {
        context.MyData.AddAsync(entity).AsTask()
        |> Async.AwaitTask
        |> ignore
        context.SaveChangesAsync true |> ignore

        // TODO: return what?
        return entity
    }

let saveMyDataSync (context: MyContext) (entity: MyType) =
    context.MyData.Add(entity) |> ignore
    context.SaveChanges true

let dbFileName = "MyData.db"

let myData1 =
    { Id = System.Nullable()
      Question = "Universe?"
      Answer = 42 }

// TODO: Catch exceptions
let demo =
    printfn "F# Demo: Entity Framework Core"
    let context = makeSqliteContext dbFileName ()
    // saveMyData context myData1
    // |> Async.RunSynchronously
    saveMyDataSync context myData1 |> ignore
    0
