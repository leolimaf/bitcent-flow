FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BitcentFlow.Auth/BitcentFlow.Auth.csproj", "BitcentFlow.Auth/"]
RUN dotnet restore "BitcentFlow.Auth/BitcentFlow.Auth.csproj"
COPY . .
WORKDIR "/src/BitcentFlow.Auth"
RUN dotnet build "BitcentFlow.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BitcentFlow.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitcentFlow.Auth.dll"]
