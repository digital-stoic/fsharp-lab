module ToXLS

open System.IO
open NPOI.SS.UserModel
open NPOI.XSSF.UserModel

type CellStyle = CellStyle of string

type CellContent =
    | CellValue of string
    | CellFormula of string

type Cell = CellContent * CellStyle
type CellRange = list<Cell>

// TODO: Typed Style?
let C (content: string) (style: string) =
    let c =
        if content.StartsWith('=') then
            (CellFormula content)
        else
            (CellValue content)

    (c, CellStyle style)

let cellAddress (row, column) =
    C $"=INDIRECT(ADDRESS({row}, {column}))"

let A = cellAddress

let initSheet (workbook: XSSFWorkbook) sheetName =
    workbook.CreateSheet(sheetName) |> ignore

let initRows (workbook: XSSFWorkbook) sheetName ranges =
    let sheet = workbook.GetSheet(sheetName)

    List.indexed ranges
    |> List.iter (fun (i, _) -> sheet.CreateRow(i) |> ignore)

let initCellsOfRow (workbook: XSSFWorkbook) sheetName rowNumber range =
    let row =
        workbook.GetSheet(sheetName).GetRow(rowNumber)

    List.indexed range
    |> List.iter (fun (i, _) -> row.CreateCell(i) |> ignore)

let initCells (workbook: XSSFWorkbook) sheetName ranges =
    List.indexed ranges
    |> List.iter (fun (i, r) -> initCellsOfRow workbook sheetName i r)

let cellToXLS (row: IRow) (column: int) (cell: Cell) =
    match cell with
    | (CellFormula f, _) ->
        let formula = f.Substring(1) // Remove leading '='
        row.GetCell(column).SetCellFormula(formula)
    | (CellValue v, _) -> row.GetCell(column).SetCellValue(v)

let rangeToXLS (row: IRow) (range: CellRange) =
    List.indexed range
    |> List.iter (fun (column, cell) -> cellToXLS row column cell)

let sheetToXLS (workbook: XSSFWorkbook) sheetName (ranges: list<CellRange>) =
    initSheet workbook sheetName
    initRows workbook sheetName ranges
    initCells workbook sheetName ranges

    List.indexed ranges
    |> List.iter (fun (i, range) ->
        let row = workbook.GetSheet(sheetName).GetRow(i)
        rangeToXLS row range)

let autoSizeColumns (workbook: XSSFWorkbook) sheetName ranges =
    // TODO: get from ranges
    let columnMax = 42

    for c in 0 .. columnMax do
        workbook.GetSheet(sheetName).AutoSizeColumn(c)


let createWorkbook fileName sheetRanges =
    let workbook = new XSSFWorkbook()

    sheetRanges
    |> List.iter (fun (sheetName, ranges) ->
        sheetToXLS workbook sheetName ranges
        autoSizeColumns workbook sheetName ranges)

    workbook.Write(new FileStream(fileName, FileMode.Create))
