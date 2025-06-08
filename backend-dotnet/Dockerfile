FROM mcr.microsoft.com/dotnet/sdk:9.0-preview
WORKDIR /app

COPY RESTProject.csproj ./
RUN dotnet restore RESTProject.csproj

COPY . ./
RUN dotnet publish RESTProject.csproj -c Release -o out

ENTRYPOINT ["dotnet", "out/RESTProject.dll"]
