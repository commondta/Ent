FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore "HRMS_Web/HRMS_Web.csproj" --disable-parallel

RUN dotnet publish "HRMS_Web/HRMS_Web.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    --no-self-contained \
    -p:WarningLevel=0 \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    -p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

ENTRYPOINT ["dotnet", "HRMS_Web.dll"]
