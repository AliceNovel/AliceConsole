# ALiCE CONSOLE

Alice Console is the useful tool to read `.anov` file and create `.anproj` fotmat.

## How to use

### Requirement

- Git
- .NET SDK 10.0.100 (or a later version)

### Install

#### From Nuget.org

```sh
dotnet tool install --global AliceProject.AliceConsole
```

#### From Source

1. Clone this repository
1. Pack source with .NET CLI
    ```sh
    cd src/AliceConsole
    dotnet pack 
    ```
1. Install this solution
    ```sh
    dotnet tool install --global --add-source ./nupkg AliceProject.AliceConsole
    ```

### Uninstall

```sh
dotnet tool uninstall --global AliceProject.AliceConsole
```

### Usage

Please show: `aliceconsole -h`
