# lap-template-v2
Template for LAP v2. Easier app without docker and stuff.



# Technology
- C#
- Visual Studio
- MSSQL (localdb)
- Blazor
- FluentUI
- Entity Framework
- ASP.NET Identity
- Aspire.NET


# Usefull Commands
## Entity Framework
First change directory to project with ef stuff:
- cd OnlineShop.ApiService

Testen ob dotnet-ef installiert:
- dotnet ef --version

Wenn kein dotnet-ef installiert:
- dotnet tool update dotnet-ef --version 8.0.14 --global

Wenn bereits Migrations verfügbar direkt update
- dotnet ef database update --context "AppDbContext"

Migration hinzufügen
- dotnet ef migrations add "migrationname" --context "AppDbContext"

Migration rückgängig
- dotnet ef migrations remove --context "AppDbContext"

Database update after migration
- dotnet ef database update --context "AppDbContext"