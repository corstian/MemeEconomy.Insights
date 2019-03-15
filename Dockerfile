FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["MemeEconomy.Insights/MemeEconomy.Insights.csproj", "MemeEconomy.Insights/"]
RUN dotnet restore "MemeEconomy.Insights/MemeEconomy.Insights.csproj"
COPY . .
WORKDIR "/src/MemeEconomy.Insights"
RUN dotnet build "MemeEconomy.Insights.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MemeEconomy.Insights.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MemeEconomy.Insights.dll"]