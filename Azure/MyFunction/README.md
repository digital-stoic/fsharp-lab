# My Function

## Setup

From https://medium.com/datarisk-io/introdu%C3%A7%C3%A3o-ao-azure-functions-em-f-e083727662ed:
```bash
func init MyFunctionProj
mv MyFunctionProj.csproj MyFunctionProj.fsproj 
vi MyFunctionProj.fsproj 
dotnet new --install Microsoft.Azure.WebJobs.ItemTemplates
```

## Develop

```bash
dotnet build 
func host start
```