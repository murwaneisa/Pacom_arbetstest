# 🛠️ Modbus Device Manager (Pacom Test Project)

## 📋 Description

This is a Blazor Server-based web application for managing Modbus devices and testing basic Modbus communication. The application allows users to:

- View Modbus device states.
- Toggle device states (ON/OFF).
- Synchronize data between the Modbus simulator and the database.
- Interact with a RESTful API (with Swagger UI).
- Run unit tests using xUnit.

---

## ✅ Requirements

To run this project locally, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 or later (with ASP.NET and web development workloads)
- SQL Server (LocalDB or any accessible SQL Server instance)
- Git (optional, for cloning the repo)
- Modbus Simulator (Windows PC ModRSSim2)

---

## 💾 Installation Instructions

1. **Clone the repository**:
   ```bash
   git clone https://github.com/murwaneisa/Pacom_arbetstest.git
   cd Pacom_arbetstest

## 🛠️ Setup & Usage Instructions

### 🔧 Open the Solution
Open `arbetstest_murwan.sln` in **Visual Studio 2022**.

---

### 🔄 Restore NuGet Packages
Visual Studio usually does this automatically. If needed, restore manually:

```bash
dotnet restore

### ⚙️ Update Connection String

Update the `DefaultConnection` in `appsettings.json` to match your SQL Server setup:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ModbusDb;Trusted_Connection=True;"
}

### 🗃️ Apply Database Migrations (If Applicable)

Run the following command to apply any pending EF Core migrations and update the database:

```bash
dotnet ef database update

### ▶️ Usage

### Start the ModRSSim2 simulator

#### Option 1: Using Visual Studio
- Open the solution in Visual Studio 2022.
- Press **F5** or click **Start Debugging** to run the application.

#### Option 2: Using .NET CLI
```bash
cd arbetstest_murwan
dotnet run
The application will be available at: https://localhost:7073

### 📚 API Documentation (Swagger)

You can test and explore the REST API using **Swagger UI**:

🔗 [https://localhost:7073/swagger](https://localhost:7073/swagger)