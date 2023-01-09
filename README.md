# SmartCarWashTest
This is test for Smart car wash

1. Develop data models of the main entities:
  - Product – product model, consisting of the following mandatory properties:
    - Id - Identifier
    - Name - Name
    - Price - cost
    - Extending the model with other properties (necessary in terms of developer) - at the discretion of the developer
  - SalesPoint - point of sale of goods, consisting of the following mandatory properties:
    - Id - Identifier
    - Name - Name
    - ProvidedProducts – list of entities (ProvidedProduct) available for sale items, with the current available quantity for each item. Essence ProvidedProduct has the following required properties:
    - ProductId - product identifier
    - ProductQuantity - quantity
    - Extending the model with other properties (necessary in terms of developer) - at the discretion of the developer
  - Buyer - a buyer, a person who purchases a product or service in one of the points of sale
    - Id - Identifier
    - Name - Name
    - SalesIds - a collection of all purchase IDs ever carried out by this buyer
    - Extending the model with other properties (necessary in terms of developer) - at the discretion of the developer
  - Sale - The act of sale, consisting of the following required properties
    - Id - Identifier
    - Date – date of the sale
    - Time - the time of the sale
    - SalesPointId – point of sale identifier
    - BuyerId – buyer identifier (Can by null)
    - SalesData – a list of SaleData entities containing the following properties:
    - ProductId – identifier of the purchased product
    - ProductQuantity - the number of pieces of purchased products of this ProductId
    - ProductIdAmount - the total cost of the purchased quantity of goods of this ProductId
    - TotalAmount - the total amount of the entire purchase
2. Develop a web API with the implementation of CRUD operations with the database over all models described in paragraph 1
3. Added automatic filling of the database with test values at application startup if the current host environment name is Development.
4. Added Swagger.
5. Added Logger.
6. Added xUnit (concept).

# Migrations
Add migration:
> dotnet ef migrations add --project ..\SmartCarWashTest.Common.SeparateMigrations\SmartCarWashTest.Common.SeparateMigrations.csproj --startup-project SmartCarWashTest.WebApi.csproj --context SmartCarWashContext --configuration Debug SaleBuyerIdIsNullTrue --output-dir Migrations

Remove migration:
> dotnet ef migrations remove --project ..\SmartCarWashTest.Common.SeparateMigrations\SmartCarWashTest.Common.SeparateMigrations.csproj --startup-project SmartCarWashTest.WebApi.csproj --context SmartCarWashContext

# Build
> SmartCarWashTest.WebApi: SmartCarWashTest.WebApi

# Configurate
Specify a relative path in `appsettings.json` if necessary, or leave the default `".."`.
