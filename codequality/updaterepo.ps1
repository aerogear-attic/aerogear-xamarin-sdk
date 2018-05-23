$ErrorActionPreference = "Stop"

if (!(Test-Path env:SOLUTION_DIR) -and !(Test-Path env:REPO_DIR)) { 
    write-host "ERROR!!!"
    write-host "Please set the following environment variables:"
    write-host "SOLUTION_DIR: must point to the aerogear-xamarin-sdk folder"
    write-host "REPO_DIR: must point to the folder you want to use as local nuget repository"
    Break
} 

$SOLUTION_DIR=$env:SOLUTION_DIR
$REPO_DIR=$env:REPO_DIR

write-host "Solution in $SOLUTION_DIR"
write-host "Repository in $REPO_DIR"

function CheckLastExitCode($command) {
    if ($LastExitCode -ne 0) {
       throw "'$command' has failed";
    }
}

Try {
    $a = New-Item "$REPO_DIR" -Force -ItemType Directory

    $nuspecfiles = dir -Path "$SOLUTION_DIR" -Filter *.nuspec -Recurse | %{$_.FullName} | Select-String -Pattern "Debug|Release" -NotMatch

    foreach ($nuspec in $nuspecfiles) {
        # The nuspec file is configured to target the 'release' dll. We want to change it to target the `debug` dll
      (Get-Content $nuspec).replace('\Release\', '\Debug\') | Set-Content $nuspec
        $nugetout = nuget pack "$nuspec"
        CheckLastExitCode("nuget pack")

        $packageString = ($nugetout -match "package '(.*)'")[0]
        $a = $packageString -match "package '(.*)'"

        $nupkg = $matches[1]

        # Revert to release
        (Get-Content $nuspec).replace('\Debug\', '\Release\') | Set-Content $nuspec
        nuget add "nupkg" -source "$REPO_DIR"
        CheckLastExitCode("nuget add")
    }


    write-host "Repository successfully created/updated in $REPO_DIR"
} 
Catch {
   write-host "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"
   write-host "ERROR: An error has occurred while creating the repository"
   write-host "Please check that you have successfully compiled the aerogear-xamarin-sdk modules and that you have used the DEBUG profile"
   write-host "Details:"
   write-host "$($($_.InvocationInfo.ScriptLineNumber)): $_"
}