FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app

COPY HCrawler.Api.sln .
COPY HCrawler.Api/HCrawler.Api.csproj ./HCrawler.Api/
COPY HCrawler.Core/HCrawler.Core.csproj ./HCrawler.Core/
COPY HCrawler.Test/HCrawler.Test.csproj ./HCrawler.Core/
RUN dotnet restore

COPY . .
RUN dotnet publish --output /out --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "HCrawler.Api.dll" ]
