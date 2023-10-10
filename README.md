## Microservices APP .NET

### Run the App

Set your mssql passwords in the `appsettings.Production.json` files.

Install Docker and K8S. Run commands inside the K8S folder:

- `kubectl apply -f plarforms-deploy.yaml`
- `kubectl apply -f plarforms-np-srv.yaml`
- `kubectl apply -f commands-deploy.yaml`
- `kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/aws/deploy.yaml`
- `kubectl apply -f ingress-srv.yaml`
- `kubectl apply -f local-pvc.yaml`
- `kubectl create secret generic mssql --from-literal=SA_PASSWORD="<your_mssql_password>"`
- `kubectl apply -f mssql-plat-deploy.yaml`
