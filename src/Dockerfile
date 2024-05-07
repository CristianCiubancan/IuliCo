# Use the official Microsoft .NET SDK image for building the projects.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# Restore and build IuliCo.Core
COPY ["IuliCo.Core/IuliCo.Core.csproj", "IuliCo.Core/"]
RUN dotnet restore "IuliCo.Core/IuliCo.Core.csproj"
COPY ["IuliCo.Core/", "IuliCo.Core/"]
RUN dotnet build "IuliCo.Core/IuliCo.Core.csproj" -c Release -o /app/build

# Restore and build IuliCo.Game
COPY ["IuliCo.Game/IuliCo.Game.csproj", "IuliCo.Game/"]
RUN dotnet restore "IuliCo.Game/IuliCo.Game.csproj"
COPY ["IuliCo.Game/", "IuliCo.Game/"]
RUN dotnet build "IuliCo.Game/IuliCo.Game.csproj" -c Release -o /app/build
RUN dotnet publish "IuliCo.Game/IuliCo.Game.csproj" -c Release -o /app/publish/game

# Restore and build IuliCo.Account
COPY ["IuliCo.Account/IuliCo.Account.csproj", "IuliCo.Account/"]
RUN dotnet restore "IuliCo.Account/IuliCo.Account.csproj"
COPY ["IuliCo.Account/", "IuliCo.Account/"]
RUN dotnet build "IuliCo.Account/IuliCo.Account.csproj" -c Release -o /app/build
RUN dotnet publish "IuliCo.Account/IuliCo.Account.csproj" -c Release -o /app/publish/account

# Final stage/game
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS game-final
WORKDIR /app
COPY --from=build /app/publish/game .
ENTRYPOINT ["dotnet", "IuliCo.Game.dll"]

# Final stage/account
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS account-final
WORKDIR /app
COPY --from=build /app/publish/account .
ENTRYPOINT ["dotnet", "IuliCo.Account.dll"]
