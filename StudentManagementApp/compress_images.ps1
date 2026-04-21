Add-Type -AssemblyName System.Drawing

function Compress-Image {
    param (
        [string]$SourcePath,
        [string]$DestinationPath,
        [long]$Quality = 70
    )

    $img = [System.Drawing.Image]::FromFile($SourcePath)
    $encoder = [System.Drawing.Imaging.ImageCodecInfo]::GetImageEncoders() | Where-Object { $_.FormatDescription -eq "JPEG" }
    $encoderParameters = New-Object System.Drawing.Imaging.EncoderParameters(1)
    $encoderParameters.Param[0] = New-Object System.Drawing.Imaging.EncoderParameter([System.Drawing.Imaging.Encoder]::Quality, $Quality)
    
    $img.Save($DestinationPath, $encoder, $encoderParameters)
    $img.Dispose()
    Write-Host "Compressed $SourcePath to $DestinationPath (Quality: $Quality%)"
}

# Compress Logo and Banner
Compress-Image "D:\CODE\NET\PRN222\Razor_Page_English_Center\TEAM_FILES\logo.png" "D:\CODE\NET\PRN222\Razor_Page_English_Center\StudentManagementApp\wwwroot\images\logo.jpg" 70
Compress-Image "D:\CODE\NET\PRN222\Razor_Page_English_Center\TEAM_FILES\banner.png" "D:\CODE\NET\PRN222\Razor_Page_English_Center\StudentManagementApp\wwwroot\images\banner.jpg" 70
