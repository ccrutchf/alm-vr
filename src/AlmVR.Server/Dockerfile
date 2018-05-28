FROM microsoft/dotnet:2.1.0-rc1-aspnetcore-runtime-bionic

# Copy all of the build
COPY ./artifacts /app

ENTRYPOINT [ "dotnet", "/app/AlmVR.Server.dll" ]