To create tables in Entity Framework:

Open the View/SQL Server Object Explorer menu
	Expand SQL Server
		Expand (localdb)
			Expand Databases
Right-click on Databases / Add New Database
Right-click on the created db / Refresh
Right-click on the created db / Properties
In the properties window, look for Connection string
Copy

In solution explorer - Presentation Layer, look for the appsettings.json file
Replace the string in red next to "Connection"

Click again on Solution Explorer and look in project Infrastructure for the appsettings.json file
Replace the string in red next to "Connection"

Search for the Infra.Repository project / Right-click / Set as startup project
Click on the Tools / Nuget Package Manager / Package Manager Console menu, a window will open at the bottom of Visual Studio
Next to "Defaul Project", choose Infra.Repository
At the cursor, type Add-Migration any_name (enter)
Type Update-Database (enter)

Go back to Solution Explorer and look for Presentation / Right click / Set as startup project and run the project

==========================================================================================================================

MongoDB Compass Configuration

Download and install the latest version of MongoDB Compass. https://www.mongodb.com/try/download/compass

Open MongoDB Compass after installation.

In MongoDB Compass, click the "New Connection" button or "Fill in Connection Fields Manually".

On the connection configuration screen, enter the following details:

Hostname: Enter the hostname of your MongoDB, usually localhost if on-premises, or the IP address or domain name of your production server.

Port: Enter the port where MongoDB is running (27017)

Database: Enter the name of the database you want to view, which can be obtained from your code as shown in appsettings.json.

Click the "Connect" button to establish the connection.

MongoDB Compass will connect to the database and you will see the data in the graphical interface.

Next to the database name, create the collections users, products, sales, saleItems.

==========================================================================================================================

The first time you use the system, sign in and login normally and update the UserProfile=1 field in mongoDB collection "users" and SQL server table "User". After that, you can set any user as an administrator by clicking on "Users List" in the main page and clicking update.







