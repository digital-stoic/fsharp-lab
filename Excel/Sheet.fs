module Sheet

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

let range1: CellRange = [ C "42" "LT"; C "=$A$1 + 1" "" ]
let range2: CellRange = [ C "43" "LT"; C "=$A$1 + 2" "" ]
let ranges: list<CellRange> = [ range1; range2 ]

let fileName = @"out/test.xlsx"

let initSheet (workbook: XSSFWorkbook) sheetName =
    workbook.CreateSheet(sheetName) |> ignore

let initRows (workbook: XSSFWorkbook) sheetName ranges =
    let sheet = workbook.GetSheet(sheetName)

    List.indexed ranges
    |> List.iter (fun (i, _) -> sheet.CreateRow(i) |> ignore)

let initCellsOfRow (workbook: XSSFWorkbook) sheetName rowNumber range =
    let row =
        workbook.GetSheet(sheetName).GetRow(rowNumber)

    List.indexed ranges
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

let toXLS (workbook: XSSFWorkbook) sheetName (ranges: list<CellRange>) =
    initSheet workbook sheetName
    initRows workbook sheetName ranges
    initCells workbook sheetName ranges

    List.indexed ranges
    |> List.iter (fun (i, range) ->
        let row = workbook.GetSheet(sheetName).GetRow(i)
        rangeToXLS row range)

let createWorkbook fileName =
    let workbook = new XSSFWorkbook()
    toXLS workbook "Main" ranges
    workbook.Write(new FileStream(fileName, FileMode.Create))

let run = createWorkbook fileName
