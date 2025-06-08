FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

COPY REST\ project.csproj ./
RUN dotnet restore REST\ project.csproj

COPY . ./
RUN dotnet publish REST\ project.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "REST project.dll"]
