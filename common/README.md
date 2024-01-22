# Common
[![Build and Publish - Common](https://github.com/Rahim373/utube/actions/workflows/common-build_and_publish.yml/badge.svg?branch=dev)](https://github.com/Rahim373/utube/actions/workflows/common-build_and_publish.yml)

This project combines necessary events, services and extension methods that is frequesntly used accross the system. This covers the follow for now -

1. [Serilog](https://serilog.net/)
1. [serilog-sinks-elasticsearch](https://github.com/serilog-contrib/serilog-sinks-elasticsearch)
1. [prometheus-net](https://github.com/prometheus-net/prometheus-net)
1. [Swagger](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-8.0)
1. [NRedisStack](https://github.com/redis/NRedisStack)


## Installation
Run the following command and this will be added to your project from GitHub [nuget](https://github.com/Rahim373/utube/pkgs/nuget/UTube.Common) repository.

```
dotnet add package UTube.Common
```

## Pre-Requisite
Add ``https://nuget.pkg.github.com/Rahim373/index.json`` as Nuget source before installing the project. Follow the example below -

```
dotnet nuget add source --username Rahim373 --password ghp_7ueasqgYBN********************0HUa0E --store-password-in-clear-text --name github "https://nuget.pkg.github.com/Rahim373/index.json"
```
Use your own username and github accss token.