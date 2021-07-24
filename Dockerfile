FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "mvc.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["mvc.csproj","."] 
RUN dotnet restore "mvc.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "mvc.csproj" -c Release -o /app/build

FROM build As publish
RUN dotnet publish "mvc.csproj" -c Release -o /app/publish

FROM base As final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet" ,"mvc.dll"]

#VOLUME ["mvc.csproj","./" ]

#RUN dotnet ef database update

