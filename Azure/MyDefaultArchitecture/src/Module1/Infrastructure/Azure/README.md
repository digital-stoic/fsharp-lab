# Module1

## Setup

From https://medium.com/datarisk-io/introdu%C3%A7%C3%A3o-ao-azure-functions-em-f-e083727662ed:
```bash
# func init MyFunctionProj
func init --worker-runtime dotnet --target-framework net6.0 Function1
mv MyFunctionProj.csproj MyFunctionProj.fsproj 
vi MyFunctionProj.fsproj 
dotnet new --install Microsoft.Azure.WebJobs.ItemTemplates
```

## Develop

```bash
dotnet build 
func host start
```

# Reference

* https://docs.microsoft.com/en-us/azure/azure-functions/functions-core-tools-reference?tabs=v2
