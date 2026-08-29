# =========================
# BUILD
# =========================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "HRMS_Web/HRMS_Web.csproj"

RUN dotnet publish "HRMS_Web/HRMS_Web.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# RUNTIME
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

# Render assigns the PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

ENTRYPOINT ["dotnet", "HRMS_Web.dll"]
