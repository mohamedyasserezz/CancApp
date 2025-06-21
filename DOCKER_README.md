# CanaApp API Docker Setup

This document provides instructions for dockerizing and deploying the CanaApp API project using Docker and Docker Compose.

## Project Structure

The project consists of:
- **Backend**: .NET 9.0 API with multiple projects (Domain, Application, Persistence, etc.)
- **Database**: SQL Server
- **Cache**: Redis

## Prerequisites

- Docker Desktop installed
- Docker Compose installed
- At least 4GB RAM available for Docker

## Quick Start

### 1. Development Environment

```bash
# Clone the repository
git clone <your-repo-url>
cd CancApp

# Copy environment file
cp env.example .env

# Edit .env file with your configuration
# (See Environment Variables section below)

# Start development environment
docker-compose up --build
```

### 2. Production Environment

```bash
# Copy environment file
cp env.example .env

# Edit .env file with production values
# (See Environment Variables section below)

# Start production environment
docker-compose -f docker-compose.prod.yml up --build -d
```

## Docker Files Overview

### Main Dockerfile
- **`CanaApp.Apis/Dockerfile`**: Existing Dockerfile for the API project (Visual Studio generated)

### Docker Compose Files
- **`docker-compose.yml`**: Main configuration for development
- **`docker-compose.prod.yml`**: Production setup with optimizations

## Environment Variables

Create a `.env` file based on `env.example`:

```bash
# Database Configuration
DB_PASSWORD=YourStrong@Passw0rd

# JWT Configuration
JWT_KEY=YourSuperSecretJwtKeyHereMakeItLongAndSecureForProduction

# Email Configuration
MAIL_PASSWORD=your-email-app-password

# Hangfire Configuration
HANGFIRE_USER=admin
HANGFIRE_PASSWORD=admin123

# Redis Configuration
REDIS_PASSWORD=redis123
```

## Services

### Development Services
- **API**: .NET API on ports 8080/8081
- **SQL Server**: Database on port 1433
- **Redis**: Cache on port 6379

### Production Services
- **API**: .NET API on ports 8080/8081
- **SQL Server**: Database on port 1433
- **Redis**: Cache on port 6379

## Commands

### Development
```bash
# Start development environment
docker-compose up

# Start with rebuild
docker-compose up --build

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Production
```bash
# Start production environment
docker-compose -f docker-compose.prod.yml up -d

# View logs
docker-compose -f docker-compose.prod.yml logs -f

# Stop services
docker-compose -f docker-compose.prod.yml down

# Remove volumes (WARNING: This will delete all data)
docker-compose -f docker-compose.prod.yml down -v
```

### General Commands
```bash
# Build specific service
docker-compose build api

# Rebuild and start
docker-compose up --build

# View running containers
docker-compose ps

# Execute commands in container
docker-compose exec api dotnet --version

# View container logs
docker-compose logs api
```

## Access Points

### Development
- **API**: http://localhost:8080
- **Database**: localhost:1433
- **Redis**: localhost:6379

### Production
- **API**: http://localhost:8080
- **Database**: localhost:1433
- **Redis**: localhost:6379

## Database Setup

The SQL Server container will be created automatically. You may need to:

1. **Wait for database to be ready**: The container takes a few minutes to start
2. **Run migrations**: If you have Entity Framework migrations
3. **Seed data**: If you have initial data to load

```bash
# Check database status
docker-compose logs sqlserver

# Run migrations (if needed)
docker-compose exec api dotnet ef database update
```

## API Endpoints

Your API will be available at:
- **Base URL**: http://localhost:8080
- **Swagger/OpenAPI**: http://localhost:8080/swagger (if configured)
- **Health Check**: http://localhost:8080/health (if configured)

## Troubleshooting

### Common Issues

1. **Port conflicts**: Ensure ports 1433, 6379, 8080, 8081 are available
2. **Memory issues**: Increase Docker memory limit to at least 4GB
3. **Database connection**: Wait for SQL Server to fully start (check logs)
4. **Build failures**: Clear Docker cache with `docker system prune -a`

### Useful Commands

```bash
# Clear Docker cache
docker system prune -a

# Remove all containers and volumes
docker-compose down -v

# View resource usage
docker stats

# Check container health
docker-compose ps
```

### Logs

```bash
# View all logs
docker-compose logs

# View specific service logs
docker-compose logs api
docker-compose logs sqlserver
docker-compose logs redis

# Follow logs in real-time
docker-compose logs -f
```

## Security Considerations

1. **Change default passwords** in `.env` file
2. **Use strong JWT keys** for production
3. **Enable HTTPS** in production (configure SSL certificates)
4. **Restrict network access** in production
5. **Regular security updates** for base images

## Performance Optimization

1. **Resource limits**: Adjust memory/CPU limits in docker-compose.prod.yml
2. **Caching**: Redis is configured for caching
3. **Database optimization**: Consider connection pooling

## Monitoring

### Health Checks
All services include health checks:
- API: Built into .NET Core
- Database: SQL query check
- Redis: Ping command

### Logging
- Application logs: `./logs` directory
- Database logs: Docker logs

## Backup and Recovery

### Database Backup
```bash
# Create backup
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $DB_PASSWORD -Q "BACKUP DATABASE CancApp TO DISK = '/var/opt/mssql/backup.bak'"

# Copy backup from container
docker cp $(docker-compose ps -q sqlserver):/var/opt/mssql/backup.bak ./backup.bak
```

### Volume Backup
```bash
# Backup volumes
docker run --rm -v cancapp_sqlserver_data:/data -v $(pwd):/backup alpine tar czf /backup/sqlserver_backup.tar.gz -C /data .
docker run --rm -v cancapp_redis_data:/data -v $(pwd):/backup alpine tar czf /backup/redis_backup.tar.gz -C /data .
```

## Support

For issues related to:
- **Docker setup**: Check this README and Docker documentation
- **Application**: Check application logs and documentation
- **Database**: Check SQL Server logs and connection strings
- **Performance**: Monitor resource usage and adjust limits 