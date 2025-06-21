# CancApp Backend

## Overview

CancApp Backend is built with ASP.NET Core Web API, following clean architecture principles to ensure modularity, scalability, and maintainability. The backend provides RESTful APIs for user management, authentication, content moderation, and business logic for the CancApp platform.

## Features
- User authentication and authorization (JWT-based)
- Role-based access control (Admin, Doctor, Patient, Pharmacist, etc.)
- Profile management and verification
- Content moderation (posts, comments, reactions)
- Doctor-patient record access requests and approval workflow
- Caching and performance optimizations
- Logging and error handling

## Requirements
- .NET 8.0 SDK or later
- SQL Server (or compatible database)
- (Optional) Redis or distributed cache for hybrid caching

## Setup
1. **Clone the repository:**
   ```bash
   git clone <repo-url>
   cd CancApp
   ```
2. **Configure the database connection:**
   - Update `appsettings.json` with your SQL Server connection string.
3. **Apply migrations:**
   ```bash
   dotnet ef database update
   ```
4. **Run the backend:**
   ```bash
   dotnet run
   ```
5. **API Documentation:**
   - Swagger UI is available at `/swagger` when running locally.

## API Endpoints
- **Authentication:** `/api/Auth/login`, `/api/Auth/register`, `/api/Auth/refresh`, etc.
- **User Management:** `/api/User/disable`, `/api/User/enable`, `/api/User/warning`, etc.
- **Profile Verification:** `/api/User/UnCompletedProfile`, `/api/User/ConfirmCompleteProfile`, `/api/User/FailCompleteProfile`
- **Content Moderation:** `/api/Post/reported`, `/api/Comments/reported`, `/api/Post/top`, etc.
- **Doctor-Patient Record Access:** `/api/Records/request-access`, `/api/Records/approve-access`, `/api/Records/reject-access`, `/api/Records/pending-requests/{patientId}`

## Logging & Error Handling
- All actions are logged using ASP.NET Core logging.
- Errors are returned in a consistent format with error codes and messages.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](LICENSE) 
