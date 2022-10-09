# SportCommentary
Web application created in Blazor <img src="https://upload.wikimedia.org/wikipedia/commons/d/d0/Blazor.png" width="32" height="32"> and .NET 6 for running sports coverage.

With SignalR, the report is refreshed in real time for all users.

<br/>
<img src="https://owik100.github.io/Portfolio/images/Projects/Sport%20Commentary/Sport%20Commentary.gif" width="480" height="456">

With SportCommentary you can:

- Define sports
<img src="https://owik100.github.io/Portfolio/images/Projects/Sport%20Commentary/sports.png" width="622" height="459">

 - Manage events for every sport
 <img src="https://owik100.github.io/Portfolio/images/Projects/Sport%20Commentary/events.png" width="622" height="459">

- Have an overview of all live commenteries and the ability to create them
 <img src="https://owik100.github.io/Portfolio/images/Projects/Sport%20Commentary/Strong%20glowna.png" width="622" height="459">
 <img src="https://owik100.github.io/Portfolio/images/Projects/Sport%20Commentary/new%20live.png" width="622" height="459">
 
  - Manage users roles
  
## Configuration
1. Add connection string in **appsettings.json** to DefaultConnection
```
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BestGiftsLocal;Trusted_Connection=True;"
  }

```
2. Create Migration:    
Open the Package Manager Console from the menu Tools -> NuGet Package Manager -> Package Manager Console in Visual Studio and execute the following command to add a migration.
```
add-migration InitMigration
```
 If you are using dotnet Command Line Interface, execute the following command.
```
dotnet ef migrations add InitMigration
```
3. Creating or Updating the Database:  
Use the following command to create or update the database schema.
 ```
Update-Database
```
Or in dotnet Command Line Interface
```
dotnet ef database update
```
