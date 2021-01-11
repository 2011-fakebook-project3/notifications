#################- Build and Publish -#####################

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /app/src

# --------------------------------
COPY FakebookNotifications.WebApi/*.csproj ./FakebookNotifications.WebApi/
COPY FakebookNotifications.Testing/*.csproj ./FakebookNotifications.Testing/
COPY FakebookNotifications.DataAccess/*.csproj ./FakebookNotifications.DataAccess/
COPY FakebookNotifications.Domain/*.csproj ./FakebookNotifications.Domain/
COPY *.sln ./
RUN dotnet restore
# ---------------------------------

COPY . ./

RUN dotnet publish FakebookNotifications.WebApi -c Release -o ../publish

#################- Package Assemblies -###################

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime

WORKDIR /app

COPY --from=build /app/publish ./

CMD dotnet FakebookNotifications.WebApi.dll
