COPY RESTProject.csproj ./
RUN dotnet restore RESTProject.csproj
COPY . ./
RUN dotnet publish RESTProject.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "RESTProject.dll"]
