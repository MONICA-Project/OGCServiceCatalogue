FROM microsoft/dotnet:2.1.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["OGCServiceCatalogue/OGCServiceCatalogue.csproj", "OGCServiceCatalogue/"]
RUN dotnet restore "OGCServiceCatalogue/OGCServiceCatalogue.csproj"
COPY . .
WORKDIR "/src/OGCServiceCatalogue"
RUN dotnet build "OGCServiceCatalogue.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OGCServiceCatalogue.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OGCServiceCatalogue.dll"]