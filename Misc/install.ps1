$repo = "shashisadasivan/SSD365VSAddIn"
$releases = "https://api.github.com/repos/$repo/releases"
#Enter the Flder where you want the files to be downloaded
$addinPath = "C:\d365_VS_AddIns" 

If(!(test-path $addinPath))
{
    New-Item -ItemType Directory -Force -Path $addinPath
}
cd $addinPath

Write-Host Determining latest release
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$tag = (Invoke-WebRequest -Uri $releases -UseBasicParsing | ConvertFrom-Json)[0].tag_name

#Write-Host $tag

$files = @("Newtonsoft.Json.dll",  "SSD365VSAddIn.dll", "Microsoft.VisualStudio.Interop.dll", "envdte80.dll")

Write-Host Downloading files
foreach ($file in $files) 
{
    $download = "https://github.com/$repo/releases/download/$tag/$file"
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-WebRequest $download -Out $file
    Unblock-File $file
}
Write-Host "Adding path to configuration"
#Edit the config file "C:\Users\<username>\Documents\Visual Studio Dynamics 365\DynamicsDevConfig.xml" and add an entry for the above folder

$configRelativePath = "Visual Studio Dynamics 365\DynamicsDevConfig.xml"
$documentsPath = [Environment]::GetFolderPath([Environment+SpecialFolder]::MyDocuments)

$D365VSConfigFile = @(
    Join-Path $documentsPath $configRelativePath
    Join-Path $env:USERPROFILE "Documents\$configRelativePath"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

If(!$D365VSConfigFile)
{
    throw "DynamicsDevConfig.xml was not found. Please enter the add-in path into the configuration manually: $addinPath"
}

$AddPathToConfig = $true
[xml]$XMLDocument = Get-Content $D365VSConfigFile

foreach ($element in $XMLDocument.DynamicsDevConfig.AddInPaths.string)
{
    if($element.Equals($addinPath))
    {
        $AddPathToConfig = $false
    }
}

if($AddPathToConfig.Equals($true))
{
    $configStr = $XMLDocument.CreateElement("d2p1", "string", "http://schemas.microsoft.com/2003/10/Serialization/Arrays")
    $configstr.InnerText = $addinPath
    $XMLDocument.DynamicsDevConfig.AddInPaths.AppendChild($configStr)
    $XmlDocument.Save($D365VSConfigFile)
}

