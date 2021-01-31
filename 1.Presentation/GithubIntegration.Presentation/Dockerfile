#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["1.Presentation/GithubIntegration.Presentation/GithubIntegration.Presentation.csproj", "1.Presentation/GithubIntegration.Presentation/"]
RUN dotnet restore "1.Presentation/GithubIntegration.Presentation/GithubIntegration.Presentation.csproj"
COPY . .
WORKDIR "/src/1.Presentation/GithubIntegration.Presentation"
RUN dotnet build "GithubIntegration.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GithubIntegration.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GithubIntegration.Presentation.dll"]