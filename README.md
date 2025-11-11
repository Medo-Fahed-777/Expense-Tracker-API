# Expense Tracker API

A backend REST API that allows users to track, categorize, and analyze their daily expenses.  
This project focuses on personal finance management and provides reporting tools to help users understand their spending habits.

---

## ✨ Features

- User Registration & Login (JWT Authentication)
- Create, update, delete, and view expenses
- Organize expenses using custom categories
- Filter expenses by category, and amount range
- Sort expenses by date, amount, or category
- Monthly spending report (with totals and per-category breakdowns)
- Lifetime summary report (top categories, highest expense, total spending)
- Secure user-based data access (each user sees only their data)

---

## 🏛️ Architecture Overview

The project is organized using a layered structure to separate responsibilities:

API Layer (Controllers)
Service Layer (Business Logic)
Data Access Layer (Repositories / EF Core)
Database (SQL Server)


- **Entity Framework Core** is used for database access.
- **JWT** is used to authenticate and authorize users.
- **AutoMapper** is used to map entities to DTOs.
- **Validation** is applied on incoming request models.

---

## 🗄️ Database Entities

### User
- Id
- Username
- Email
- PasswordHash

### Category
- Id
- Name
- UserId (each user has their own categories)

### Expense
- Id
- Amount
- Description
- Date (auto-set by system, not editable)
- CategoryId
- UserId

---

## 🔐 Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST   | `/api/Account/Register` | Register a new user |
| POST   | `/api/Account/Login` | Login and receive JWT token |
| PUT   | `/api/Account/UpdateProfile` | Update User Profile |
| Delete   | `/api/Account` | Delete User Profile |

---

## 💰 Expense Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/Expense` | Get all expenses (supports filtering & sorting) |
| GET    | `/api/Expense/{id}` | Get a specific expense |
| POST   | `/api/Expense` | Create a new expense |
| PUT    | `/api/Expense/{id}` | Update amount, description, or category |
| DELETE | `/api/Expense/{id}` | Delete an expense |

### Example Query Parameters for Filtering:

/api/Expense?CategoryName=Food&SortBy=amount&IsDescending=true&PageSize=3&PageNumber=1


---

## 🗂️ Category Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/Category` | Get all user categories |
| POST   | `/api/Category` | Create a category |
| PUT    | `/api/Category/{id}` | Update a category |
| DELETE | `/api/Category/{id}` | Delete a category |

---

## 📊 Reporting Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/Report/MonthlyReport` | Monthly spending report  |
| GET    | `/api/Report/SummaryReport` | Lifetime spending summary |

### Example Query Parameters for Monthly Report:
/api/Report/MonthlyReport?month=11&year=2025


### Example Monthly Report Response:

```json
{
  "monthName": "October",
  "totalExpenses": 93500,
  "topCategories": [
    {
      "id": 1,
      "name": "Food",
      "userId": "#############################",
      "totalAmount": 59500
    }
  ],
  "averagePerDay": 3016.13
}

### Example Summary Report Response:

```json
{
  "totalLifeTimeSpent": 101500,
  "totalExpensesRecorded": 13,
  "topSpendingCategories": [
    {
      "id": 1,
      "name": "Food",
      "userId": "8b2cc2c9-3188-4156-8aed-43f8a6520a86",
      "totalAmount": 67500
    },
    {
      "id": 2,
      "name": "Transport",
      "userId": "8b2cc2c9-3188-4156-8aed-43f8a6520a86",
      "totalAmount": 34000
    }
  ],
  "highestSingleExpense": {
    "id": 11,
    "ammount": 25000,
    "description": "Taxi to the Althawra Street",
    "username": "medo",
    "userId": "8b2cc2c9-3188-4156-8aed-43f8a6520a86",
    "categoryName": "Transport",
    "categoryId": 2,
    "date": "2025-10-30T20:56:14.6534799"
  },
  "firstExpenseDate": "30 Oct 2025",
  "mostRecentExpenseDate": "05 Nov 2025"
}

🛠️ Tech Stack

C# .NET Core Web API

Entity Framework Core (SQL Server)

JWT Authentication

AutoMapper

