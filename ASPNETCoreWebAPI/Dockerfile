FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ASPNETCoreWebAPI.csproj", "."]
RUN dotnet restore "./ASPNETCoreWebAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ASPNETCoreWebAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ASPNETCoreWebAPI.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# It is not recommended to copy certificates into the image
RUN mkdir -p /app/certificates
COPY ./Certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "ASPNETCoreWebAPI.dll"]