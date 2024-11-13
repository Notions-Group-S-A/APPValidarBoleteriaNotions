https://htmlcolorcodes.com/es/


Dependencias

Instalado en este proyecto.
```
dotnet add package BarcodeScanner.Mobile.Maui  8.0.40.1
```

otros recomiendan instalarlo así:

```bash
dotnet add package BarcodeScanner.Mobile.Maui --prerelease -s=https://api.nuget.org/v3/index.json
dotnet add MauiQR.csproj package Microsoft.Maui.Controls.Compatibility --prerelease -s=https://api.nuget.org/v3/index.json
```

Construcción
Cuando falla visual - la construcción por comando fuerza a que baja las librerias
```bash
dotnet clean
dotnet restore
dotnet build -f net8.0-android34.0
```

Actualiza Microsoft.Android.Sdk.Windows
```bash
dotnet workload update
```