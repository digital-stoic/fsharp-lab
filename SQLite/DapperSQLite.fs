// Inspired from https://isthisit.nz/posts/2019/sqlite-database-with-dapper-and-fsharp/

module DapperSQLite

open Microsoft.Data.Sqlite
open FSharp.Data.Dapper

[<CLIMutable>]
type MyDataDto =
    { Id: int64
      Question: string
      Answer: int
      Comments: string option }

let makeSQLiteConnectionOnDisk dbFilename () =
    let connectionStr =
        // sprintf "Data Source=%s;Version=3;" dbFilename
        sprintf "Data Source=%s;" dbFilename

    new SqliteConnection(connectionStr)

let makeConnection sqliteConnection () =
    Connection.SqliteConnection(sqliteConnection)

let createTable connection =
    querySingleOptionAsync<int> connection {
        script """
            DROP TABLE IF EXISTS MyData;
            CREATE TABLE MyData (
                Question VARCHAR(256) NOT NULL,
                Answer INTEGER NOT NULL,
                Comments VARCHAR(256)
            );
        """
    }

let dbFileName = "myData.sqlite"

let demo =
    printfn "F# Demo: DapperSQlite"
    let sqliteConnection = makeSQLiteConnectionOnDisk dbFileName ()
    sqliteConnection.Open()
    let connection = makeConnection sqliteConnection
    createTable connection
    |> Async.RunSynchronously
    |> ignore
    sqliteConnection.Close()
