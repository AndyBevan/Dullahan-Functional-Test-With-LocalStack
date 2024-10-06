# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the .NET 8 runtime image for the base layer
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET 8 SDK image for the build layer
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Dullahan.LocalStack.Sample.Api/Dullahan.LocalStack.Sample.Api.csproj", "Dullahan.LocalStack.Sample.Api/"]
RUN dotnet restore "Dullahan.LocalStack.Sample.Api/Dullahan.LocalStack.Sample.Api.csproj"
COPY . .
WORKDIR "/src/Dullahan.LocalStack.Sample.Api"
RUN dotnet build "Dullahan.LocalStack.Sample.Api.csproj" -c Release -o /app/build

# Publish the app to a separate directory
FROM build AS publish
RUN dotnet publish "Dullahan.LocalStack.Sample.Api.csproj" -c Release -o /app/publish

# Use the base image for the final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dullahan.LocalStack.Sample.Api.dll"]

