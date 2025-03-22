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
Wenn kein dotnet-ef installiert:
- dotnet tool update dotnet-ef --version 8.0.14 --global

Migration hinzufügen
- dotnet ef migrations add "migrationname" --context "AppDbContext"

Database update after migration
- dotnet ef database update --context "AppDbContext"