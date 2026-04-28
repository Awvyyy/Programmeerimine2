# Run this from the root folder of Programmeerimine2_MediaLibrary.
# It replaces two files that caused the remaining test failures.

Copy-Item .\_testfix_patch\KooliProjekt\Services\Services.cs .\KooliProjekt\Services\Services.cs -Force
Copy-Item .\_testfix_patch\KooliProjekt\Controllers\MediaItemsApiController.cs .\KooliProjekt\Controllers\MediaItemsApiController.cs -Force

Write-Host "Patch applied. Now run: dotnet clean; dotnet test"
