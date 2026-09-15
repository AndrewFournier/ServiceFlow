# ServiceFlow

ServiceFlow is an in-progress full-stack platform for small service businesses. The project is intended to bring service discovery, quote requests, scheduling, and day-to-day business management into one focused system for owners and their customers.

> **Project status:** ServiceFlow is under active development. The repository currently contains the backend foundation and an initial read-only services API. Customer workflows, business-management features, authentication, and the frontend described in the roadmap are not implemented yet.

## The problem

Small service businesses often manage customer inquiries, pricing, appointments, and job details across phone calls, text messages, spreadsheets, and separate calendar tools. That fragmentation makes it harder to respond consistently, avoid scheduling mistakes, and maintain a clear view of the business.

ServiceFlow aims to provide a single platform where customers can understand available services and request work while business owners can manage the operational workflow behind those requests.

## Current features

- ASP.NET Core Minimal API backed by PostgreSQL.
- Read-only `GET /api/services` endpoint for active service offerings.
- Entity Framework Core model configuration and migrations.
- Fixed seed data for Lawn Care, Pressure Washing, and Gutter Cleaning.
- Docker Compose configuration for a local PostgreSQL database with a health check and persistent volume.
- OpenAPI document generation in the Development environment.
- xUnit integration test using `WebApplicationFactory` and a uniquely named EF Core InMemory database, independent of the local PostgreSQL container.

## Technology stack

- C# and .NET 10
- ASP.NET Core Minimal APIs
- Entity Framework Core 10.0.12
- Npgsql Entity Framework Core provider 10.0.3
- PostgreSQL 18 (Alpine image)
- Docker Compose
- OpenAPI
- xUnit and ASP.NET Core `WebApplicationFactory`
- EF Core InMemory 10.0.12 for integration tests

## Repository structure

```text
ServiceFlow/
├── docker-compose.yml                 # Local PostgreSQL service
├── dotnet-tools.json                  # Local dotnet-ef tool manifest
├── ServiceFlow.slnx                   # Solution definition
├── src/
│   └── ServiceFlow.Api/
│       ├── Data/
│       │   ├── Migrations/            # EF Core migrations and model snapshot
│       │   └── ServiceFlowDbContext.cs
│       ├── Domain/
│       │   └── ServiceOffering.cs
│       ├── Properties/
│       │   └── launchSettings.json
│       ├── Program.cs                 # Application setup and HTTP endpoint
│       └── appsettings.Development.json
└── tests/
    └── ServiceFlow.Api.Tests/
        └── ServicesEndpointTests.cs   # Isolated API integration test
```

## Local setup

### Prerequisites

Install:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://docs.docker.com/get-docker/) with Docker Compose

Run all commands below from the repository root.

### 1. Restore dependencies and tools

```bash
dotnet restore
dotnet tool restore
```

The local tool manifest pins `dotnet-ef` to version 10.0.12.

### 2. Start PostgreSQL

```bash
docker compose up -d database
docker compose ps
```

Wait until `serviceflow-database` is reported as healthy. The development database is exposed on `localhost:5432` with the values configured in `docker-compose.yml` and `src/ServiceFlow.Api/appsettings.Development.json`:

```text
Database: serviceflow
Username: serviceflow
Password: serviceflow_dev
```

These credentials are for local development only.

### 3. Apply migrations

```bash
dotnet tool run dotnet-ef database update \
  --project src/ServiceFlow.Api \
  --startup-project src/ServiceFlow.Api
```

This creates the `Services` table and inserts the three initial service offerings through the existing migrations.

When the EF Core model changes, create a migration with:

```bash
dotnet tool run dotnet-ef migrations add <MigrationName> \
  --project src/ServiceFlow.Api \
  --startup-project src/ServiceFlow.Api \
  --output-dir Data/Migrations
```

Review generated migrations before applying them, then run the database-update command above.

### 4. Run the API

```bash
dotnet run --project src/ServiceFlow.Api --launch-profile http
```

The API listens on `http://localhost:5003`. In Development, its generated OpenAPI document is available at `http://localhost:5003/openapi/v1.json`.

### 5. Call the services endpoint

```bash
curl http://localhost:5003/api/services
```

### 6. Run automated tests

```bash
dotnet test
```

The services integration test replaces the production PostgreSQL registration with a uniquely named in-memory database and initializes the EF Core seed data. Tests therefore do not require the Docker database to be running and do not read or modify developer data.

### 7. Stop local infrastructure

```bash
docker compose down
```

The PostgreSQL data remains in the named Docker volume for the next run.

## API

### `GET /api/services`

Returns all active service offerings from PostgreSQL. The query is asynchronous and read-only; inactive records are excluded.

**Request**

```http
GET /api/services HTTP/1.1
Host: localhost:5003
Accept: application/json
```

**Successful response:** `200 OK`

```json
[
  {
    "id": "11111111-1111-1111-1111-111111111111",
    "name": "Lawn Care",
    "description": "Mowing, edging, and general yard cleanup.",
    "startingPrice": 75.00,
    "estimatedMinutes": 90,
    "isActive": true
  },
  {
    "id": "22222222-2222-2222-2222-222222222222",
    "name": "Pressure Washing",
    "description": "Exterior cleaning for driveways, patios, and siding.",
    "startingPrice": 150.00,
    "estimatedMinutes": 120,
    "isActive": true
  },
  {
    "id": "33333333-3333-3333-3333-333333333333",
    "name": "Gutter Cleaning",
    "description": "Removal of leaves and debris from gutters and downspouts.",
    "startingPrice": 125.00,
    "estimatedMinutes": 90,
    "isActive": true
  }
]
```

The endpoint does not currently define a guaranteed result order.

## Planned features

The following items are a roadmap, not current functionality:

- Customer quote-request workflow.
- Appointment availability and scheduling.
- Owner dashboard for managing services, requests, and upcoming work.
- Authentication and role-aware access for customers and business owners.
- React and TypeScript customer and owner frontend.
- Additional APIs and persistence models to support customer, quote, and scheduling workflows.

## Technical highlights

- Uses dependency injection to keep the API coupled to the EF Core context abstraction rather than database setup details.
- Performs asynchronous, no-tracking reads for the services catalog.
- Keeps domain entities, persistence configuration, migrations, and HTTP composition in distinct areas.
- Uses migrations for reproducible schema changes and deterministic seed identifiers.
- Exercises the real HTTP pipeline through `WebApplicationFactory` while swapping PostgreSQL for an isolated test provider.
- Maintains test independence from local infrastructure and developer data.

## Development notes

Before opening a change, run:

```bash
dotnet build
dotnet test
```

As the project expands, documentation will be updated to distinguish shipped behavior from planned capabilities.
