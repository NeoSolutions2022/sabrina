FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sabrina/src/Monitor/Monitor.csproj", "Monitor/"]
RUN dotnet restore "src/Monitor/Monitor.csproj"
COPY . .
WORKDIR "/src/Monitor"
RUN dotnet build "Monitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Monitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Monitor.dll"]
