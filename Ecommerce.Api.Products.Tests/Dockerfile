#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Ecommerce.Api.Products.Tests/Ecommerce.Api.Products.Tests.csproj", "Ecommerce.Api.Products.Tests/"]
COPY ["Ecommerce.Api.Products/Ecommerce.Api.Products.csproj", "Ecommerce.Api.Products/"]
RUN dotnet restore "Ecommerce.Api.Products.Tests/Ecommerce.Api.Products.Tests.csproj"
COPY . .
WORKDIR "/src/Ecommerce.Api.Products.Tests"
RUN dotnet build "Ecommerce.Api.Products.Tests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Api.Products.Tests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Api.Products.Tests.dll"]