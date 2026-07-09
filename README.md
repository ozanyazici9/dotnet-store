## 🌐 Live Demo

https://ozanyazici.com.tr

# 🛒 dotnet-store

A full-featured **E-Commerce web application** built with **ASP.NET Core MVC (.NET 9)**. The project includes a customer-facing storefront, a role-based admin panel, SQL Server integration, and demonstrates clean architecture principles using Entity Framework Core and ASP.NET Core Identity.

---

## 🌟 Features

### 🏪 Storefront

- Category-based product browsing
- Dynamic sidebar navigation
- Product detail page with image gallery
- Discount badges
- Add to cart
- Wishlist support
- Responsive Bootstrap 5 design
- Email subscription

### 🛠️ Admin Panel

- Dashboard with sales & order statistics
- Category management (CRUD)
- Product management (CRUD)
- Slider management
- User & Role management
- Order management

---

## 📸 Screenshots

### 🏠 Homepage

<img src="https://github.com/user-attachments/assets/3f4f29a6-9935-43d8-a3ca-1e51185c013f" width="800"/>

### 📦 Product Detail

<img src="https://github.com/user-attachments/assets/da96a1a1-f7f5-44b4-af4f-b3456e3363c9" width="800"/>

### 🖥️ Admin Dashboard

<img src="https://github.com/user-attachments/assets/4609056e-c215-48d7-8cdd-ae9b263dcfb0" width="800"/>

### 📋 Admin – Category List

<img src="https://github.com/user-attachments/assets/a84deb83-b066-48e6-aef4-8f12b6d33d8e" width="800"/>

### ➕ Admin – Add Category

<img src="https://github.com/user-attachments/assets/2e7f749c-d14b-4acc-b081-5ce515363a27" width="800"/>

### 📦 Admin – Product List

<img src="https://github.com/user-attachments/assets/bc7f386b-4147-421f-b2cf-116e981b2734" width="800"/>

---

## 🧰 Tech Stack

| Layer          | Technology                         |
| -------------- | ---------------------------------- |
| Framework      | ASP.NET Core MVC (.NET 9)          |
| Language       | C#                                 |
| ORM            | Entity Framework Core              |
| Database       | SQL Server                         |
| Authentication | ASP.NET Core Identity              |
| Frontend       | Bootstrap 5, HTML, CSS, JavaScript |
| Architecture   | MVC, Service Layer                 |

---

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

```bash
# Clone the repository
git clone https://github.com/ozanyazici9/dotnet-store.git

cd dotnet-store

# Restore packages
dotnet restore

# Configure your SQL Server connection string
# in appsettings.json

# Run the application
dotnet run
```

On first startup, the application will automatically:

- Apply all Entity Framework Core migrations
- Create the database schema
- Seed the default roles and administrator account (if they do not already exist)

```
> **Note:** Before running the application, create an empty SQL Server database and update the connection string in `appsettings.json`.

The application will automatically apply pending Entity Framework Core migrations on startup.

The application will be available at:

```

https://localhost:5271

```

---

## 🗂️ Project Structure

```

dotnet-store/
├── Controllers/
├── Models/
├── Services/
├── Views/
│ ├── Admin/
│ └── Shared/
├── Migrations/
├── wwwroot/
└── Program.cs

```

---

## 🔐 Default Roles

| Role     | Description                                   |
| -------- | --------------------------------------------- |
| Admin    | Full access to the administration panel       |
| Customer | Browse products, manage cart and place orders |

On the first application startup, default roles and an administrator account are created automatically if they do not already exist.

---

## 📄 License

This project is licensed under the MIT License.

---

## 👤 Author

**Ozan Yazıcı**

GitHub: https://github.com/ozanyazici9

LinkedIn: https://www.linkedin.com/in/ozan-yaz%C4%B1c%C4%B1-a5025a236/
```
