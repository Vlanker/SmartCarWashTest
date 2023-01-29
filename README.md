# SmartCarWashTest

This is test for Smart car wash company.

## SmartCarWashTest.CRUD.WebApi

1. Develop data models of the main entities:

    * Product – product model, consisting of the following mandatory properties:
        * Id - Identifier
        * Name - Name
        * Price - cost
        * Extending the model with other properties (necessary in terms of developer) - at the discretion of the
          developer
        * SalesPoint - point of sale of goods, consisting of the following mandatory properties:
            * Id - Identifier
            * Name - Name
            * ProvidedProducts – list of entities (ProvidedProduct) available for sale items, with the current available
              quantity for each item. Essence ProvidedProduct has the following required properties:
            * ProductId - product identifier
            * ProductQuantity - quantity
            * Extending the model with other properties (necessary in terms of developer) - at the discretion of the
              developer
    * Buyer - a buyer, a person who purchases a product or service in one of the points of sale
        * Id - Identifier
        * Name - Name
        * SalesIds - a collection of all purchase IDs ever carried out by this buyer
        * Extending the model with other properties (necessary in terms of developer) - at the discretion of the
          developer
    * Sale - The act of sale, consisting of the following required properties
        * Id - Identifier
        * Date – date of the sale
        * Time - the time of the sale
        * SalesPointId – point of sale identifier
        * BuyerId – buyer identifier (Can by null)
        * SalesData – a list of SaleData entities containing the following properties:
        * ProductId – identifier of the purchased product
        * ProductQuantity - the number of pieces of purchased products of this ProductId
        * ProductIdAmount - the total cost of the purchased quantity of goods of this ProductId
        * TotalAmount - the total amount of the entire purchase

2. Develop a web API with the implementation of CRUD operations with the database over all models described in paragraph
3. Added automatic filling of the database with test values at application startup if the current host environment name
   is Development.
4. Added Swagger.
5. Added Logger.
6. Added xUnit (concept).

### Migrations

Add migration:

```shell
dotnet ef migrations add --project ..\SmartCarWashTest.Common.SeparateMigrations\SmartCarWashTest.Common.SeparateMigrations.csproj --startup-project SmartCarWashTest.CRUD.WebApi.csproj --context SmartCarWashContext --configuration Debug SaleBuyerIdIsNullTrue --output-dir Migrations
```

Remove migration:

```shell
dotnet ef migrations remove --project ..\SmartCarWashTest.Common.SeparateMigrations\SmartCarWashTest.Common.SeparateMigrations.csproj --startup-project SmartCarWashTest.CRUD.WebApi.csproj --context SmartCarWashContext
```

### Building

Go to `.\SmartCarWashTest.CRUD.WebApi`:

```shell
cd .\SmartCarWashTest.CRUD.WebApi\
```

#### Windows

##### Build app:

```shell
dotnet build
```

#### Docker

Open `crud-web-api-depl.yaml` section `image:` and edit 17 line `you_docker_hub_name`:

```yaml
containers:
  - name: webapi
    image: you_docker_hub_name/crudwebapi:latest
```

##### Build:

```shell
docker build -t <you_docker_hub_id>/crudwebapi .
```

##### Run:

```shell
docker run -p 8080:80 -d <you_docker_hub_id>/crudwebapi
```

##### Check:

```shell
docker ps
```

##### Stop:

```shell
docker stop <container_id>
```

##### Restart

```shell
docker start <container_id>
```

##### Push:

```shell
docker push <you_docker_hub_id>/crudwebapi
```

#### Kubernetes

Go to `\deploy\K8S`:

```shell
cd .\deploy\K8S\
```

##### Apply:

```shell
kubectl apply -f crud-web-api-depl.yaml
```

##### Deployments:

```shell
kubectl get deployments
```

##### Pods:

```shell
kubectl get pods
```

##### Delete:

```shell
kubectl delete deployment crud-web-api-depl
```

##### Apply service:

```shell
kubectl apply -f crud-web-api-np-srv.yaml
```

##### Service:

```shell
kubectl get services
```

##### Persistent Volume Clime:

```shell
kubectl apply -f local-pvc.yaml
```

### *Adding a Kubernetes Secret

```shell
kubectl create secret generic mssql --from-literal-SA_PASSWORD="pa55w0rd!"
```

#### Configurate

Specify a relative path in `appsettings.Production.json` if necessary, or leave the default `".."`.
If needed edit `EventBusSettings`.
___

## SmartCarWashTest.Sale.WebApi

Develop a web API with the implementation of the following business logic:
**Sale** - the sale of a product or service. The goods can be purchased at any point of sale, subject to the
availability of
the required quantity of goods. The purchase of goods is available both for authorized users (having an Id) and for
unauthorized users.

1. When carrying out the operation of selling goods for authorized users, the corresponding records are made in the
   database:
2. Changes the number of available products at the point of sale, according to the number of products soldII. An
   instance
   of the Sale entity is formed, and written to the database
3. The identifier of the entity (formed in part 2.) Sale is written to the collection of purchases in the table of the
   buyer

* For unauthorized users, the actions are similar, with the exception of part 3. - in this case, no records are made
  in the buyer's tables, and the BuyerId property in the Sale entity is null.

### Building

Go to `.\SmartCarWashTest.Sale.WebApi`:

```shell
cd .\SmartCarWashTest.Sale.WebApi\
```

#### Windows

##### Build app:

```shell
dotnet build
```

#### Docker

Open `crud-web-api-depl.yaml` section `image:` and edit 17 line `you_docker_hub_name`:

```yaml
containers:
  - name: webapi
    image: you_docker_hub_name/crudwebapi:latest
```

##### Build:

```shell
docker build -t <you_docker_hub_id>/crudwebapi .
```

##### Run:

```shell
docker run -p 8080:80 -d <you_docker_hub_id>/crudwebapi
```

##### Check:

```shell
docker ps
```

##### Stop:

```shell
docker stop <container_id>
```

##### Restart

```shell
docker start <container_id>
```

##### Push:

```shell
docker push <you_docker_hub_id>/crudwebapi
```

#### Kubernetes

Go to `\deploy\K8S`:

```shell
cd .\deploy\K8S\
```

##### Apply:

```shell
kubectl apply -f crud-web-api-depl.yaml
```

##### Deployments:

```shell
kubectl get deployments
```

##### Pods:

```shell
kubectl get pods
```

##### Delete:

```shell
kubectl delete deployment crud-web-api-depl
```

##### Apply service:

```shell
kubectl apply -f crud-web-api-np-srv.yaml
```

#### Configurate

If needed edit `EventBusSettings`.

___

## Adding an API Getaway (Ingress Nginx)

Go to `\deploy\K8S`:

```shell
cd .\deploy\K8S\
```

### Apply:

[NGINX Ingress Controller - Installation Guide](https://kubernetes.github.io/ingress-nginx/deploy/)

Open `ingress-srv.yaml` section 10 line `host:` and edit `<you_host_name>`:

```yaml
spec:
  rules:
    - host: <you_host_name>
```

### Apply service:

```shell
kubectl apply -f ingress-srv.yaml
```

___

## Adding an RabbitMQ

Go to `\deploy\K8S`:

```shell
cd .\deploy\K8S\
```

[See RabbitMQ downloads](https://www.rabbitmq.com/download.html)

### Apply service:

```shell
kubectl apply -f rabbitmq-depl.yaml
```
