# Microservices with **.NET** and **Kubernetes**

This is a simple project made as an application to Les Jacksons's [awesome course on Youtube](https://www.youtube.com/watch?v=DgVjEo3OGBI), allowing me (and you) to tweak and play with **K8s** alongside a couple of already built services with **.NET 5**.

The system's main mission is to model a data store of a list of commands with their corresponding platform, for example, `docker-compose up` is a Docker command (Docker being the **platform**) that helps you spin up multiple containers all at once. Once the system has enough data, it would act as a repository of **command-line arguments** for a given Platform.

Having the nature of Microservices, the project's components **are decoupled** from one another, meaning that the whole Platforms Service could go down with the Commands Service and its users intact.

## Technology
As I mentioned before, the project is built using **.NET 5** for each of the two services, each one of them contacts a **SQL Server** when running in a production environment while using a simple In-Memory database when running in Development Environment.

These services are deployed to a **Kubernetes Cluster**, each in its container, each container is hosted in its own Pod.
The two services could communicate with each other using an internal **`ClusterIP` Service**, while being accessible from the external world using an **Ingress (Nginx) with a Controller**.

Whenever a new Platform is added to the Platforms Service using a `RESTful POST` request, the services notifies other services (Commands Service) of the new addition through a **RabbitMQ Message Bus**.
Commands Service subscribes to that bus and adds any new Platform to its data store as well.

Initially, once Commands Service is deployed, it does not know about previously existed Platforms, so a data seed is performed using a **gRPC** call to the Platforms Service to fetch all the missed Platforms, allowing the end-user to add a Command to the newly fetched Platform right away.

## Architecture Overview
Here is an overview of the solution architecture illustrating how the Commands Service can communicate with the Platforms Service within the cluster.

![This illustration is courtesy of Les Jackson.](https://raw.githubusercontent.com/bfahm/MicroservicesWithDotNet/master/StudyNotes/Illustrated_Notes/Microservices%20Course/NET%20Microservices%20%E2%80%93%20Full%20Course%2000-42-49%20.png)

Also, here is an overview of the Kubernetes Cluster and how each component reaches the other. 

![This illustration is courtesy of Les Jackson.](https://raw.githubusercontent.com/bfahm/MicroservicesWithDotNet/master/StudyNotes/Illustrated_Notes/Microservices%20Course/NET%20Microservices%20%E2%80%93%20Full%20Course%2002-58-13%20.png)

*These illustrations are courtesy of Les Jackson.*

## Installation Guide
Each container's Docker Image is already built and available on my own [Docker Hub Repository](https://hub.docker.com/u/bolafahmi), technically, you can pull each one separately and inspect it using

```docker
docker run -d bolafahmi/platform_service
docker run -d bolafahmi/command_service
```
But this wouldn't work right away because each service is already configured to communicate with the other using a Cluster IP hostname (including SQL Server, RabbitMQ, and gRPC communications).
So you'll have to reconfigure each service's `appsettings.Production.json` file.

### Requirements
- .NET 5 SDK and Runtime (Only if you'll want to run outside Docker or Kubernetes)
- Docker Desktop with a Kubernetes cluster configured.
- `Kubectl`
- Docker.io account (Only if you are planning to edit any of the services, you'll then have to push to your own Docker Repository)

### Installation Steps
Make sure you are inside K8s directory and then run these commands:

```cmd
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.1/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f mssql-secret.yaml

kubectl apply -f local-pvc.yaml

kubectl apply -f mssql-platform-deployment.yaml
kubectl apply -f rabbitmq_deployment.yaml

kubectl apply -f platform_deployment.yaml
kubectl apply -f command_deployment.yaml
kubectl apply -f platform_np_srv.yaml

kubectl apply -f ingress.yaml
```

## Attribution
This project wouldn't have been completed without the help of Les Jacksons's [awesome course on Youtube](https://www.youtube.com/watch?v=DgVjEo3OGBI).
At first, I struggled a little bit with some Kubernetes Terminologies. I found great assistance on this [Kubernetes Course](https://www.youtube.com/watch?v=X48VuDVv0do) by Nana.
Some of the notes I took are screenshots from these courses, for quick future references and for personal usage only. 