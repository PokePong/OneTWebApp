FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OneTWebApp/OneTWebApp/OneTWebApp.csproj", "OneTWebApp/OneTWebApp/"]
COPY ["OneTWebApp/OneTWebApp.Client/OneTWebApp.Client.csproj", "OneTWebApp/OneTWebApp.Client/"]
RUN dotnet restore "OneTWebApp/OneTWebApp/OneTWebApp.csproj"
COPY . .
WORKDIR "/src/OneTWebApp/OneTWebApp"
RUN dotnet build "OneTWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OneTWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OneTWebApp.dll"]