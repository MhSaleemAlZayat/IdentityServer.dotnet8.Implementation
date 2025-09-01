# Identity Server – .NET 8 Implementation

This repository is a **practical implementation of the Duende IdentityServer Quickstarts**, based on the official guide:  
👉 [IdentityServer Quickstart Overview](https://docs.duendesoftware.com/identityserver/quickstarts/0-overview/?utm_source=chatgpt.com)

## 🔗 Useful Resources

- 🌐 **IdentityServer Website**: [https://duendesoftware.com](https://duendesoftware.com?utm_source=chatgpt.com)
- 📖 **Documentation**: [https://docs.duendesoftware.com/identityserver](https://docs.duendesoftware.com/identityserver?utm_source=chatgpt.com)
- 💻 **Duende GitHub Repositories**: [https://github.com/DuendeSoftware](https://github.com/DuendeSoftware?utm_source=chatgpt.com)

## 🏗️ Solution Structure

This implementation consists of **three main projects**:

1. **IdentityServer**
	- Implements the Identity Server itself
    - Uses **ASP.NET Identity** for user management
    - Configured with **Duende IdentityServer** to issue tokens (Access, Refresh, ID tokens)
    - Backed by **Entity Framework** for persistence
2. **APIResource**
    - An ASP.NET Core API project
    - Represents a protected resource that requires tokens issued by IdentityServer
    - Demonstrates how APIs validate tokens and enforce scopes/claims
3. **WebClient**
    - An ASP.NET MVC (.NET 8) client application
    - Integrates with IdentityServer for login/logout
    - Consumes `APIResource` using secure access tokens
## 🚀 Why This Project?

This solution is designed as a **reference implementation** to:
- Learn and experiment with Duende `IdentityServer`
- Demonstrate how to secure APIs and clients with **OAuth 2.0 / OIDC**
- Provide a starting point for building scalable, real-world identity infrastructures

## 🚀 Getting Started

### ✅ Prerequisites

- .NET 8 SDK
- [SQL Server / SQLite / PostgreSQL] (depending on your Entity Framework provider)
- An IDE such as [Visual Studio 2022](https://visualstudio.microsoft.com/?utm_source=chatgpt.com)
### ⚙️ Setup & Run
1. Add **IdentityServer** templates for dotnet CLI
``` bach
   dotnet new install Duende.Templates
```

2. **Clone the repository**
```bach 
   git clone https://github.com/MhSaleemAlZayat/IdentityServer.dotnet8.Implementation.git
  cd your-repo-name
```
3. Go to `appsettings.json` in `IdentityServer` project and uncommit `DefaultConnection` then change connection string setting
``` json
//"DefaultConnection": "Server={YOUR-SERVER-NAME};Database=IdentityServer;User Id={YOUR-USER-ID}; Password={YOUR-PASSWORD};Trusted_Connection=False;MultipleActiveResultSets=true;Encrypt=False"
```
4. **Run the projects**
- Start **IdentityServer** first
- Then run **APIResource**
- Finally, run **WebClient**

5. **Test the flow**
- Navigate to `https://localhost:5003`
- Login via IdentityServer using `ahmed` or `samira` with `P@ssw0rd` password.
- Access the protected API through the WebClient
- Nagigate to `https://localhost:5003/Home/BrowseEmployees` to browse the randome employees.
