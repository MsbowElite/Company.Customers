#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Company.Customers.API/Company.Customers.API.csproj", "Company.Customers.API/"]
COPY ["Company.Customers.Infra.IoC/Company.Customers.Infra.CrossCutting.IoC.csproj", "Company.Customers.Infra.IoC/"]
COPY ["Company.Customers.Infra.Data/Company.Customers.Infra.Data.csproj", "Company.Customers.Infra.Data/"]
COPY ["Company.Customers.Domain/Company.Customers.Domain.csproj", "Company.Customers.Domain/"]
COPY ["Company.Customers.Infra.CrossCutting.Utils/Company.Customers.Infra.CrossCutting.Utils.csproj", "Company.Customers.Infra.CrossCutting.Utils/"]
COPY ["Company.Customers.Application/Company.Customers.Application.csproj", "Company.Customers.Application/"]
RUN dotnet restore "Company.Customers.API/Company.Customers.API.csproj"
COPY . .
WORKDIR "/src/Company.Customers.API"
RUN dotnet build "Company.Customers.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Company.Customers.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Company.Customers.API.dll"]
