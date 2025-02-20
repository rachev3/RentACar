# 🚗 RentACar - Car Rental Management System

A modern, full-featured car rental management system built with ASP.NET Core MVC 8.0. This web application provides a comprehensive solution for managing car rentals, including user authentication, vehicle management, booking system, and administrative features.

## 🛠️ Built With

- ASP.NET Core MVC 8.0
- Entity Framework Core
- Microsoft SQL Server
- ASP.NET Core Identity
- Bootstrap (Frontend)
- Entity Framework Core Tools
- ASP.NET Core Diagnostics
- Razor Views
- jQuery

## ✨ Features

✅ User Authentication & Authorization  
✅ Vehicle Management (CRUD operations)  
✅ Booking System  
✅ Admin Dashboard  
✅ User Roles & Permissions  
✅ Vehicle Image Management  
✅ Responsive Design  
✅ Data Seeding  
✅ Entity Framework Integration
✅ Razor View Templates
✅ Client-side Validation

## 🏗️ Project Structure

```
RentACar/
├── Areas/
├── Controllers/
├── Data/
├── Models/
├── Services/
├── Seeders/
├── ViewModels/
├── Views/
│   ├── Shared/
│   ├── Home/
│   ├── Car/
│   ├── Rent/
│   └── Admin/
└── wwwroot/
```

## 🔧 Installation

### Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 (recommended) or VS Code

## 👥 User Roles

- **Admin**: Full system access and management
- **Customer**: Browse vehicles and make bookings

## 🔐 Security Features

- ASP.NET Core Identity for authentication
- Role-based authorization
- Secure password hashing
- HTTPS enforcement
- Anti-forgery token validation
- User secrets management

## 🗃️ Database Schema

The application uses Entity Framework Core with the following main entities:

- Users
- Vehicles
- Rent

## 📱 Main Features & Pages

### Authentication Pages

- Login
- Register

### Vehicle Management

- Vehicle Listing
- Vehicle Details
- Add/Edit Vehicle
- Vehicle Categories
- Vehicle Search and Filtering

### Booking System

- Booking Calendar
- Create Booking

### Admin Area

- Dashboard
- User Management
- Vehicle Management

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.
