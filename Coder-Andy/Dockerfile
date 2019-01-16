FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Coder-Andy/Coder-Andy.csproj", "Coder-Andy/"]
RUN dotnet restore "Coder-Andy/Coder-Andy.csproj"
COPY . .
WORKDIR "/src/Coder-Andy"
RUN dotnet build "Coder-Andy.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Coder-Andy.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Coder-Andy.dll"]