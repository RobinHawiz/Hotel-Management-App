## The App Idea
To create a hotel management application, which includes a web portal that allows people to book an available room and a desktop app for when they check in (when they arrive at the hotel).

I'm going to create a minimum viable product that will allow the customer to get going and to identify what features should be added later on.

Side note: My early commit messages sometimes refer to stored procedures as storage procedures, which is wrong.

## How to run the demo
### Dependencies
The demo (located in the SQLiteRelease folder) application was published as self-contained with the runtime identifier win-x64, which means that the application has been compiled and its dependencies has been published to their own directory. 

In short, you don't have to install anything to run the demo. However, the os it can be run on is limited to windows 10 & 11, because the project was made using .NET 8 (and RID win-x64).

### Run the applications
The following folder structure shows where the .exe files are located:

    .
    ├── ...                   
    ├── SQLiteRelease           # Contains the runnable demo version of the applications (using SQLite db).
    │   ├── DesktopApp              # WPF app that checks in the guests that booked a room from the web portal.
    │   │   ├── HotelApp.Desktop.exe    # Desktop application. 
    │       └── ...                     # A bunch of dll-files and whatnot.
    │   ├── SQLiteDB                # SQLite database.
    │   │   ├── HotelAppDb.db   
    │   ├── WebApp                  # ASP.NET Razor Pages app that allows users to book a room at a specific date.
        │   ├── HotelApp.Web.exe        # Web portal application. 
    │   └── ...                         # A bunch of dll-files and whatnot.
    └── ...
Just run the .exe files to start the applications.

## How to run the project
### Dependencies
To run the projects with Microsoft Visual Studio you need:
- MinimumVisualStudioVersion = 10.0.40219.1
- Workloads:
  - .NET desktop development.
  - Data storage and processing.
- NuGet packages get installed automatically.

### Running the project

First get the project:
```
git clone https://github.com/RobinHawiz/Hotel-Management-App.git
```
The following folder structure shows where the solution file is located:

    .
    ├── HotelApp                # Contains the whole Hotel app management project.
    │   ├── HotelApp.sln            # The project solution file.
    │   ├── ...                     # Other project folders.
    └── ...
Open the HotelApp.sln with visual studio.

#### SQL Server database

Follow the steps to make the app work with the <ins>SQL Server database</ins>:

1. Publish the HotelAppDB database project.

    - Right click on the HotelAppDB that is located in the Solution Explorer and click on publish, then in the publish database window you go to Edit -> Browse -> Local, then click on MSSQLLocalDB and press ok.

3. Now you need to change the appsettings.json files that are located in both the HotelApp.Web and HotelApp.Desktop (found in the Solution Explorer).

    - Change the "DatabaseChoice" value from "SQLite" to "SQL" to use the published SQL server database.

You're good to go.

#### SQLite database

Follow the steps to make the app work with the <ins>SQLite database</ins>:

1. You need a SQLite database.

Copy the HotelAppDb.db file from:

    .
    ├── SQLiteRelease                # Contains the runnable demo apps.
    │   ├── SQLiteDB                     # Has SQLite databases.
    │   │   ├── HotelAppDb.db                # The SQLite database.
    └── ...

Go to your C: drive directory and create a folder named Dbs and paste the HotelAppDb.db inside your new directory called Dbs.

You're good to go.

## Issues you might run into
### Troubles with SQL Server database

If you get a Data.SqlClient.SqlException because the database could not be found:

  1. Open the SQL Server Object Explorer, then navigate to SQL Server -> (localdb) -> Databases -> HotelAppDB, then right click HotelAppDB and click on properties and you'll find the connection string there. 

  2. Copy that string to the appsettings.json files and replace whatever "SqlDb" has as a value with that string. 

If you could not find HotelAppDB in the SQL Server Object Explorer, make sure to click on the refresh button (blue arrow) that is located at the top of the SQL Server Object Explorer, after you've published the database project.

### Troubles with SQLite database

If you get a Data.SQLite.SQLiteException: 'unable to open database found':

Then the connection string in the appsettings.json files is not correct. Make sure you set a correct filepath for where the .db file is located.

### Troubles with running the HotelApp.Web project

If you try to run the HotelApp.Web inside Microsoft Visual Studio it will prompt you about some trust warnings with the self generated ssl certificate called localhost or whatever. Just install it. 

Now when you go to the site (by copying the url from the command line) the web browser will tell you that the connection to the site is not secure. That's because your using http. Follow the url that uses https instead and that should resolve the issue. Either way your hosting the server locally, so it doesn't really matter. However if the website communicated with a database that was located on a remote server, that'd be a different story.
