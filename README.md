# Rest-Api

## Front-End project example

[front-end project](https://github.com/bossjien26/webpack-vue)

## Database

### How to start a database locally

```sh
docker compose up -d database
```

### How to setting allow connect in mysql

```mysql
CREATE USER 'User Account'@'Docker Database Ip' IDENTIFIED BY 'Password';
GRANT ALL PRIVILEGES ON *.* TO 'User Account'@'Docker Database Ip' WITH GRANT OPTION;
Flush Privileges;
```

### Database connection strings

Modify the `ConnectionStrings` in `DefaultConnection` at the following file

### development is use in develop env , production use in product env

> src/RestApi/ appsettings-Development.json  or appsettings-Production.json

### Redis

need use docker or self env build one redis env.

> docker-compose.yml

### Add Migration

```sh
#switch to src/Repositories
dotnet ef migrations add InitialCreate --context DbContextEntity --startup-project ../RestApi
```

### Update Database

```sh
#switch to src/Repositories
dotnet ef database update --context DbContextEntity --startup-project ../RestApi

#When run project show Failed executing DbCommand , because sql run ALTER DATABASE CURRENT COLLATE Chinese_Taiwan_Stroke_CI_AS , so need repeat update database.
dotnet ef database update -v --context DbContextEntity --startup-project ../RestApi
```

## Redis

```shell
#install redis image
docker compose up -d redis
```

### Use Package

```sh
dotnet add package ServiceStack.Redis
```

## Use Visual Studio

> Choice open `RestApi.sln` in root

## Project

### How run the project

```sh
#switch to src/RestApi
#use swagger api test
dotnet run
```

### Swagger api setting env

> launchSettings.json

### How build project

```sh
#switch to root
dotnet build
```

### How build project in docker

```sh
#switch to root 
docker-compose up -d cart_rest_api
```

### How clean build outputs

```sh
#switch to root
dotnet clean
```

### How restore dependencies specified

```sh
#switch to root
dotnet restore
```

###

## Test

### How reload test

```sh
#switch to the tests project
dotnet watch test
```

### Run test

[Example Url](https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests?pivots=mstest)

```sh
#run all tests
dotnet test
```

### How to test project

```sh
#run project
dotnet test Directory/Test.csproj
```

```sh
#filter the test method name
dotnet test Directory/Test.csproj --filter MethodName
```

## Api example

### This is the route table for Api Example:

| VERB   | URL                                | DESCRIPTION                   |
| ------ | ---------------------------------- | ----------------------------- |
| GET    | api/ControllerRoute/MethodRoute    | Retrieves stock items         |
| GET    | api/ControllerRoute/MethodRoute/Id | Retrieves a stock item by id  |
| POST   | api/ControllerRoute/MethodRoute    | Create a new stock item       |
| PUT    | api/ControllerRoute/MethodRoute    | Update an existing stock item |
| DELETE | api/ControllerRoute/MethodRoute/Id | Delete an existing stock item |
