# InsureApi
A simple web app to manage a list of high value items

## Features
* User can add an item
* User can delete an item
* User can view all items with categories and total sum

## Requirements

* Dotnet Core 3.1
* MSSQL Server

## API Endpoints
<table>
<tr><th>HTTP VERB</th><th>ENDPOINTS</th><th>DESCRIPTION</th><th>BODY PARAMS</th></tr>
<tr><td>POST</td><td>/api/v1/items</td><td>Creates an item</td><td>Name, Value, CategoryId</td></tr>
<tr><td>DELETE</td><td>/api/v1/items/:id</td><td>Deletes an item</td><td></td></tr>
<tr><td>GET</td><td>/api/v1/categories/items</td><td>Retrieves the list of items with their categories</td><td></td></tr>
</table>

## Available Categories
<table>
<tr><th>Id</th><th>Category</th></tr>
<tr><td>1</td><td>Electronics</td></tr>
<tr><td>2</td><td>Clothing</td></tr>
<tr><td>3</td><td>Kitchen</td></tr>
</table>

# Getting Started
**Via Cloning The Repository**
```
# Clone the app
git clone https://github.com/Stylll/insureapi.git

# Switch to directory
cd insureapi

# Connect Database
update the connection string in the Insure.Api/appsettings.json file with your database connection

# Package dependencies
ensure to go through the dependencies and have them installed

# Run Migration
dotnet ef --startup-project Insure.Api/Insure.Api.csproj database update

#Start the application
dotnet run -p Insure.Api/Insure.Api.csproj

Open http://localhost:5001 in your browser, you should see the swagger documentation page
```
