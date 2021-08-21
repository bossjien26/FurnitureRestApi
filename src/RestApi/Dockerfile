FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "RestApi.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["../RestApi/RestApi.csproj","."]
COPY ["../Services/Services.csproj","../Services/"]
COPY ["../Entities/Entities.csproj","../Entities/"]
COPY ["../Repositories/Repositories.csproj","../Repositories/"]
COPY ["../Middlewares/Middlewares.csproj","../Middlewares/"]
COPY ["../Helpers/Helpers.csproj","../Helpers/"]
COPY ["../Enum/Enum.csproj","../Enum/"]
RUN dotnet restore "RestApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "RestApi.csproj" -c Release -o /app/build

FROM build As publish
RUN dotnet publish "RestApi.csproj" -c Release -o /app/publish

FROM base As final
WORKDIR /app
ENV TZ Asia/Taipei
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet" ,"RestApi.dll"]

#VOLUME ["RestApi.csproj","./" ]

#RUN dotnet ef database update

