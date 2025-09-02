
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY . .

RUN dotnet restore ./Agenda.Api/Agenda.Api.csproj
RUN dotnet build ./Agenda.Api/Agenda.Api.csproj -c Release -o dist/build
RUN dotnet publish ./Agenda.Api/Agenda.Api.csproj -c Release -o dist/publish

WORKDIR /app/dist/publish

EXPOSE 8080

ENTRYPOINT ["dotnet", "Agenda.Api.dll"]