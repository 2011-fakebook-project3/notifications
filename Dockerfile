#base image + dependencies
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY  FakebookNotifications/*.sln .
COPY FakebookNotifications/FakebookNotifications.WebApi/*.csproj ./FakebookNotifications.WebApi/
COPY FakebookNotifications/FakebookNotifications.Testing/*.csproj ./FakebookNotifications.Testing/
RUN dotnet restore

# Copy everything else and build
COPY . ./
COPY FakebookNotifications/FakebookNotifications.WebApi/. ./FakebookNotifications.WebApi/
COPY FakebookNotifications/FakebookNotifications.Testing/. ./FakebookNotifications.Testing/
RUN dotnet publish -c Release -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "FakebookNotifications.WebApi.dll"]



# To Run:
# 1. docker build -t fakebooknotifications .
# 2. docker run fakebooknotifications