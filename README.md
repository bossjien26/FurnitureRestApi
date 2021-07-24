# FurnitureRestApi

## Database

### How to start a database locally

```sh
docker compose up -d db
```

### Database connection strings
Modify the `ConnectionStrings` in `DefaultConnection` at the following file

>src/RestApi/appsettings.json


TODO:Update Entity and Repository directory need combine
### Add Migration
```sh
#switch to src/Repositories
dotnet ef migrations add InitialCreate --context DbContextEntity --startup-project ../RestApi
```
### Update Database

```sh
#switch to src/Repositories
dotnet ef database update --context DbContextEntity --startup-project ../RestApi
```

## Test

### How reload test

```sh
#switch to the tests project
dotnet watch test
```

### Run test

[Example Url](https://docs.microsoft.com/en-us/dotnet/core/testing/selective-unit-tests?pivots=mstest)

```sh
#filter the test method
dotnet test --filter Method
```

```sh
#run all tests
dotnet test
```

### This is the route table for Api Example:

| VERB | URL | DESCRIPTION |
| ------------- | ------------- | ------------- |
| GET | api/ControllerRoute/MethodRoute | Retrieves stock items |
| GET | api/ControllerRoute/MethodRoute/Id | Retrieves a stock item by id |
| POST| api/ControllerRoute/MethodRoute | Create a new stock item |
| PUT | api/ControllerRoute/MethodRoute | Update an existing stock item |
| DELETE | api/ControllerRoute/MethodRoute/Id | Delete an existing stock item |
