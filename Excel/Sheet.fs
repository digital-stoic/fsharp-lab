module Sheet

open System.IO
open NPOI.XSSF.UserModel

let createWorkbook fileName =
    let workbook = new XSSFWorkbook()
    let sheet = workbook.CreateSheet()
    let row = sheet.CreateRow(0)
    let cell = row.CreateCell(0)
    cell.SetCellValue("Hello World")
    workbook.Write(new FileStream(fileName, FileMode.Create))

let fileName = @"out/test.xlsx"

let run = createWorkbook fileName
