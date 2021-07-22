FROM mcr.microsoft.com/dotnet/sdk:5.0

COPY dist /app
WORKDIR /app
EXPOSE 5002
ENTRYPOINT ["dotnet","data.collection.identity.dll"] 