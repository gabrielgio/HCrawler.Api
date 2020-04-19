FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet publish --output /out --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT [ "dotnet", "HCrawler.Api.dll" ]
