FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore

WORKDIR /app/MonApi
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/MonApi/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "MonApi.dll"]