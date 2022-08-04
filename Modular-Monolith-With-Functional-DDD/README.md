# Modular Monolith with Functional DDD

This is an attempt to port to F# the great [Modular Monolith with DDD](https://github.com/kgrzybek/modular-monolith-with-ddd). This is also strongly inspired by [Domain Modeling Made Functional](https://fsharpforfunandprofit.com/ddd/).

This is a [work in progress](WIP.md).

## How to build

- [Install FAKE](https://fake.build/fake-gettingstarted.html#Install-FAKE):
  ```bash
  cd build
  dotnet tool restore
  ./clean.sh
  ./build.sh
  ./test.sh
  ```