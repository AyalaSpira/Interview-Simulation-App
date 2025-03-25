FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 443
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["InterviewSimulationApp/InterviewSim.API/InterviewSim.API.csproj", "InterviewSimulationApp/InterviewSim.API/"]
COPY ["InterviewSimulationApp/InterviewSim.DAL/InterviewSim.DAL.csproj", "InterviewSimulationApp/InterviewSim.DAL/"]
COPY ["InterviewSimulationApp/InterviewSim.BLL/InterviewSim.BLL.csproj", "InterviewSimulationApp/InterviewSim.BLL/"]
COPY ["InterviewSimulationApp/InterviewSim.Shared/InterviewSim.Shared.csproj", "InterviewSimulationApp/InterviewSim.Shared/"]

RUN dotnet restore "InterviewSimulationApp/InterviewSim.API/InterviewSim.API.csproj"
COPY . .
WORKDIR "/src/InterviewSimulationApp/InterviewSim.API"
RUN dotnet build "InterviewSim.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "InterviewSim.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterviewSim.API.dll"]

