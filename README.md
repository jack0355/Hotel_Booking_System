# Hotel Booking System — Full Stack

A full-stack hotel room booking platform built with **ASP.NET Core REST API**, **Blazor Server** frontend, and **Docker** containerization.

---

## Architecture

```
[ Blazor Server Frontend ]
         |
         | HTTP + JWT Bearer Token
         |
[ ASP.NET Core REST API ]
         |
         | Entity Framework Core
         |
     [ SQL Server ]
         |
  [ Docker Compose ]
```

---

## Features

### Backend (ASP.NET Core API)
- JWT authentication with **access token + refresh token rotation**
- Role-based authorization — Admin and Guest roles
- Room management — CRUD with soft delete
- Booking system with **double-booking prevention** (date overlap logic)
- **Background service** — auto-cancels pending bookings after 15 minutes using `IServiceScopeFactory`
- Room availability search by date range
- EF Core Code First with SQL Server
- Dockerized with health checks and auto-migration on startup

### Frontend (Blazor Server + MudBlazor)
- Modern Material Design UI with MudBlazor
- Login page with JWT auth
- Room browsing with card layout
- Date picker booking flow with validation
- My Bookings page with status colors and cancel button
- Role-aware navigation (Admin/Guest)

---

## CI/CD Pipeline

Automated build and containerization pipeline using **GitHub Actions**:

- Triggers automatically on every push to `master`
- Restores and builds the full .NET solution (API, Core, Blazor)
- Builds a Docker image from the API's Dockerfile
- Authenticates and pushes the built image to **Docker Hub** using GitHub Actions secrets

```yaml
name: CI

on: [push]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0'
      - run: dotnet build HotelBookinSystem.sln
      - run: docker build -t ${{ secrets.DOCKER_USERNAME }}/hotelbookingsystem -f HotelBookingSystem/Dockerfile .
      - run: docker login -u ${{ secrets.DOCKER_USERNAME }} -p ${{ secrets.DOCKER_PASSWORD }}
      - run: docker push ${{ secrets.DOCKER_USERNAME }}/hotelbookingsystem
```

Credentials are stored securely as **GitHub Actions repository secrets** — never hardcoded in the workflow file.

See the [Actions tab](https://github.com/jack0355/Hotel_Booking_System/actions) for full build history, including real debugging along the way (build path errors, Docker tag formatting, and authentication fixes).

---

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Auth | JWT + Refresh Token Rotation |
| Frontend | Blazor Server |
| UI Library | MudBlazor |
| Containerization | Docker + Docker Compose |
| CI/CD | GitHub Actions → Docker Hub |
| Language | C# |

---

## Project Structure

```
HotelBookingSystem/
│
├── .github/workflows/           # CI/CD pipeline (GitHub Actions)
│   └── ci.yml
│
├── HotelBookingSystem/          # ASP.NET Core Web API
│   ├── Controllers/             # Auth, Rooms, Bookings
│   ├── Data/                    # AppDbContext
│   ├── DTOs/                    # Request/Response shapes
│   ├── Services/                # BookingExpiryService (background)
│   ├── Migrations/              # EF Core migrations
│   └── Dockerfile
│
├── HotelBookingSystem.Core/     # Shared entities
│   └── Entities/                # Room, Guest, Booking, User
│
├── HotelBookingSystem.Blazor/   # Blazor Server frontend
│   ├── Components/Pages/        # Login, Rooms, Book, MyBookings
│   ├── Components/Layout/       # MainLayout with MudBlazor
│   └── Services/                # TokenStore (JWT client-side)
│
└── docker-compose.yml           # API + SQL Server containers
```

---

## Getting Started

### Prerequisites
- Docker Desktop
- .NET 8 SDK (for local development)

### Run with Docker
```bash
git clone https://github.com/jack0355/Hotel_Booking_System.git
cd Hotel_Booking_System
docker-compose up --build
```
API starts on `http://localhost:8080` — Swagger available at `http://localhost:8080/swagger`

### Run Blazor Frontend
```bash
cd HotelBookingSystem.Blazor
dotnet run
```
Open `http://localhost:5295`

---

## Security Notes
- Refresh tokens rotated on every use — stolen tokens are invalidated
- Passwords hashed with SHA-256 before storage
- All protected endpoints require valid JWT Bearer token
- Role checks prevent guests from accessing admin endpoints
- Double-booking prevented at the database query level
- CI/CD credentials stored as encrypted GitHub Actions secrets, never committed to source

---

## Author

**Zain Ramadan** — Junior .NET Backend Developer
[github.com/jack0355](https://github.com/jack0355)
