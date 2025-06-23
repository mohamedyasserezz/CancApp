# CancApp - Healthcare Community Platform

## 🏥 Overview

CancApp is a comprehensive healthcare community platform built with ASP.NET Core 9.0, following clean architecture principles. The platform connects patients, doctors, pharmacists, and volunteers in a secure, moderated environment for healthcare discussions, record management, and community support.

## ✨ Key Features

### 🔐 Authentication & Authorization
- **JWT-based authentication** with refresh tokens
- **Role-based access control** (Admin, Doctor, Patient, Pharmacist, Psychiatrist, Volunteer)
- **Email confirmation** and password reset workflows
- **Profile verification** system for healthcare professionals

### 👥 User Management
- **Multi-role user system** with specific permissions
- **Profile management** with image uploads
- **User moderation** (warnings, disable/enable)
- **Professional verification** (medical licenses, pharmacy permits)

### 💬 Community Features
- **Posts and comments** with rich media support
- **Reactions system** (likes, reactions)
- **Content moderation** with reporting system
- **Real-time updates** via SignalR

### 🏥 Healthcare Records
- **Doctor-patient record access** requests
- **Approval workflow** for record sharing
- **Secure record management** with audit trails
- **Patient consent** management

### 📊 Admin Dashboard
- **User analytics** and statistics
- **Content moderation** tools
- **Profile verification** management
- **System monitoring** and health checks

### 🚀 Technical Features
- **Hybrid caching** for performance optimization
- **Background job processing** with Hangfire
- **Real-time updates** via SignalR (community, posts, comments)
- **Push notifications** via FCM (Firebase Cloud Messaging)
- **Comprehensive logging** with Serilog
- **Health monitoring** with detailed health checks
- **File management** with secure uploads

## 🛠️ Technology Stack

### Core Framework
- **ASP.NET Core 9.0** - Modern web framework
- **Entity Framework Core** - ORM with SQL Server
- **ASP.NET Core Identity** - Authentication and user management

### Architecture
- **Clean Architecture** - Layered, modular, and testable
- **Domain-Driven Design (inspired)** - Clear separation of domain, application, infrastructure, and API layers. Not a full DDD implementation (no aggregates, domain events, etc.)
- **Repository & Unit of Work Patterns** - Data access abstraction and transaction management

### Testing
- **xUnit** - Unit testing framework
- **FakeItEasy** - Mocking library
- **FluentAssertions** - Readable assertions
- **MockQueryable.FakeItEasy** - EF Core query mocking

### Infrastructure
- **Docker** - Containerization
- **SQL Server** - Primary database
- **Hangfire** - Background job processing
- **SignalR** - Real-time communication
- **Serilog** - Structured logging

## 📋 Requirements

### Development
- **.NET 9.0 SDK** or later
- **SQL Server 2019** or later
- **Docker Desktop** (optional, for containerized development)

### Production
- **.NET 9.0 Runtime**
- **SQL Server 2019+** or Azure SQL Database
- **Docker** (recommended for deployment)

## 🚀 Quick Start

### 1. Clone the Repository
```bash
git clone <repository-url>
cd CancApp
```

### 2. Development Setup

#### Option A: Local Development
```bash
# Restore dependencies
dotnet restore

# Update connection string in appsettings.json
# Apply database migrations
dotnet ef database update --project CanaApp.Persistance --startup-project CanaApp.Apis

# Run the application
dotnet run --project CanaApp.Apis
```

#### Option B: Docker Development
```bash
# Copy environment file
cp env.example .env

# Edit .env with your configuration
# Start development environment
docker-compose up --build
```

### 3. Access the Application
- **API Base URL**: `https://localhost:7001` (or configured port)
- **Swagger Documentation**: `https://localhost:7001/swagger`
- **Health Check**: `https://localhost:7001/health`
- **Hangfire Dashboard**: `https://localhost:7001/jobs`

## 🧪 Testing

### Run All Tests
```bash
dotnet test CanaApp.Application.Tests
```


## 🐳 Docker Deployment

### Development Environment
```bash
# Start all services
docker-compose up

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Production Environment
```bash
# Start production environment
docker-compose -f docker-compose.prod.yml up -d

# Monitor production
docker-compose -f docker-compose.prod.yml logs -f
```

### Docker Features
- **Multi-stage builds** for optimized images
- **Health checks** for all services
- **Resource limits** and monitoring
- **Log rotation** and management
- **Environment-specific** configurations

## 📊 Health Monitoring

### Health Check Endpoints
- **`/health`** - Complete health status with all checks


### Monitored Services
- ✅ **Database connectivity** - Entity Framework health check
- ✅ **Background jobs** - Hangfire service health
- ✅ **Email service** - SMTP connectivity check
- ✅ **Application health** - Basic self-check

## 🔧 Configuration

### Environment Variables
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=CancApp;...",
    "HangfireConnection": "Server=...;Database=CancApp_Hangfire;..."
  },
  "JwtOptions": {
    "Key": "your-secret-key",
    "Issuer": "CancApp",
    "Audience": "CancApp",
    "ExpireTimeInMinutes": 60
  },
  "MailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Mail": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### Email Templates
- Email templates are located in `CanaApp.Apis/Templates/`
- Supports HTML templates with variable substitution
- Background job processing for email sending

## 📚 API Documentation

### Authentication Endpoints
- `POST /api/Auth/login` - User authentication
- `POST /api/Auth/register` - User registration
- `POST /api/Auth/refresh` - Token refresh
- `POST /api/Auth/confirm-email` - Email confirmation
- `POST /api/Auth/reset-password` - Password reset

### User Management
- `GET /api/User/profile` - Get user profile
- `PUT /api/User/profile` - Update user profile
- `POST /api/User/disable/{userId}` - Disable user
- `POST /api/User/enable/{userId}` - Enable user
- `POST /api/User/warning/{userId}` - Add warning

### Community Features
- `GET /api/Post` - Get posts with pagination
- `POST /api/Post` - Create post
- `PUT /api/Post` - Update post
- `DELETE /api/Post/{id}` - Delete post
- `POST /api/Post/{id}/report` - Report post
- `GET /api/Comments` - Get comments
- `POST /api/Comments` - Create comment
- `POST /api/Reactions` - Add reaction

### Healthcare Records
- `POST /api/Records/request-access` - Request record access
- `POST /api/Records/approve-access` - Approve access request
- `GET /api/Records/pending-requests` - Get pending requests
- `GET /api/Records/accepted-patients` - Get accepted patients

### Admin Dashboard
- `POST /api/Dashboard/login` - Admin authentication
- `GET /api/Dashboard/users-chart` - User statistics
- `GET /api/Dashboard/uncompleted-profiles` - Pending verifications
- `POST /api/Dashboard/confirm-profile/{userId}` - Approve profile
- `POST /api/Dashboard/fail-profile/{userId}` - Reject profile
- `GET /api/Dashboard/reported-posts` - Reported content
- `GET /api/Dashboard/reported-comments` - Reported comments

## 🔒 Security Features

### Authentication & Authorization
- **JWT tokens** with configurable expiration
- **Refresh token rotation** for security
- **Role-based permissions** with fine-grained control
- **Email verification** for account activation

### Data Protection
- **Password hashing** with ASP.NET Core Identity
- **HTTPS enforcement** in production
- **Input validation** with FluentValidation
- **SQL injection protection** with parameterized queries

### Content Security
- **File upload validation** and virus scanning
- **Content moderation** with reporting system
- **User reputation** and warning system
- **Audit logging** for sensitive operations

## 📈 Performance & Scalability

### Caching Strategy
- **Hybrid caching** with in-memory storage
- **Cache invalidation** patterns for data consistency
- **Background job processing** for heavy operations
- **Database connection pooling** for optimal performance

### Monitoring & Observability
- **Structured logging** with Serilog
- **Health checks** for all critical services
- **Performance metrics** and monitoring
- **Error tracking** and alerting


### Code Quality Standards
- **Unit test coverage** for all business logic
- **Clean architecture** principles
- **SOLID principles** and design patterns
- **Consistent code formatting** and naming conventions



---

**Built with ❤️ for the healthcare community** 
