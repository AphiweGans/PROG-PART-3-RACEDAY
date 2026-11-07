# RaceDay Part 3 — Video Presentation Script

**Before recording:** Start SQL Server, configure Azure Blob Storage, run the API and MVC app, and confirm all demonstrations work. Do not narrate unverified functionality as successful. Replace any failed step before recording.

## 1. Introduction
"Hello, this presentation demonstrates RaceDay Part 3, a race-event management application. It uses an ASP.NET Core MVC front end, a REST API, SQL Server, Azure Blob Storage for event images, and Docker for containerisation."

## 2. MVC user interface
- Show the RaceDay landing page and explain the visual theme and navigation.
- Open Explore Events and show event cards and event details, dates, locations, categories and entry fees.
- Register and log in as a Participant. Show the participant dashboard, enter an event, open My Events, My Results and Profile.
- Log out, then register and log in as an Organiser. Show the organiser dashboard, create/edit an event, create a category, inspect enrolments, update entry status and capture a result.

## 3. Azure Blob Storage
- Explain that the MVC upload form sends an image to the API, the API checks ownership and file type/size, and the API uploads it to the configured private Azure Blob container.
- Show the stored URL in the event record/API response and the event image displayed in MVC.
- Do not show the connection string or other credentials in the recording.

## 4. Docker
- Show `docker-compose.yml` and the API/MVC Dockerfiles.
- Run `docker compose up --build` and show the SQL Server, API and MVC services running.
- Open `http://localhost:8080` and `http://localhost:8081/swagger`.
- Explain that the API applies EF Core migrations on startup in the Compose environment.

## 5. Closing
"RaceDay brings event discovery, registration, organiser management and race results together in one application. Thank you for watching."

## Suggested duration
Aim for approximately 6–9 minutes. Keep the video focused on the features in the marking rubric and use genuine running software, not mockups.
