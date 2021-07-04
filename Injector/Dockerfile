FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Injector/Injector.csproj", "Injector/"]
RUN dotnet restore "Injector/Injector.csproj"
COPY . .
WORKDIR "/src/Injector"
RUN dotnet build "Injector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Injector.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Injector.dll"]
