## Microservices APP .NET

### Run the App

Add `127.0.0.1 acme.com` to the `Windows\System32\drivers\etc\hosts` file.

Install Docker and K8S. Run commands inside the K8S folder:

- `kubectl apply -f plarforms-deploy.yaml`
- `kubectl apply -f plarforms-np-srv.yaml`
- `kubectl apply -f commands-deploy.yaml`
- `kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/aws/deploy.yaml`
- `kubectl apply -f ingress-srv.yaml`
- `kubectl apply -f local-pvc.yaml`
- `kubectl create secret generic mssql --from-literal=SA_PASSWORD="12345678D!"`
- `kubectl apply -f mssql-plat-deploy.yaml`
- `kubectl apply -f rabbitmq-deploy.yaml`
