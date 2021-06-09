## Migrations
cd to notifyme.server
dotnet ef migrations add MIGRATIONAME --context notifymecontext -p ../notifyme.infrastructure/notifyme.infrastructure.csproj -s notifyme.server.csproj -o Data/Migrations

dotnet ef database update MIGRATIONAME --context notifymecontext -p ../notifyme.infrastructure/notifyme.infrastructure.csproj -s notifyme.server.csproj