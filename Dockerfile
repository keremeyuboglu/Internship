#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Altamira.csproj", "."]
RUN dotnet restore "Altamira.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Altamira.csproj" -c Release -o /app/build
#RUN dotnet tool install --global dotnet-ef
#ENV PATH="${PATH}:/root/.dotnet/tools"
#RUN dotnet ef database update # Database docker image should've been started up already, with this command I am 

FROM build AS publish
RUN dotnet publish "Altamira.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Docker
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Altamira.dll"]

COPY sql/wait-for-it.sh /usr/wait-for-it.sh
RUN chmod +x /usr/wait-for-it.sh
