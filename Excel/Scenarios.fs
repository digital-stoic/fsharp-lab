module Scenarios

open ToXLS

// let range1: CellRange = [ C "42" "LT"; C "=$A$1 + 1" "" ]
// let range2: CellRange = [ C "43" "LT"; C "=$A$1 + 2" "" ]
// let ranges: list<CellRange> = [ range1; range2 ]

let updateOffset (offset: byref<_>) range =
    offset <- offset + (range |> List.length)

let mutable offset = 0

let rangeParameters =
    [ [ C "Parameters" "" ]
      [ C "Currency" ""; C "EUR" "" ]
      [ C "Costs" "" ]
      [ C "Equipment 100L cost per unit" ""
        C "60000" "" ]
      [ C "Equipment 100L units" ""
        C "Base growth" "" ] ]

let equipmentScenarioAddress = (5 + offset, 2)
updateOffset &offset rangeParameters

let rangeTest =
    [ [ C "42" ""
        C "=$A$1" ""
        A equipmentScenarioAddress "" ] ]

let mainRanges = rangeParameters @ rangeTest

offset <- 0

let headerInvestmentRange =
    [ [ C "Scenario" "", C "Investment" "", C "31/01/2023" "", C "28/02/2023" "", C "31/03/2023" "" ] ]

updateOffset &offset headerInvestmentRange

let equipment100LInvestmentRange =
    [ [ C "Base growth" "", C "Equipment 100L" "", C "1" "", C "1" "", C "" "" ]
      [ C "High growth" "", C "Equipment 100L" "", C "2" "", C "3" "", C "" "" ] ]

updateOffset &offset equipment100LInvestmentRange

let equipment100LCopperInvestmentRange =
    [ [ C "Base growth" "", C "Equipment 100L copper" "", C "2" "", C "3" "", C "" "" ]
      [ C "High growth" "", C "Equipment 100L copper" "", C "2" "", C "3" "", C "" "" ] ]

let fileName = @"out/test.xlsx"

let run =
    createWorkbook
        fileName
        [ ("Main", mainRanges)
          ("Investments", mainRanges) ]
    |> ignore
