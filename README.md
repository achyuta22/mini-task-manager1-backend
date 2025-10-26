# 🧩 Mini Task Manager API
A **minimal yet powerful Task Management backend** built using **ASP.NET Core (.NET 8)**, **Entity Framework Core**, and **JWT Authentication**. It allows users to register, log in, create and manage tasks, set dependencies, and view scheduled (upcoming / overdue / today) tasks.

---
## 🚀 Features
- 🔐 User authentication with JWT  
- 🧾 Full CRUD operations for tasks  
- 🔗 Task dependencies  
- ⏰ Schedule API for upcoming and overdue tasks  
- ⚡ RESTful API responses  
- 🧱 Built using Entity Framework Core (SQLite / PostgreSQL compatible)

---
## 🛠️ Tech Stack
| Layer | Technology |
|-------|-------------|
| Language | C# (.NET 8) |
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Auth | JWT Bearer Tokens |
| JSON Serialization | System.Text.Json |

---
## ⚙️ Setup Instructions
### 1️⃣ Clone Repository
```bash
git clone https://github.com/yourusername/mini-task-manager.git
cd mini-task-manager
```

### 2️⃣ Configure Environment
Before running the project, configure your environment by editing **appsettings.json** (or adding a `.env` file).  
This defines your database connection and JWT secret key.
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=taskmanager.db"
  },
  "Jwt": {
    "Key": "your_super_secret_key_here",
    "Issuer": "MiniTaskManager"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 3️⃣ Run Migrations
```bash
dotnet ef database update
```

### 4️⃣ Start the API
```bash
dotnet run
```
The API will start on `http://localhost:5000` (or the port specified in launch settings).

---
## 📡 API Endpoints

### 🔐 Auth Routes
| Method | Endpoint | Description |
|---------|-----------|-------------|
| POST | /api/auth/register | Register a new user |
| POST | /api/auth/login | Log in and receive a JWT token |

#### Example – Register
```json
POST /api/auth/register
{
  "username": "bharat",
  "email": "bharat@example.com",
  "password": "secure123"
}
```

#### Example – Login
```json
POST /api/auth/login
{
  "email": "bharat@example.com",
  "password": "secure123"
}
```

**Response**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

---
### ✅ Task Routes
| Method | Endpoint | Description |
|---------|-----------|-------------|
| GET | /api/tasks | Get all tasks for the logged-in user |
| GET | /api/tasks/{id} | Get a specific task by ID |
| POST | /api/tasks | Create a new task |
| PUT | /api/tasks/{id} | Update an existing task |
| DELETE | /api/tasks/{id} | Delete a task |

#### Example – Create Task
```json
POST /api/tasks
Authorization: Bearer <token>
{
  "title": "Complete Project Report",
  "description": "Finish the project report before submission",
  "dueDate": "2025-10-30T18:00:00",
  "priority": "High"
}
```

**Response**
```json
{
  "id": 1,
  "title": "Complete Project Report",
  "status": "Pending",
  "dueDate": "2025-10-30T18:00:00"
}
```

---
### 🔗 Task Dependencies
| Method | Endpoint | Description |
|---------|-----------|-------------|
| POST | /api/tasks/{taskId}/dependencies | Add dependencies for a task |
| GET | /api/tasks/{taskId}/dependencies | Get all dependencies for a task |

#### Example
```json
POST /api/tasks/3/dependencies
{
  "dependentTaskIds": [1, 2]
}
```

---
### ⏰ Schedule API
| Method | Endpoint | Description |
|---------|-----------|-------------|
| GET | /api/schedule/{projectid} | Get tasks with due dates in the future |


#### 🔍 Explanation
The **Schedule API** automatically categorizes tasks based on their due dates and dependencies tasks using topological sort:

These APIs help in building **daily reminders**, **task dashboards**, or **cron-based notifications**.

#### Example – Upcoming Tasks
```bash
GET /api/schedule/10
Authorization: Bearer <token>
```

**Response**
```json
[
  {
    "id": 5,
    "title": "Prepare for Interview",
    "dueDate": "2025-10-28T09:00:00",
    "status": "Pending"
  }
]
```

---
## 📘 Folder Structure
```
MiniTaskManager/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── TaskController.cs
│   └── ScheduleController.cs
│
├── Data/
│   ├── ApplicationDbContext.cs
│
├── Models/
│   ├── User.cs
│   ├── TaskItem.cs
│   └── TaskDependency.cs
│
├── Services/
│   ├── AuthService.cs
│   ├── TaskService.cs
│   └── ScheduleService.cs
│
└── Program.cs
```

---
## 🧪 Testing
Use **Postman** or **Thunder Client** for API testing.
1. Register and log in to get a JWT token.  
2. Add the token in headers for all `/api/tasks` and `/api/schedule` routes.

Example Header:
```
Authorization: Bearer <token>
```

---
## 🧱 Database Schema (Simplified)
**Users**
| Column | Type |
|--------|------|
| Id | int |
| Username | string |
| Email | string |
| PasswordHash | string |

**Tasks**
| Column | Type |
|--------|------|
| Id | int |
| Title | string |
| Description | string |
| DueDate | DateTime |
| Status | string |
| UserId | int (FK → Users.Id) |

**TaskDependencies**
| Column | Type |
|--------|------|
| Id | int |
| TaskId | int |
| DependentTaskId | int |

---
## 📄 License
This project is licensed under the **MIT License**.

---
## 👨‍💻 Author
**Ravihal Bharat Achyuta**  
Backend Developer | IIT Roorkee  
🔗 [GitHub](https://github.com/achyuta22)
