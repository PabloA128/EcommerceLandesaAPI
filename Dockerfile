# Imagen base para ejecutar la app
FROM mcr.microsoft.comdotnetaspnet8.0 AS base
WORKDIR app
EXPOSE 80

# Imagen para compilar la app
FROM mcr.microsoft.comdotnetsdk8.0 AS build
WORKDIR src

# Copia todos los archivos al contenedor
COPY . .

# Publica el proyecto
RUN dotnet publish EcommerceAPI.csproj -c Release -o apppublish

# Imagen final
FROM base AS final
WORKDIR app
COPY --from=build apppublish .

# Comando para iniciar la app
ENTRYPOINT [dotnet, EcommerceAPI.dll]
