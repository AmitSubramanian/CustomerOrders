**Documentation and Testing Notes**

- Download all files from this repository to your local machine.  Open .sln in VS.  Run .sln using Ctrl-F5, and proceed as follows.
- Web API Launch URL:  http://localhost:50744/api/customers
- For documentation, or testing the API, please access the Swagger link:  http://localhost:50744/
- To assist with testing, the database is seeded with data (file: Models\CustomerOrdersContext.cs)

---

**Architecture Notes**

- The solution uses:
	.NET Core version 2.1, sqlite database, and xUnit
- The code is separated into layers: Controller, Service, Models (database).
