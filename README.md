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
| Language | C# |

---

## Project Structure

```
HotelBookingSystem/
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

---

## Author

**Zain Ramadan** — Junior .NET Backend Developer  
[github.com/jack0355](https://github.com/jack0355)
