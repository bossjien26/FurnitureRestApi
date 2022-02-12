FROM redis

#COPY redis.conf /usr/local/etc/redis/redis.conf
#CMD [ "redis-server", "/usr/local/etc/redis/redis.conf" ]
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
# EXPOSE 5000
WORKDIR /app
# ENTRYPOINT ["dotnet", "RestApi.dll"]

RUN apk add libgdiplus --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted && \
    apk add terminus-font


FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

WORKDIR /src

COPY ["src/RestApi/RestApi.csproj","src/RestApi/"]
COPY ["src/Services/Services.csproj","src/Services/"]
COPY ["src/Entities/Entities.csproj","src/Entities/"]
COPY ["src/Repositories/Repositories.csproj","src/Repositories/"]
COPY ["src/Middlewares/Middlewares.csproj","src/Middlewares/"]
COPY ["src/Helpers/Helpers.csproj","src/Helpers/"]
COPY ["src/Enum/Enum.csproj","src/Enum/"]
RUN dotnet restore "src/RestApi/RestApi.csproj"
COPY . .
WORKDIR "/src/src/RestApi"
RUN dotnet build "RestApi.csproj" -c Release -o /app/build

FROM build As publish
RUN dotnet publish "RestApi.csproj" -c Release -o /app/publish

FROM base As final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apk add --no-cache tzdata
ENV TZ Asia/Taipei
# ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "RestApi.dll"]
# RUN dotnet ef database update

#RUN dotnet ef database update

