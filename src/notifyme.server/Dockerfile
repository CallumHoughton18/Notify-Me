FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# need to give permission to /Data Stores folder so just removed for now

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app /app/Data Stores
# USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
RUN apt-get update && apt-get -y install nodejs                   
WORKDIR /src
COPY ["src/notifyme.server/notifyme.server.csproj", "src/notifyme.server/"]
RUN dotnet restore "src/notifyme.server/notifyme.server.csproj"
COPY . .
WORKDIR "/src/src/notifyme.server"
RUN dotnet build "notifyme.server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "notifyme.server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "notifyme.server.dll"]
