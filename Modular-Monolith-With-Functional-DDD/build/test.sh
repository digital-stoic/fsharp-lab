#!/bin/sh

which dotnet >/dev/null || { echo "Error: 'dotnet' not found."; exit 1; }

dotnet fake run build.fsx --target "Test"