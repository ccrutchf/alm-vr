# Add MyGet repo
choco source add -n="Alm-VR" -s="https://www.myget.org/F/alm-vr/api/v2/package"

& choco install dotnetcore-sdk --source Alm-VR --version 2.1.300-rc1 -y --no-progress

# Force Unity version.
& choco install unity --version 2018.1.0 -y --no-progress