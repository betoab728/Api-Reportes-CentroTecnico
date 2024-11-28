# Etapa 1: Compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos del proyecto y restaurar las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto de los archivos y construir la aplicación
COPY . ./
RUN dotnet publish -c Release -o /out

# Etapa 2: Ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Instalar las dependencias necesarias para procesar informes
RUN apt-get update && apt-get install -y \
    libgdiplus \
    libc6-dev \
    && rm -rf /var/lib/apt/lists/*

# Copiar los archivos compilados desde la etapa de construcción
COPY --from=build /out .

# Exponer el puerto en el que escucha tu API
EXPOSE 5017

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "ApiReportes.dll"]
