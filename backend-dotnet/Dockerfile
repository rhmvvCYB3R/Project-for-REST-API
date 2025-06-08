FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# Копируем только файл проекта с кавычками из-за пробелов
COPY "REST project.csproj" ./

# Восстанавливаем зависимости
RUN dotnet restore "REST project.csproj"

# Копируем весь остальной контент (код)
COPY . ./

# Публикуем проект
RUN dotnet publish "REST project.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app

# Копируем собранное приложение из стадии build
COPY --from=build /app/out .

# Запускаем
ENTRYPOINT ["dotnet", "REST project.dll"]
