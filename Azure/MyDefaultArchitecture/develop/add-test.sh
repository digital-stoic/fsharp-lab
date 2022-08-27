#!/bin/sh

FOLDER="$1"
NAMESPACE="$2"
if [ -z "$FOLDER" -o -z "$NAMESPACE" ]; then
    echo "Usage: $0 <code-folder> <code-namespace>"
    exit 1
fi

mkdir -p $FOLDER
dotnet new console -lang 'F#' -o $FOLDER -n $NAMESPACE

cat > $FOLDER/Program.fs <<EOF
open Expecto
open MyCompany.MyApp.Common.Test.Helper

[<Tests>]
let tests =
    testList "MyCompany.MyApp.XYZ"
    <| testListAppend [ ]

[<EntryPoint>]
let main argv =
    Tests.runTestsInAssemblyWithCLIArgs [] argv
EOF

cd $FOLDER
dotnet add package Expecto

# dotnet sln add $FOLDER