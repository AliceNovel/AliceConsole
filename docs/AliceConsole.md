# ALiCE CONSOLE

Alice Console is the useful tool to read `.anov` file and create `.anproj` fotmat.

## How to use

### Requirement

- Git
- .NET CLI  
(.NET SDK 8.0.100 or a later version)

### Install

1. Clone this repository
1. Pack source with .NET CLI
    ```sh
    cd AliceConsole
    dotnet pack 
    ```
1. Install this solution
    ```sh
    dotnet tool install --global --add-source ./nupkg AliceConsole
    ```

### Uninstall

<!-- 
1. Check tool placement
    ```sh
    which aliceconsole
    # In my case: /home/lemon73/.dotnet/tools/aliceconsole
    ```
-->
1. Uninstall this tool
    ```sh
    # Linux, macOS
    dotnet tool install --tool-path ~/.dotnet/tools --add-source ./nupkg AliceConsole
    # Windows
    dotnet tool install --tool-path c:\dotnet-tools --add-source ./nupkg AliceConsole
    ```

### Usage

Please show: `aliceconsole -h`
