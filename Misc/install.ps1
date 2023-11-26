$repo = "shadowchamber/SSD365VSAddIn"
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

$files = @("Newtonsoft.Json.dll",  "SSD365VSAddIn.dll")

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

$D365VSConfigFile = "$env:USERPROFILE\Documents\Visual Studio Dynamics 365\DynamicsDevConfig.xml"
If(!(test-path $addinPath))
{
    throw "Please enter the path into configuration manually"
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

