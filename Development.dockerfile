FROM mcr.microsoft.com/dotnet/core/sdk:3.0
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /app
COPY . .
EXPOSE 5000
ENTRYPOINT ["/bin/bash", "-c", "dotnet restore && dotnet run"]
