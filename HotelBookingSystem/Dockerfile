FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["HotelBookingSystem/HotelBookingSystem.API.csproj", "HotelBookingSystem/"]
COPY ["HotelBookingSystem.Core/HotelBookingSystemCore.csproj", "HotelBookingSystem.Core/"]

RUN dotnet restore "HotelBookingSystem/HotelBookingSystem.API.csproj"

COPY . .

WORKDIR "/src/HotelBookingSystem"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HotelBookingSystem.API.dll"]