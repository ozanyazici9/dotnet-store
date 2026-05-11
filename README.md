# 🛒 dotnet-store

A full-featured **E-Commerce web application** built with **ASP.NET Core MVC**, featuring a customer-facing storefront and a fully functional admin panel. The project demonstrates clean architecture principles, role-based access control, and a modern UI experience.

---

## 🌟 Features

### 🏪 Storefront
- Category-based navigation with dynamic sidebar
- Product listing with discount badges
- Product detail page with image gallery, specifications, and size/variant selection
- Add to cart & add to wishlist functionality
- Responsive design with a clean purple-themed UI
- Email subscription in footer

### 🛠️ Admin Panel
- Dashboard with key metrics: Sales, Orders, Products, Messages
- Full **CRUD** operations for:
  - **Categories** — manage category names and URLs
  - **Products** — manage images, prices, active status, homepage visibility, and category assignment
  - **Sliders** — banner management for the homepage carousel
  - **Users & Roles** — role-based access control
  - **Orders** — view and manage order statuses

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

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 8) |
| Language | C# |
| ORM | Entity Framework Core |
| Database | SQL Server / SQLite |
| Authentication | ASP.NET Core Identity |
| Frontend | Bootstrap 5, HTML/CSS, JavaScript |
| Architecture | MVC, Repository Pattern |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server or SQLite
- Visual Studio 2022 / VS Code

### Installation

```bash
# Clone the repository
git clone https://github.com/ozanyazici9/dotnet-store.git
cd dotnet-store

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run
```

The app will be available at `https://localhost:5271`

---

## 🗂️ Project Structure

```
dotnet-store/
├── Controllers/          # MVC Controllers (Admin & User)
├── Models/               # Entity models and ViewModels
├── Views/                # Razor Views
│   ├── Admin/            # Admin panel views
│   └── Shared/           # Layouts and partials
├── Data/                 # DbContext and migrations
├── Repositories/         # Data access layer
└── wwwroot/              # Static files (CSS, JS, images)
```

---

## 🔐 Roles & Access

| Role | Access |
|---|---|
| **Admin** | Full access to admin panel, product/category/order management |
| **User** | Browse products, add to cart, manage wishlist, place orders |

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

## 👤 Author

**Ozan Yazıcı**  
[![GitHub](https://img.shields.io/badge/GitHub-ozanyazici9-181717?style=flat&logo=github)](https://github.com/ozanyazici9)  
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Ozan%20Yazıcı-0077B5?style=flat&logo=linkedin)](https://www.linkedin.com/in/ozan-yaz%C4%B1c%C4%B1-a5025a236/)

