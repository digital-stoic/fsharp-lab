module EFCore

open System
open Microsoft.EntityFrameworkCore

// TODO: Try from DTO type
// TODO: Try with list and options
// type MyTypeDTO = { Question: string; Answer: int }

[<CLIMutable>]
type MyTypeEntity =
    { Id: Nullable<int64>
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
    val mutable myData: DbSet<MyTypeEntity>

    member x.MyData
        with get () = x.myData
        and set v = x.myData <- v

let makeSqliteContext dbFilename () =
    let optionsBuilder = new DbContextOptionsBuilder<MyContext>()
    let connectionStr = sprintf "Data Source=%s;" dbFilename
    optionsBuilder.UseSqlite(connectionStr) |> ignore
    new MyContext(optionsBuilder.Options)

let saveMyData (context: MyContext) (entity: MyTypeEntity) =
    async {
        context.MyData.AddAsync(entity).AsTask()
        |> Async.AwaitTask
        |> ignore
        context.SaveChangesAsync true |> ignore

        // TODO: return what?
        return entity
    }

let saveMyDataSync (context: MyContext) (entity) =
    context.MyData.Add(entity) |> ignore
    context.SaveChanges true

let getMyDataById (context: MyContext) (id: int) =
    let _id = Nullable<int64>(int64 id)
    query {
        for data in context.MyData do
            where (data.Id = _id)
            select data
            exactlyOne
    }

// Check https://stackoverflow.com/questions/21097357/async-database-query
let getMyDataByIdAsync =
    fun (context: MyContext) (id: int) ->
        async {
            let _id = Nullable<int64>(int64 id)

            let result =
                query {
                    for data in context.MyData do
                        where (data.Id = _id)
                        select data
                        exactlyOne
                }

            return result
        }

let getMyDataById' (context: MyContext) (id: int) =
    let _id = Nullable<int64>(int64 id)
    context.MyData.Find(_id)

let updateMyDataById (context: MyContext) (id: int) newAnswer =
    let _id = Nullable<int64>(int64 id)
    let data = context.MyData.Find(_id)
    let newData = { data with Answer = newAnswer }
    context.Entry(data).CurrentValues.SetValues(newData)
    context.SaveChanges true |> ignore

let dbFileName = "MyData.db"

let myData1 =
    { Id = Nullable()
      Question = "Universe?"
      Answer = 42 }


// TODO: Catch exceptions
let demo =
    printfn "F# Demo: Entity Framework Core"
    let context = makeSqliteContext dbFileName ()
    // context.Database.Migrate()
    saveMyData context myData1
    |> Async.RunSynchronously
    |> ignore
    // saveMyDataSync context myData1 |> ignore
    let id = 3
    let res1 = getMyDataById context id
    let res1' = getMyDataById' context id

    let res1'' =
        getMyDataByIdAsync context id
        |> Async.RunSynchronously

    printfn "Record Id %d: %A" id res1
    updateMyDataById context id 9
    // printfn "Record Id %d: %A" id res1'
    printfn "Record Id %d: %A" id res1''
    0
