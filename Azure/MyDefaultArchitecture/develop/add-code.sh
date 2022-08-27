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
[<EntryPoint>]
let main _ =
    printfn "This program should not be run in standalone mode"
    0
EOF

# dotnet sln add $FOLDER