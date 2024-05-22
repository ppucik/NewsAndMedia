FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ASPNETCoreWebAPI/ASPNETCoreWebAPI.csproj", "ASPNETCoreWebAPI/"]
RUN dotnet restore "./ASPNETCoreWebAPI/ASPNETCoreWebAPI.csproj"
COPY . .
WORKDIR "/src/ASPNETCoreWebAPI"
RUN dotnet build "./ASPNETCoreWebAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ASPNETCoreWebAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY ASPNETCoreWebAPI/Certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "ASPNETCoreWebAPI.dll"]