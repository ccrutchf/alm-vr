# Force Unity version.
& choco install unity --version 2018.1.0 -y

[System.Diagnostics.FileVersionInfo]::GetVersionInfo("C:\Program Files\Unity\Editor\Unity.exe")