# LibrarySystemProject

### Technology Stack

- asp.net mvc5

### Develop tools

- Microsoft Visual Studio 2017
- sql server 2012

### Version Management tools

- Github

### Project Portfolio Instructions

You need **asp.net mvc5** development environment. 

1. Download **vs2017** and **sql server 2012**. And download **Github for windows**.
2. clone Repository from **LibrarySystemProject** from origin.
3. Open project in vs2017, and display all the file. **Copy all the new files to vs2017 from local respository.**
4. Update database. Just execute the following commands.
	```
	enable-migrations
	add-migration init
	update-database
	```
	and **mvcdemo** database is created at local.
5. In the table **BorrowAndReturn**, you should change the **datetime** type to **datetime2(7)** type in the 
**BorrowTime** field and **ReturnTime** field.