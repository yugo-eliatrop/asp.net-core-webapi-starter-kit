ASP.NET Core WebApi template
============================
The application is based on the cross-platform .Net Core 3.0 platform developed by Microsoft.

Implemented functionality:
* Authentification, Authorization, user and role management using Microsoft Identity
* JWT tokens + Refresh tokens
* Work with Postgresql using Microsoft Entity Framework
* Example of Rest API (Books controller)
* Hosted services (Jobs)
* Swagger (/swagger/index.html)
* Asynchronous methods and controllers
* Docker for Production mode

## System dependencies

- Microsoft .Net Core 3.0.0
- Postgresql

or
- Docker, docker-compose

## Install

Clone the repo:
```sh
git clone ssh://git@gitlab.roonyx.team:2222/findbook/findbook-aspnet.git
vim docker-compose.yml
cd findbook-aspnet
```
Exaple of docker-compose file is below.

**There are two ways to install and run the app.** The first is the use of the .Net Core platform. The second is the use of Docker (the development mode is not implemented yet).

If .Net Core is installed on your system, run:
```sh
dotnet restore
dotnet run --seeds
```
To launch the application using Docker, run:
```sh
docker-compose build
docker-compose up
```
The application uses http://localhost:5000/

## Development mode

The mode is launched by `dotnet run` or `dotnet watch run` command. The mode is not implemented in docker.

## Production mode / Compilation

Using docker:
```sh
docker-compose build
docker-compose up
```
Using .Net Core:
```sh
dotnet publish -c Release
cd bin/Release
dotnet FindbookApi.dll
```

<hr>

## Example docker-compose file
```sh
version: '3.5'

networks: 
  fbnetwork:
    driver: bridge

services:
  api:
    build: 
      context: ./findbook-aspnet
      dockerfile: Production.dockerfile
    ports:
      - "5000:80"
    environment: 
      - DB_CONNECTION_STRING=host=db;database=findbook;username=postgres;password=postgres
    depends_on:
      - db
    networks: 
      - fbnetwork
  db:
    image: postgres:latest
    environment: 
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=findbook
    volumes:
      - ./tmp/db:/var/lib/postgresql/data
    networks: 
      - fbnetwork

```