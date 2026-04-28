# Run this from solution root: Programmeerimine2_MediaLibrary
# It overwrites only the files that had build errors in the log.

$ErrorActionPreference = "Stop"
$Root = Get-Location
$PatchRoot = Join-Path $Root "_buildfix_patch"

Copy-Item "$PatchRoot\KooliProjekt.WpfApp\Api\ApiClient.cs" "$Root\KooliProjekt.WpfApp\Api\ApiClient.cs" -Force
Copy-Item "$PatchRoot\KooliProjekt.BlazorApp\_Imports.razor" "$Root\KooliProjekt.BlazorApp\_Imports.razor" -Force
Copy-Item "$PatchRoot\KooliProjekt.BlazorApp\Shared\MainLayout.razor" "$Root\KooliProjekt.BlazorApp\Shared\MainLayout.razor" -Force
Copy-Item "$PatchRoot\KooliProjekt\Views\Shared\Components\Pager\Default.cshtml" "$Root\KooliProjekt\Views\Shared\Components\Pager\Default.cshtml" -Force
Copy-Item "$PatchRoot\KooliProjekt\KooliProjekt.csproj" "$Root\KooliProjekt\KooliProjekt.csproj" -Force

Write-Host "Patch files copied. Now run: dotnet clean; dotnet restore; dotnet build" -ForegroundColor Green
