FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /src

COPY *.sln ./
COPY ContactBook.Api/ContactBook.Api.csproj ContactBook.Api/
COPY ContactBook.CrossCutting.Exceptions/ContactBook.CrossCutting.Exceptions.csproj ContactBook.CrossCutting.Exceptions/
COPY ContactBook.Domain/ContactBook.Domain.csproj ContactBook.Domain/
COPY ContactBook.Infrastructure.Data.Query/ContactBook.Infrastructure.Data.Query.csproj ContactBook.Infrastructure.Data.Query/
COPY ContactBook.Infrastructure.Data.Service/ContactBook.Infrastructure.Data.Service.csproj ContactBook.Infrastructure.Data.Service/
RUN dotnet restore

COPY . .
RUN dotnet publish --output /app/ --configuration Release

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=builder /app .
CMD ["dotnet", "ContactBook.Api.dll"]
