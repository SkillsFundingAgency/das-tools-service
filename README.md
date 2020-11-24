# das-tools-service


## Infrastructure requirements
* App Gateway
* App Service
* App Service plan
* Storage Account
* Auth0 Tenant

## Auth0 SetUp

If the application isn't already registered in Auth0 register the application.  Once the application is registered you'll need to update the following properties:
* Allowed Callback URLs: https://<hostname>/callback
* Allowed Logout URLs: https://<hostname>
