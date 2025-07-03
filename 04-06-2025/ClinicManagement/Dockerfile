# Base image used for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

RUN apt-get update && apt-get install -y openssl

RUN mkdir /https && \
    openssl req -x509 -nodes -days 365 \
    -newkey rsa:2048 \
    -keyout /https/aspnetcore.key \
    -out /https/aspnetcore.crt \
    -subj "/CN=localhost" && \
    openssl pkcs12 -export \
    -out /https/aspnetcore.pfx \
    -inkey /https/aspnetcore.key \
    -in /https/aspnetcore.crt \
    -passout pass:password

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy everything into container
COPY . .

# Debug Step 1: List root contents
RUN echo "Contents of /src:" && ls -la /src

# Debug Step 2: List contents of the project folder
RUN echo "Contents of /src/:" && ls -la /src/

# Set working directory to project folder
WORKDIR /src/

# Debug Step 3: Show Program.cs to confirm Main method exists
RUN echo "Displaying Program.cs:" && cat Program.cs || echo "Program.cs NOT FOUND"

# Optional: check if .csproj exists and is correct
RUN echo "Displaying ClinicManagement.csproj:" && cat ClinicManagement.csproj || echo "csproj NOT FOUND"


# Build the application
RUN dotnet build "./ClinicManagement.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClinicManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development

ENV ASPNETCORE_URLS="http://+:8080;https://+:8081"

ENTRYPOINT ["dotnet", "ClinicManagement.dll"]
