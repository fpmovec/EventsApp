FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app/backend
EXPOSE 5299

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release
COPY ["./Web/Web.csproj", "./Web/"]
COPY ["./Infrastructure/Infrastructure.csproj", "./Infrastructure/"]
COPY ["./Domain/Domain.csproj", "./Domain/"]
COPY ["./Application/Application.csproj", "./Application/"]
RUN dotnet restore "./Web/Web.csproj"
COPY . .
WORKDIR "/src/Web/"
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/backend/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.csproj" -c $BUILD_CONFIGURATION -o /app/backend/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app/backend
COPY --from=publish /app/backend/publish .
ENTRYPOINT ["dotnet", "Web.dll"]