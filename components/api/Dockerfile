FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS env
WORKDIR /build
COPY ./ScoreTracking.Build.sln ./
COPY ./src/scoretracking-api/ScoreTracking.API.csproj ./src/scoretracking-api/
COPY ./src/scoretracking-app/ScoreTracking.App.csproj ./src/scoretracking-app/
RUN dotnet restore ScoreTracking.Build.sln

FROM env AS build-env
WORKDIR /build
COPY ./src ./src
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION --no-restore -o ./out

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine as runtime
WORKDIR /app
COPY --from=build-env /build/out .
ENV ASPNETCORE_ENVIRONMENT="Docker"
ENV ASPNETCORE_URLS="http://+:80"
EXPOSE 80
ENTRYPOINT ["dotnet", "ScoreTracking.API.dll"]