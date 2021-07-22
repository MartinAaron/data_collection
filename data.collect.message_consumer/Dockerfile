FROM mcr.microsoft.com/dotnet/sdk:5.0

COPY /dist /app
WORKDIR /app
ENTRYPOINT ["dotnet", "data.collect.message_consumer.dll"]
