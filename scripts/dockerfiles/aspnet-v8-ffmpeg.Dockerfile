FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN apt-get -y update
RUN apt-get -y upgrade
RUN apt-get install -y ffmpeg

EXPOSE 8080
EXPOSE 443