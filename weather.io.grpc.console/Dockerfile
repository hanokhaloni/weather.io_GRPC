#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["weather.io.grpc.console/weather.io.grpc.console.csproj", "weather.io.grpc.console/"]
RUN dotnet restore "weather.io.grpc.console/weather.io.grpc.console.csproj"
COPY . .
WORKDIR "/src/weather.io.grpc.console"
RUN dotnet build "weather.io.grpc.console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "weather.io.grpc.console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "weather.io.grpc.console.dll"]