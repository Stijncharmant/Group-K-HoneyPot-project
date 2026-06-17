# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only csproj files first so "restore" is cached unless a .csproj changes.
# API references DataAccessLayer -> LogicLayer -> DomainModels, so all are needed.
COPY API/API.csproj API/
COPY DataAccessLayer/DataAccessLayer.csproj DataAccessLayer/
COPY LogicLayer/LogicLayer.csproj LogicLayer/
COPY DomainModels/DomainModels.csproj DomainModels/
RUN dotnet restore API/API.csproj

# Copy the rest of the sources for these projects and publish.
COPY API/ API/
COPY DataAccessLayer/ DataAccessLayer/
COPY LogicLayer/ LogicLayer/
COPY DomainModels/ DomainModels/
RUN dotnet publish API/API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Listen on 8080 (HTTP) inside the container.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]
