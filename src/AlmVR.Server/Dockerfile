FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
EXPOSE 80

# Copy all of the build
COPY ./artifacts /app

ENTRYPOINT [ "dotnet", "AlmVR.Server.dll" ]