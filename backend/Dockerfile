# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY GymApp.API/*.csproj GymApp.API/
COPY GymApp.Core/*.csproj GymApp.Core/
COPY GymApp.Infrastructure/*.csproj GymApp.Infrastructure/
RUN dotnet restore "GymApp.API/GymApp.API.csproj"

# Copy the rest of the source code
COPY . .

# Publish the application
WORKDIR "/src/GymApp.API"
RUN dotnet publish "GymApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ----- Runtime Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GymApp.API.dll"]