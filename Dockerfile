#base image + dependencies
FROM mcr.microsoft.com/dotnet/sdk:5.0
#copy in published stuff (TODO: update for actual published location)
COPY bin/Release/net5.0/publish/ App/
#set working directory
WORKDIR /App
#set entrypoint of file
ENTRYPOINT ["dotnet", "Fakebook-Notifications.WebAPI.dll"]






# To Run:
# 2. docker build -t Fakebook-Notifications.WebAPI  .
# 3. docker run