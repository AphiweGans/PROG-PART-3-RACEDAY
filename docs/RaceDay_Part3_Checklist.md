# RaceDay Part 3 — Official Rubric Checklist

Use this checklist against the rubric supplied for PROG6212 Part 3 (100 marks). Items requiring runtime evidence must only be marked complete after actual verification.

## 1. Correct Submission — 5 marks
- [x] Solution has a clear folder structure and README setup instructions.
- [x] README includes Docker Compose run commands and describes major features.
- [ ] Add the genuine successful CI screenshot and workflow URL after a green run.
- [ ] Add the actual video presentation link after recording and uploading.

## 2. GitHub and CI/CD — 10 marks
- [x] GitHub Actions workflow restores, builds, tests, checks project structure and builds both Docker images.
- [ ] Push the final project to the intended GitHub repository.
- [ ] Build succeeds and tests pass on GitHub Actions.
- [ ] Maintain at least 20 meaningful commits that reflect real development work. Do not fabricate commit history.

## 3. Authentication and Role-Based Navigation — 10 marks
- [x] MVC registration and login forms support both Organiser and Participant roles through the Part 2 API.
- [x] MVC forwards the API session cookie server-to-server and stores the logged-in role/name in its own session.
- [x] Navigation menus differ by role and logout calls the API then clears MVC session.
- [ ] Verify login, role redirects, protected pages, API authorization, logout and unauthenticated redirects in a browser.

## 4. Organiser Interface — 15 marks
- [x] Organiser dashboard lists owned events.
- [x] Create, edit and delete event screens.
- [x] Category creation/listing/deletion screens.
- [x] Event enrolment list and status management.
- [x] Result capture and results listing.
- [x] Event image upload form.
- [ ] Test each organiser action against a running API and database, including ownership denial for another organiser.

## 5. Participant Interface — 15 marks
- [x] Participant dashboard and upcoming event cards.
- [x] Event details and category entry form.
- [x] My Events and cancellation.
- [x] My Results with finish time and position fields.
- [x] Profile page and update form.
- [ ] Verify all pages with a real Participant account and confirm participants cannot access organiser-only actions.

## 6. Azure Blob Storage — 10 marks
- [x] API image upload endpoint accepts JPG, PNG and WebP files with an 8 MB limit.
- [x] Upload endpoint checks that the Organiser owns the event.
- [x] Blob URL/SAS URL is stored in `Events.ImageUrl` and is included in event API responses for MVC display.
- [x] EF migration adds `Events.ImageUrl`.
- [ ] Set Azure connection string through environment variables or user secrets (never commit credentials).
- [ ] Test a real Azure upload and confirm the image displays in the MVC event list/details.

## 7. Docker — 10 marks
- [x] API and MVC Dockerfiles included.
- [x] Compose includes SQL Server, API and MVC, and enables migrations at API startup.
- [x] README includes the compose command and service URLs.
- [ ] Run `docker compose up --build` on a machine with Docker installed and confirm all services start cleanly.
- [ ] Capture genuine evidence of the working containerised application.

## 8. UI Design — 10 marks
- [x] Consistent RaceDay colour palette, responsive layout, role-appropriate navigation, status chips, cards, forms and empty states.
- [x] Event image display is wired to `ImageUrl`.
- [ ] Inspect on desktop/mobile and adjust any layout issues found during actual use.

## 9. Video Presentation — 15 marks
- [x] Presentation outline supplied in `RaceDay_Part3_Video_Script.md`.
- [ ] Record a clear demonstration of the complete MVC app, both roles, event creation/entry/results, Azure upload and Docker setup.
- [ ] Upload the video and add the real link to README/submission.

## Honest verification status
The project source has been assembled, but build/test, SQL Server, Docker and real Azure Storage operations must be verified on a machine with the .NET 8 SDK, Docker and valid Azure credentials. CI screenshot, commit history and video evidence cannot be manufactured by source code.
