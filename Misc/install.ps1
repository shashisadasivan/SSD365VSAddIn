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

$files = @("Newtonsoft.Json.dll",  "SSD365VSAddIn.dll")

Write-Host Downloading files
foreach ($file in $files) 
{
    $download = "https://github.com/$repo/releases/download/$tag/$file"
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Invoke-WebRequest $download -Out $file
    Unblock-File $file
}

#TODO: Edit the config file "C:\Users\<username>\Documents\Visual Studio Dynamics 365\DynamicsDevConfig.xml" and add an entry for the above folder
