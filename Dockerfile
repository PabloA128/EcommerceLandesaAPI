# Imagen base para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Imagen para compilar la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar las dependencias
COPY ["EcommerceAPI/EcommerceAPI.csproj", "EcommerceAPI/"]
RUN dotnet restore "EcommerceAPI/EcommerceAPI.csproj"

# Copiar el resto del código fuente
COPY . .

# Cambiar al directorio del proyecto para build
WORKDIR "/src/EcommerceAPI"

# Construir la app
RUN dotnet build "EcommerceAPI.csproj" -c Release -o /app/build

# Publicar la app
FROM build AS publish
RUN dotnet publish "EcommerceAPI.csproj" -c Release -o /app/publish

# Imagen final para la ejecución
FROM base AS final
WORKDIR /app

# Copiar el resultado publicado desde el contenedor de build
COPY --from=publish /app/publish .

# Definir el punto de entrada
ENTRYPOINT ["dotnet", "EcommerceAPI.dll"]
