FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /ProductExercise

COPY . ./

RUN dotnet restore

WORKDIR /ProductExercise/WebApi
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /ProductExercise

COPY --from=build /ProductExercise/WebApi/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "WebApi.dll"]