#base image + dependencies
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet public -c Release -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "FakebookNotifications.WebApi.dll"]



# To Run:
# 1. docker build -t FakebookNotification.WebApi .
# 2. docker run FakebookNotification.WebApi