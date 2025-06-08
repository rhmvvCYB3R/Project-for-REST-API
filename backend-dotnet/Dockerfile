FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app

# Копируем только csproj и восстанавливаем зависимости
COPY RESTProject.csproj ./
RUN dotnet restore RESTProject.csproj

# Копируем все остальные файлы в контейнер
COPY . ./

# Публикуем в папку out
RUN dotnet publish RESTProject.csproj -c Release -o out

# Запускаем приложение
ENTRYPOINT ["dotnet", "out/RESTProject.dll"]
