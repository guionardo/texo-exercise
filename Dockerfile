FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["texo.api/texo.api.csproj", "texo.api/"]
COPY ["texo.commons/texo.commons.csproj", "texo.commons/"]
COPY ["texo.data/texo.data.csproj", "texo.data/"]
COPY ["texo.domain/texo.domain.csproj", "texo.domain/"]
COPY ["texo.tests/texo.tests.csproj", "texo.tests/"]
COPY ["texo-exercise.sln", "texo-exercise.sln"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "texo.api" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "texo.api.dll"]
