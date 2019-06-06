#Copy all the DLL's in the containing directory to the visual studio extension manager folder

$RegistryLocation = "HKCU:\SOFTWARE\Microsoft\VisualStudio\14.0\ExtensionManager\EnabledExtensions\"
$ExtensionDir = ""
$AddinExtensionDir = ""

#Find the Extension Folder
Push-Location
Set-Location -Path $RegistryLocation
Get-Item . | Select-Object -ExpandProperty property | ForEach-Object {

        if($_ -like "DynamicsRainier*") { 
            $ExtensionDir = (Get-ItemProperty -Path . -Name $_).$_
        #New-Object psobject -Property @{“property”=$_;
        #“Value” = (Get-ItemProperty -Path . -Name $_).$_}
        }

        Pop-Location
}

#Write-Output $ExtensionDir

If($ExtensionDir.Length -gt 0) {
    $AddinExtensionDir = "$ExtensionDir\AddinExtensions"
    #Write-Output $AddinExtensionDir
    Get-ChildItem -Path ".\*.dll" | ForEach-Object {
        #Copy the DLL's in the given directory
        $fileName = $_.Name
        Copy-Item -Path $_.Name -Destination "$AddinExtensionDir\$fileName"
        Write-Output "Copied file: $fileName" 
    }
} else {
    Write-Output "Could not find extension folder"
}