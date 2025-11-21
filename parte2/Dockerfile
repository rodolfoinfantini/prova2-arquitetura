FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Efc2/Efc2.csproj ./Efc2/
COPY Efc2.Tests/Efc2.Tests.csproj ./Efc2.Tests/
RUN dotnet restore Efc2/Efc2.csproj
RUN dotnet restore Efc2.Tests/Efc2.Tests.csproj

COPY . .
RUN dotnet test Efc2.Tests/Efc2.Tests.csproj --logger "console;verbosity=detailed"
RUN dotnet publish Efc2/Efc2.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Configure to listen on port 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "Efc2.dll"]
