FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "AWSSQSDotnet.csproj" --disable-parallel
RUN dotnet publish "AWSSQSDotnet.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:5000 

ENV QUEUE_ACCESS_KEY=
ENV QUEUE_SECRET_KEY=
ENV QUEUE_NAME=


ENTRYPOINT ["dotnet", "AWSSQSDotnet.dll"]

# docker run -p 5000:5000 --name aws-sqs-dotnet-api aws-sqs-dotnet