FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["texo.api/texo.api.csproj", "texo.api/"]
RUN dotnet restore "texo.api/texo.api.csproj"
COPY . .
WORKDIR "/src/texo.api"
RUN dotnet build "texo.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "texo.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "texo.api.dll"]
