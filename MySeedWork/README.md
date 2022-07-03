# My Seed Work

# Requirements

* [Install .Net 6 CLI](https://docs.microsoft.com/en-us/dotnet/core/install/linux)
* [Install FAKE build](https://fake.build/fake-gettingstarted.html#Install-FAKE)

# Build & Deploy

```bash
fake run run-build.fsx --target Build.Clean
fake run run-build.fsx --target Build.Clean
fake run run-build.fsx --target Deploy
```