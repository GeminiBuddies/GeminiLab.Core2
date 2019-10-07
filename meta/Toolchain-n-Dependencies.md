# Toolchain

## 0. .NET Core

.NET Core 3.0 SDK and corresponding .NET Core CLI.

Used to:

- Build(and publish).
- Test and coverage report.

## 1. Make (optional)

Used to:

- Run build scripts.

## 2. Tools on .NET Core

### 2.1. `dotnet-reportgenerator-globaltool`

Can be installed with:

```
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Used to:

- Generate coverage report locally.
