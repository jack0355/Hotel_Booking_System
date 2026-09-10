# Hotel Booking System — Full Stack

A full-stack hotel room booking platform built with ASP.NET Core REST API, Blazor Server frontend, PostgreSQL, and cloud deployment on Railway.

**Live demo:** [Hotel Booking System](https://attractive-inspiration-production-2acc.up.railway.app)

## Architecture

```
[ Blazor Server Frontend ]  →  deployed on Railway
         |
         | HTTP + JWT Bearer Token
         |
[ ASP.NET Core REST API ]   →  deployed on Railway
         |
         | Entity Framework Core (Npgsql)
         |
     [ PostgreSQL ]         →  Railway-managed Postgres
         |
[ Cloudinary ]              →  room image hosting
```

Also fully runnable locally via Docker Compose (API + PostgreSQL, with an optional SQL Server service retained for reference).

## Features

### Backend (ASP.NET Core API)
- JWT authentication with access token + refresh token rotation
- Role-based authorization — Admin and Guest roles
- Room management — CRUD with soft delete
- **Room image uploads** — multipart file upload, stored via Cloudinary, URL persisted per room
- Booking system with double-booking prevention (date-overlap logic at the query level)
- Background service — auto-cancels pending bookings after 15 minutes using `IServiceScopeFactory`
- Check-in / check-out timestamp tracking, separate from booking status
- Guest review system, gated on verified checkout
- Admin dashboard endpoints — filterable bookings, review board
- Room availability search by date range
- EF Core Code First — migrated from SQL Server to **PostgreSQL** mid-project
- Dockerized with health checks and auto-migration on startup

### Frontend (Blazor Server + MudBlazor)
- Modern Material Design UI with MudBlazor
- Login and signup with JWT auth
- Room browsing with card layout **and room photos**
- Collapsible, responsive navigation drawer (mobile-friendly)
- Date picker booking flow with validation
- My Bookings page with status colors, cancel button, and review submission
- Role-aware navigation (Admin/Guest)
- Admin dashboard: room management with photo upload, all-bookings view, review moderation

### Testing
- xUnit unit tests covering booking price calculation and business logic
- Integration tests using an in-memory database, simulating authenticated requests

## Deployment

The API and Blazor frontend are deployed as two independent services on **Railway**, each built directly from its own Dockerfile in this repository. PostgreSQL runs as a managed Railway service; the API auto-applies EF Core migrations on startup, so schema changes ship with each deploy.

Room images are uploaded through the API, forwarded to **Cloudinary**, and only the resulting URL is persisted — keeping the deployed containers stateless.

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8 Web API |
| ORM | Entity Framework Core (Npgsql) |
| Database | PostgreSQL |
| Auth | JWT + Refresh Token Rotation |
| Frontend | Blazor Server |
| UI Library | MudBlazor |
| Image Hosting | Cloudinary |
| Containerization | Docker + Docker Compose |
| Hosting | Railway |
| Testing | xUnit |
| Language | C# |

## Project Structure

```
HotelBookingSystem/
│
├── HotelBookingSystem/          # ASP.NET Core Web API
│   ├── Controllers/             # Auth, Room, Bookings
│   ├── Data/                    # AppDbContext
│   ├── DTOs/                    # Request/Response shapes
│   ├── Services/                # BookingService, BookingExpiryService
│   ├── Migrations/              # EF Core migrations (PostgreSQL)
│   └── Dockerfile
│
├── HotelBookingSystem.Core/     # Shared entities
│   └── Entities/                # Room, Guest, Booking, User, Review
│
├── HotelBookingSystem.Blazor/   # Blazor Server frontend
│   ├── Components/Pages/        # Login, Rooms, Book, MyBookings, Admin
│   ├── Components/Layout/       # MainLayout with MudBlazor
│   ├── Services/                # TokenStore (JWT client-side)
│   └── Dockerfile
│
├── HotelBookingSystem.Tests/    # xUnit unit + integration tests
│
└── docker-compose.yml           # API + PostgreSQL (+ SQL Server, retained)
```

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
API starts on `http://localhost:8080` — Swagger available in Development mode at `/swagger`.

### Run Blazor Frontend
```bash
cd HotelBookingSystem.Blazor
dotnet run
```

## Security Notes
- Refresh tokens rotated on every use — stolen tokens are invalidated
- Passwords hashed with **BCrypt** (migrated from an earlier SHA-256 implementation)
- All protected endpoints require a valid JWT Bearer token
- Role checks prevent guests from accessing admin endpoints
- Double-booking prevented at the database query level
- Cloudinary credentials and database connection strings stored as environment variables, never committed to source

## Author

Zain Ramadan — Junior .NET Backend Developer
[github.com/jack0355](https://github.com/jack0355)
