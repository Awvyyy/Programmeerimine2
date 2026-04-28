## Run project

dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run

Debug seed creates user:

- admin@example.com
- Password123!

powershell
dotnet test
./run-tests.ps1