# :boom::rocket: UTube :rocket::boom:
A simple youtube like video streaming platform based on microservice architecture.

[![CodeQL](https://github.com/Rahim373/utube/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/Rahim373/utube/actions/workflows/github-code-scanning/codeql)

## :computer: Services
* Storage Service
* Video Service
* Process Service
* Reverse Proxy (BFF)
* Web Client

## :hammer: Technology Used
* ASP.NET Core 8
* MongoDB
* gRPC
* RabbitMQ
* [Minio](https://min.io/) (Object storage)
* ffmpeg
* ELK (Elastic, Logstash, Kibana) for logging
* Vue 3

## :nut_and_bolt: Architecture (Current)
![utube.drawio.png](/utube.drawio.png)

### :speak_no_evil: Note
This is an ongoing project. If you love to contribute, feel fee to create a PR.