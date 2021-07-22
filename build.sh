#!/bin/bash -xe 

dotnet restore ./data.collection.api && dotnet publish  ./data.collection.api --framework net5.0 --configuration Release --output ./data.collection.api/dist
docker rm data_collection_api -f
docker rmi data_collection_api_img
docker build -t data_collection_api_img --file ./data.collection.api/Dockerfile .
docker run -d -v /app/wwwroot/:/home/docker/professor -p 8708:5000  -it --name=data_collection_api  data_collection_api_img