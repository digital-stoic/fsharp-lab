module DirectSQLite

open System.Data.SQLite

type MyData1 = { Question: string; Answer: int }

let myData1 = { Question = "Universe?"; Answer = 42 }

let dbFilename = "myData.sqlite"

let createDbFile dbFilename =
    SQLiteConnection.CreateFile(dbFilename)

    let connectionStr =
        sprintf "Data Source=%s;Version=3;" dbFilename
    // let connectionMemoryStr = sprintf "Data Source=:memory:;Version=3;New=True;"
    let connection = new SQLiteConnection(connectionStr)
    connection.Open()
    connection

let createTableMyData1Sql = @"create table MyData1 (
        Question varchar(42),
        Answer integer)
    "

let createTable connection sql =
    let commmand = new SQLiteCommand(sql, connection)
    commmand.ExecuteNonQuery()

let createDb dbFilename =
    let connection = createDbFile dbFilename

    let result =
        createTable connection createTableMyData1Sql

    printfn "createDb result = %A" result
    connection

let createDb' dbFilename =
    SQLiteConnection.CreateFile(dbFilename)

    let connectionStr =
        sprintf "Data Source=%s;Version=3;" dbFilename
    // let connectionMemoryStr = sprintf "Data Source=:memory:;Version=3;New=True;"
    let connection = new SQLiteConnection(connectionStr)
    connection.Open()

    let commmand =
        new SQLiteCommand(createTableMyData1Sql, connection)

    let result = commmand.ExecuteNonQuery()
    printfn "createTable result = %A" result
    connection.Close()

let demo =
    printfn "F# demo: DirectSQLite"
    createDb' dbFilename
