FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["ProductExercise.sln", "."]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["ProductExercise/Models.csproj", "ProductExercise/"]
COPY ["WebApi/WebApi.csproj", "WebApi/"]

RUN dotnet restore "ProductExercise.sln"

COPY . .

WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
