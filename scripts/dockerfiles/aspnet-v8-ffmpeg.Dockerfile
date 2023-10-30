## Name : ghcr.io/letslearn373/utube/aspnet-v8-ffmpeg:latest
FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt-get -y update
RUN apt-get -y upgrade
RUN apt-get install -y ffmpeg

EXPOSE 80
EXPOSE 443