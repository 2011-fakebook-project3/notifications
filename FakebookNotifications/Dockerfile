#base image + dependencies
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY  *.sln .
COPY FakebookNotifications.WebApi/*.csproj ./FakebookNotifications.WebApi/
COPY FakebookNotifications.Testing/*.csproj ./FakebookNotifications.Testing/
COPY FakebookNotifications.DataAccess/*.csproj ./FakebookNotifications.DataAccess/
RUN dotnet restore

# Copy everything else and build
COPY . ./
COPY FakebookNotifications.WebApi/. ./FakebookNotifications.WebApi/
COPY FakebookNotifications.Testing/. ./FakebookNotifications.Testing/
COPY FakebookNotifications.DataAccess/. ./FakebookNotifications.DataAccess/
RUN dotnet publish FakebookNotifications.WebApi/FakebookNotifications.WebApi.csproj -c Release -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "FakebookNotifications.WebApi.dll"]



# To Run:
# 1. docker build -t fakebooknotifications .
# 2. docker run fakebooknotifications