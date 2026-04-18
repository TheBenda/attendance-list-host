# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Architecture & Project Structure
This repository (`attendancy-list-host`) is the central orchestrator for the Attendancy List application, using **.NET Aspire**. It coordinates running the backend, frontend, and infrastructure.

The solution references external projects via relative paths (`../attendance-list-backend` and `../attendance-list-frontend`).

* **`aspire/ALB.AppHost`**: The central Aspire AppHost. Defines the topology: PostgreSQL database -> Migration Service -> Backend API -> Vite Frontend.
* **`aspire/ALB.MigrationService`**: Responsible for creating/migrating the PostgreSQL database using EF Core (`ApplicationDbContext`).
* **`aspire/ALB.HostServiceDefaults`**: Standard .NET Aspire service defaults (OpenTelemetry tracing, metrics, health checks).
* **`EndToEndTests`**: UI and Integration tests using `TUnit` and `TUnit.Playwright` alongside `Aspire.Hosting.Testing` to spin up the entire application stack in memory.

## Key Technologies
* **.NET 10** with the new `.slnx` solution format.
* **.NET Aspire** for local orchestration and service discovery.
* **TUnit** (not xUnit/NUnit) for testing.
* **Playwright** for end-to-end tests.
* **NodaTime** for handling dates/times in PostgreSQL.

## Common Development Commands
* **Build Solution**: `dotnet build attendancy-list-host.slnx`
* **Run Application locally (spins up frontend, backend, DB, etc.)**: 
  `dotnet run --project aspire/ALB.AppHost`
* **Run All Tests**: `dotnet test`
* **Run Specific Test**: `dotnet test --filter "EndToEndTests.Tests.FrontUriNotNull"`

## Development Guidelines
* When adding new services to the stack, always update the topology in `aspire/ALB.AppHost/Program.cs` and ensure the proper `.WaitFor()` calls are made so services start in the correct order.
* Use `TUnit` features for tests (e.g., `[ClassDataSource]`, `[Test]`) rather than xUnit attributes.
* Be aware that changing the AppHost might require corresponding changes in the `attendance-list-backend` or `attendance-list-frontend` repositories if environment variables or connection string names are modified.
