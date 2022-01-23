module Tmp

open System
open System.Collections.Generic
open System.IO
open NPOI.SS.UserModel
open NPOI.SS.Util
open NPOI.XSSF.UserModel
open NPOI.OpenXmlFormats.Spreadsheet


//write headers at the top of the sheet
let initSheet (sheet: ISheet) (headers: #seq<string>) =
    //freeze below top row, on the left of first column
    sheet.CreateFreezePane(1, 1)
    let mutable col = 0
    let row = sheet.CreateRow(0)

    for header in headers do
        row.CreateCell(col).SetCellValue(header)
        col <- col + 1


let autoSizeColumns (headers: #seq<string>) (sheet: ISheet) =
    for col in 0 .. Seq.length headers - 1 do
        sheet.AutoSizeColumn(col)


let generateWorkbook fileName headers =
    let workbook = new XSSFWorkbook()

    let nHeaders = Seq.length headers

    let DAILY = "Daily"
    let END_OF_MONTH = "EOM"
    let YTD = "Ytd"
    let ALL_SHEET_NAMES = [ DAILY; END_OF_MONTH; YTD ]


    let mutable col = 0

    //create sheets
    ALL_SHEET_NAMES
    |> List.map workbook.CreateSheet
    |> ignore

    //use the first one
    let sheet = workbook.GetSheet(DAILY)


    //====================================================
    //
    // Here starts the "failed" attempt to create a table
    //
    //====================================================

    //create table
    let s = sheet :?> XSSFSheet
    let table = s.CreateTable()
    let cttable = table.GetCTTable()
    cttable.id <- 1u

    //name
    table.Name <- "MyTableName"
    table.DisplayName <- "MyTableName"

    //show headers, not totals
    cttable.headerRowCount <- 1u
    cttable.totalsRowCount <- 0u

    //area
    let startRef = CellReference(0, 0)

    let endRef =
        CellReference(70000, Seq.length headers - 1)

    let tableArea = AreaReference(startRef, endRef)
    let mutable areaRef = tableArea.FormatAsString()

    let rowCount =
        (tableArea.LastCell.Row - tableArea.FirstCell.Row)
        + 1

    let minimumRowCount =
        1u
        + cttable.headerRowCount
        + cttable.totalsRowCount

    if uint32 rowCount < minimumRowCount then
        invalidArg "row count"
        <| "AreaReference needs at least "
           + string minimumRowCount
           + " rows, to cover at least one data row and all header rows and totals rows"

    //strip the sheet name
    if areaRef.IndexOf('!') <> -1 then
        areaRef <- areaRef.[areaRef.IndexOf('!') + 1..]

    //update
    cttable.ref <- areaRef
    let autoFilter = new CT_AutoFilter()
    autoFilter.ref <- areaRef
    cttable.autoFilter <- autoFilter
    table.UpdateReferences()

    //number of columns
    if cttable.tableColumns = null then
        cttable.tableColumns <- new CT_TableColumns()

    if cttable.tableColumns.tableColumn = null then
        cttable.tableColumns.tableColumn <- new List<_>()

    let createColumn () =
        let mutable id = 0u

        for i in 0 .. cttable.tableColumns.tableColumn.Count - 1 do
            id <- max id cttable.tableColumns.tableColumn.[i].id

        id <- id + 1u
        let col = new CT_TableColumn()
        col.name <- "Col " + string id
        cttable.tableColumns.tableColumn.Add(col)
        cttable.tableColumns.count <- uint32 cttable.tableColumns.tableColumn.Count

    let columnsCount = cttable.tableColumns.count

    let newColumnsCount =
        tableArea.LastCell.Col - tableArea.FirstCell.Col
        + 1s

    if uint32 newColumnsCount > columnsCount then
        for i in columnsCount .. uint32 newColumnsCount - 1u do
            createColumn ()

    //write headers
    initSheet sheet headers

    //headers
    table.UpdateHeaders()

    //====================================================
    //
    // Here ends the "failed" attempt to create a table
    //
    //====================================================

    //table values (date in first column, values in following columns)
    let mutable x = 1.0

    for i in 1 .. 70000 do
        let row = sheet.CreateRow(i)

        for j in 0 .. 3 do
            let cell = row.CreateCell(j)

            if j = 0 then
                cell.SetCellValue(DateTime.Now)
            else
                cell.SetCellValue(x)

            x <- x + 1.0

    //auto-resize all columns on all sheets
    ALL_SHEET_NAMES
    |> List.map workbook.GetSheet
    |> List.iter (autoSizeColumns headers)

    let sw = File.Create(fileName)
    workbook.Write(sw)
    sw.Close()


// [<EntryPoint>]
let run =
    let headers = [ "Date"; "A"; "B"; "C" ]
    // let fileName = @"C:\test.xlsx"
    let fileName = @"out/test.xlsx"
    generateWorkbook fileName headers
    0
