# RentalApp - Peer-to-Peer Rental Marketplace

SET09102 Coursework - Hamzah Imtiaz (40651904)

## Overview
A peer-to-peer rental marketplace built with .NET MAUI where users can list items, request rentals, approve/reject requests and leave reviews.

## Setup
1. Install Docker Desktop
2. Run `docker compose up` in the project root
3. Open in VS Code Dev Container
4. Run `dotnet build`
5. Deploy to Android emulator with `adb install`

## Running Tests
```bash
dotnet test StarterApp.Test/StarterApp.Test.csproj
```

## Architecture
4 layer architecture: Views → ViewModels → Services → Repositories → PostgreSQL

## GitHub Actions
CI/CD pipeline runs on every push to main. Builds and runs all 10 tests automatically.
