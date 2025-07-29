# 🕒 Timesheet Tracker

# Timesheet Tracker Application

A comprehensive timesheet tracking system built with ASP.NET Core Web API and Blazor WebAssembly.

## 🏗️ Architecture

This project follows a clean architecture pattern with the following structure:

### Backend (ASP.NET Core Web API)
- **Location**: `src/backend/TimesheetTracker.API/`
- **Framework**: .NET 8.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer Authentication
- **Features**:
  - User management and role-based access control (Admin, Manager, Employee)
  - Project management
  - Time entry tracking with timer functionality
  - Approval workflow for time entries
  - Comprehensive reporting system
  - Structured logging with Serilog

### Frontend (Blazor WebAssembly)
- **Location**: `src/frontend/TimesheetTracker.Client/`
- **Framework**: .NET 9.0 Blazor WebAssembly
- **Features**:
  - Responsive dashboard with statistics
  - Real-time timer functionality
  - User authentication and authorization
  - Time entry management
  - Project management
  - Reports and analytics

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

### Running the Application

1. **Start the Backend API**:
   ```bash
   cd src/backend/TimesheetTracker.API
   dotnet run
   ```
   The API will be available at `https://localhost:7001`

2. **Start the Frontend**:
   ```bash
   cd src/frontend/TimesheetTracker.Client
   dotnet run
   ```
   The frontend will be available at `https://localhost:5001`

3. **Access the Application**:
   - Open your browser to `https://localhost:5001`
   - Register a new account or use the seeded admin account
   - Default admin credentials (if seeded):
     - Email: admin@timesheettracker.com
     - Password: Admin123!

### Building the Entire Solution
```bash
dotnet build
```

## 📋 Features

### For All Users
- ✅ User registration and authentication
- ✅ Dashboard with personal statistics
- ✅ Time tracking with start/stop timer
- ✅ Manual time entry creation and editing
- ✅ Project assignment and tracking
- ✅ Personal time reports

### For Managers
- ✅ Approve/reject time entries
- ✅ View team time reports
- ✅ Project management capabilities
- ✅ Department-level analytics

### For Administrators
- ✅ Complete user management
- ✅ System-wide reporting
- ✅ Project and department management
- ✅ All manager capabilities

## 🗂️ Project Structure

```
├── docs/                          # Project documentation
│   ├── BRD.md                    # Business Requirements Document
│   ├── TSD.md                    # Technical Specification Document
│   └── architecture.md           # Architecture overview
├── src/
│   ├── backend/
│   │   └── TimesheetTracker.API/ # ASP.NET Core Web API
│   │       ├── Controllers/      # API Controllers
│   │       ├── Models/           # Domain Models
│   │       ├── DTOs/             # Data Transfer Objects
│   │       ├── Services/         # Business Logic Services
│   │       ├── Repositories/     # Data Access Layer
│   │       ├── Data/             # EF Core DbContext
│   │       └── Mapping/          # AutoMapper Profiles
│   └── frontend/
│       └── TimesheetTracker.Client/ # Blazor WebAssembly
│           ├── Pages/            # Blazor Pages
│           ├── Layout/           # Layout Components
│           ├── Services/         # Frontend Services
│           └── Models/           # Client-side Models
├── tests/                        # Unit and Integration Tests
├── deploy/                       # Deployment configurations
├── deployment/                   # CI/CD pipeline files
└── security/                     # Security configurations
```

## 🔧 API Endpoints

The API includes the following main endpoints:

- **Authentication**: `/api/auth/login`, `/api/auth/register`
- **Users**: `/api/users` (CRUD operations)
- **Projects**: `/api/projects` (CRUD operations)
- **Time Entries**: `/api/timeentries` (CRUD + timer operations)
- **Reports**: `/api/reports` (various reporting endpoints)

## 🛠️ Technologies Used

### Backend
- ASP.NET Core 8.0 Web API
- Entity Framework Core (SQL Server)
- JWT Authentication
- AutoMapper
- Serilog
- FluentValidation
- BCrypt.Net (password hashing)

### Frontend
- Blazor WebAssembly (.NET 9.0)
- Bootstrap 5
- Bootstrap Icons
- System.Net.Http.Json

### Development & Deployment
- Docker support
- Azure DevOps CI/CD pipelines
- GitHub Actions workflows
- Comprehensive testing framework

## 📝 Recent Updates

- ✅ Fixed duplicate interface definitions in repository layer
- ✅ Backend API builds successfully with comprehensive features
- ✅ Frontend Blazor application created with authentication
- ✅ Dashboard, login, register, and timer pages implemented
- ✅ Solution structure organized for maintainability
- ✅ Both projects build successfully together

## 🔄 Next Steps

1. **Database Setup**: Configure connection string and run migrations
2. **Enhanced Frontend**: Add more pages (Time Entries, Projects, Reports, User Management)
3. **Testing**: Implement comprehensive unit and integration tests
4. **Deployment**: Set up automated deployment pipelines
5. **Documentation**: Complete API documentation with Swagger

## 📞 Support

For questions or issues, please refer to the documentation in the `docs/` folder or create an issue in the repository.

---

**Status**: ✅ Both backend and frontend build successfully
**Last Updated**: January 2025

## 📋 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Getting Started](#-getting-started)
- [API Documentation](#-api-documentation)
- [Development](#-development)
- [Testing](#-testing)
- [Deployment](#-deployment)
- [Security](#-security)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

### Core Functionality
- **Real-time Time Tracking**: Start/stop timers with live tracking
- **Manual Time Entry**: Add time entries manually with date, duration, and descriptions
- **Project Management**: Create and manage projects with budgets and deadlines
- **Multi-Project Allocation**: Track time across multiple projects simultaneously
- **Role-Based Access Control**: Employee, Manager, and Admin roles with appropriate permissions

### Advanced Features
- **Approval Workflows**: Multi-level time entry approval process
- **Smart Suggestions**: AI-powered suggestions based on historical data
- **Reporting & Analytics**: Comprehensive reports with export capabilities (PDF, Excel, CSV)
- **Calendar Integration**: Sync with Google Calendar and Outlook
- **Mobile Support**: Responsive design with offline capabilities
- **Audit Trail**: Complete audit logging for compliance

### Integrations
- **DevOps Tools**: Jira and Azure Boards integration
- **Payroll Systems**: Export to ADP, QuickBooks, Paychex
- **Authentication**: SSO via SAML/OAuth (Okta, Azure AD)

## 🏗️ Architecture

The application follows a clean architecture pattern with clear separation of concerns:

### Technology Stack

**Backend:**
- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- AutoMapper
- Serilog

**Frontend:**
- Blazor Server/WebAssembly
- Bootstrap 5
- Chart.js
- Progressive Web App (PWA)

**Infrastructure:**
- Azure App Service
- Azure SQL Database
- Azure Redis Cache
- Azure Application Insights

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB for development)
- Visual Studio 2022 or VS Code
- Git

### Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-org/timesheet-tracker.git
   cd timesheet-tracker
   ```

2. **Set up the database**
   ```bash
   cd src/backend/TimesheetTracker.API
   dotnet ef database update
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the application**
   - API: https://localhost:7001
   - Swagger UI: https://localhost:7001/swagger

### Configuration

Update `appsettings.json` with your settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  },
  "JwtSettings": {
    "SecretKey": "Your JWT secret key (32+ characters)",
    "Issuer": "TimesheetTracker.API",
    "Audience": "TimesheetTracker.Client"
  }
}
```

## 📚 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and get JWT token |
| GET | `/api/auth/me` | Get current user info |
| POST | `/api/auth/logout` | Logout user |

### Time Entry Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/timeentries` | Get user's time entries |
| POST | `/api/timeentries` | Create new time entry |
| PUT | `/api/timeentries/{id}` | Update time entry |
| DELETE | `/api/timeentries/{id}` | Delete time entry |
| POST | `/api/timeentries/start` | Start a timer |
| POST | `/api/timeentries/{id}/stop` | Stop a timer |
| POST | `/api/timeentries/{id}/approve` | Approve time entry |

For complete API documentation, visit `/swagger` when running the application.

## 💻 Development

### Project Structure

```
src/
├── backend/
│   └── TimesheetTracker.API/
│       ├── Controllers/
│       ├── Models/
│       ├── Services/
│       ├── Repositories/
│       ├── DTOs/
│       └── Data/
└── frontend/
    └── (Frontend implementation)

tests/
└── MyDotNetApp.Tests/

docs/
├── BRD.md
├── TSD.md
└── architecture.md

deploy/
├── azure-pipelines.yml
└── docker-compose.yml
```

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/MyDotNetApp.Tests/
```

### Test Data

The application includes seed data for testing:
- Admin user: `admin@timesheettracker.com` / `Admin@123`
- Sample project for demonstration

## 🚀 Deployment

### Local Development

```bash
# Using Docker Compose
docker-compose -f deploy/docker-compose.yml up -d

# Using .NET CLI
dotnet run --project src/backend/TimesheetTracker.API
```

### Azure Deployment

The project includes CI/CD pipelines for both GitHub Actions and Azure DevOps:
- GitHub Actions: `.github/workflows/ci-cd.yml`
- Azure DevOps: `deploy/azure-pipelines.yml`

## 🔒 Security

### Security Features

- **Authentication**: JWT token-based authentication
- **Authorization**: Role-based access control (RBAC)
- **Data Protection**: Encryption at rest and in transit
- **Input Validation**: Comprehensive input sanitization
- **Audit Logging**: Complete audit trail for compliance

### Compliance

- **GDPR**: Right to be forgotten, data export, consent management
- **DCAA**: Audit trails, approval workflows, time tracking compliance

## 📄 Documentation

- [Business Requirements Document](docs/BRD.md)
- [Technical Specification Document](docs/TSD.md)
- [Architecture Documentation](docs/architecture.md)

---

**Made with ❤️ by the Timesheet Tracker Team**

## Getting Started

To get started with MyDotNetApp, follow these steps:

1. **Clone the repository**:
   ```
   git clone https://github.com/yourusername/MyDotNetApp.git
   ```

2. **Navigate to the project directory**:
   ```
   cd MyDotNetApp
   ```

3. **Build the application**:
   ```
   dotnet build
   ```

4. **Run the application**:
   ```
   dotnet run --project src/MyDotNetApp
   ```

## Documentation

For detailed documentation, please refer to the [docs/index.md](docs/index.md) file.

## Running Tests

To run the unit tests for the application, use the following command:

```
dotnet test tests/MyDotNetApp.Tests
```

## Deployment

For deployment instructions, refer to the [deployment/azure-pipelines.yml](deployment/azure-pipelines.yml) file.

## Security

For information on security practices and how to report vulnerabilities, please see the [SECURITY.md](security/SECURITY.md) file.

## Contributing

Contributions are welcome! Please read the [CONTRIBUTING.md](CONTRIBUTING.md) file for details on our code of conduct, and the process for submitting pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.