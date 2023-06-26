#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["carrefour-challenge/carrefour-challenge.csproj", "carrefour-challenge/"]
RUN dotnet restore "carrefour-challenge/carrefour-challenge.csproj"
COPY . .
WORKDIR "/src/carrefour-challenge"
RUN dotnet build "carrefour-challenge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "carrefour-challenge.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "carrefour-challenge.dll"]