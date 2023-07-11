#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ScoreTracking/ScoreTracking.csproj", "ScoreTracking/"]
RUN dotnet restore "ScoreTracking/ScoreTracking.csproj"
COPY . .
WORKDIR "/src/ScoreTracking"
RUN dotnet build "ScoreTracking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScoreTracking.csproj"  

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScoreTracking.dll"]