# Como levantar localmente la api: 

## Desde consola: 

1. Ingresar a la carpeta `src` del proyecto
2. Correr el comando `dotnet build`
3. Correr el comando `dotnet run --project Host/Host.csproj`
4. Una vez que la consola muestre el mensaje:

```
info: Aspire.Hosting.DistributedApplication[0]
      Distributed application started. Press Ctrl+C to shut down.
```
4a. Abrir la URL `https://localhost:17064/` para acceder al tablero de Aspire
4b. Abrir la URL `https://localhost:7165/scalar/v1` para acceder a la pagina de la API desde donde se puede invocar.

## Invocar la API

1. Ingresar un JWT en la cabecera de autenticación
<img width="1155" height="500" alt="image" src="https://github.com/user-attachments/assets/1ddc7208-3bf6-479e-996c-2972c36e24e3" />
2. Scrollear hasta encontrar el endpoint que se desee testear e invocarlo, tener en cuenta que el id de evento que se usa para la data mockeada es `955a8a87-7a18-4a59-ac3c-03e73a53cff0`


