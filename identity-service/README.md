# :key: Identity Service
This service is the identity provider for the other services in this system. Whenever the client application needs sign in it shows the login page and after successfull login, it redirects the request to the client as well. 

Beside, it validates the JWT token that is used accross the system.


## :hammer: Technology Used
1. .Net Core 8
1. [Identity Server 4](https://identityserver4.readthedocs.io/en/latest/)
1. MongoDB
1. 


## Current Implementation
### [Client](https://identityserver4.readthedocs.io/en/latest/intro/terminology.html#client)
* Web Client - In this case the web application
### [API Resources](https://identityserver4.readthedocs.io/en/latest/intro/terminology.html#resources)
* Video Service
* Storage Service
* Interaction Service
