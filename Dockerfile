FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app

COPY . .
RUN dotnet restore
RUN dotnet publish --output /out --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

RUN apt update
RUN apt install ffmpeg youtube-dl -y

WORKDIR /app
COPY --from=build /out .
ENTRYPOINT dotnet HCrawler.DB.dll; dotnet HCrawler.Api.dll

