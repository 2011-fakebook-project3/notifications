#base image + dependencies
FROM mcr.microsoft.com/dotnet/sdk:5.0
#copy in published stuff (TODO: update for actual published location)
COPY publish/ /app
#set working directory
WORKDIR /app
#set entrypoint of file
CMD dotnet /app/FakebookNotifications.WebApi.dll 






# To Run:
# how do i use this Dockerfile?
# 1. dotnet publish -o publish
# 2. docker build -t FakebookNotification.WebApi .
# 3. docker run FakebookNotification.WebApi