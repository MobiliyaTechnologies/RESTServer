# Energy Management REST API Server


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Introduction

```
It contains following projects
1. RestService - It contain back end API's. For API's help page redirect to http://<Hosted URL>/Help.
2. EnergyManagementScheduler.WebJob - It contain scheduler to run periodic jobs for - Anomaly detection, Consumption Alert, Daily and Monthly Consumption, Daily and weekly prediction on half hourly power scout data.

```

### Prerequisites

What things you need to install the software and how to install them

```
1.	Active Azure subscription.
2.	Microsoft SQL Server.
3.	Email host to send emails.	
4.	Google firebase used for notification.						
5.	B2C Application with sign-up, sign-in and change password policy.
6.	Azure storage.
7. 	Add value for following configurations,
    a.	RestService Configuration - 	
        ⦁	b2c:Tenant - B2C application id.
        ⦁	b2c:ClientId - Valid client id, API only accepts tokens from its own clients.
        ⦁	b2c:SignUpPolicyId - B2c sign-in policy.
        ⦁	b2c:SignInPolicyId - B2c sign-up policy.
        ⦁	b2c:ChangePasswordPolicy - B2c change password policy.
        ⦁	b2c:ClientSecret - B2c app key.
        ⦁	EmailHost
        ⦁	EmailHostPassword
        ⦁	EmailHostPort
        ⦁	EmailSender
        ⦁	BlobStorageConnectionString - azure storage connection string.
        ⦁	NotificationURL - google firebase url.
        ⦁	NotificationClickAction - google firebase click action.
        ⦁	PowerGridEntities - Entity framework generated sql server connection string, change data source, user id and password. 
	b. EnergyManagementScheduler.WebJob configuration
        ⦁	DbConnectionString - sql server connection string.
        ⦁	NotificationURL - google firebase url.
        ⦁	NotificationClickAction - google firebase click action.
        ⦁	MeterKwhCost - default 0.04.
        ⦁	AnomalyThreshold - default 0.6.
        ⦁	AzureWebJobsDashboard - azure storage connection string.
        ⦁	AzureWebJobsStorage- azure storage connection string.	
```

### Installing

A step by step series of examples that tell you have to get a development env running


```
1. Clone or download source code.
2. Open code in microsoft visual studio and rebuild it.
3. Create and publish database.
4. Host Api on IIS.
5. Run EnergyManagementScheduler.WebJob project.

```
