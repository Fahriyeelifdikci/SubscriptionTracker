# Subscription Tracker

A web-based subscription management application built with ASP.NET Core MVC, designed to help users track recurring payments, manage monthly budgets, and visualize subscription expenses.

## Features

- **User Authentication:** Secure registration, login, and logout using ASP.NET Core Identity.
- **Subscription Management:** Add, view, edit, and delete subscriptions.
- **Dashboard:** View monthly and yearly subscription costs.
- **Budget Management:** Set a monthly spending limit and receive warnings when approaching or exceeding it.
- **Payment Tracking:** Monitor upcoming subscription payments.
- **Expense Visualization:** Interactive charts showing category-based spending and estimated expenses for the next six months.
- **Account Settings:** View account information and securely change passwords.
- **Responsive Design:** User-friendly interface built with Bootstrap.
- **Error Handling:** Custom error pages for invalid routes and unexpected errors.

## Technologies Used

| Technology | Purpose |
|---|---|
| C# / .NET 10 | Backend development |
| ASP.NET Core MVC | Web application architecture |
| Entity Framework Core | Database operations |
| SQL Server | Data storage |
| ASP.NET Core Identity | Authentication and account management |
| Razor Views | Server-rendered user interface |
| Bootstrap | Responsive design |
| Chart.js | Data visualization |
| Git & GitHub | Version control |

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server or SQL Server Express
- Git

### Installation

Clone the repository:

```bash
git clone https://github.com/Fahriyeelifdikci/SubscriptionTracker.git
cd SubscriptionTracker
```

Configure the `DefaultConnection` connection string for your local SQL Server instance. Use .NET user secrets or another secure configuration mechanism for sensitive credentials.

Navigate to the web project:

```bash
cd src/SubscriptionTracker.Web
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

Open the local address shown in the terminal.

## Screenshots

Screenshots of the dashboard, budget management, subscription management, and account settings will be added here.

## Project Structure

```text
SubscriptionTracker/
├── src/
│   └── SubscriptionTracker.Web/
│       ├── Controllers/
│       ├── Data/
│       ├── Extensions/
│       ├── Migrations/
│       ├── Models/
│       ├── Services/
│       ├── ViewModels/
│       ├── Views/
│       ├── wwwroot/
│       └── Program.cs
├── .gitignore
├── README.md
└── SubscriptionTracker.slnx
```

## Future Improvements

- Email reminders for upcoming payments
- Multi-currency support
- Exportable expense reports
- More detailed spending analytics

## Purpose

This project was developed to strengthen practical skills in ASP.NET Core MVC, database design, user authentication, and full-stack web development.

It demonstrates how recurring subscription information can be organized into a simple and accessible financial tracking application.