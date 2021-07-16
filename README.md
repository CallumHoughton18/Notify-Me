# NotifyMe.App - An Open Source, Self-Hostable, Progressive Web App Notifications Solution

[![Build Status](https://dev.azure.com/callumhoughton13/callumhoughton13/_apis/build/status/CallumHoughton18.Notify-Me?branchName=master)](https://dev.azure.com/callumhoughton13/callumhoughton13/_build/latest?definitionId=8&branchName=master)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/callumhoughton13/callumhoughton13/8)
![Docker Image Size (tag)](https://img.shields.io/docker/image-size/callumhoughton22/notifymeserver/latest)

**Demo at: notifymeapp.callums-stuff.net**

<!-- vscode-markdown-toc -->
* 1. [Introduction](#Introduction)
* 2. [Self-Hosting](#Self-Hosting)
* 3. [Architecture](#Architecture)
  * 3.1. [Reusability](#Reusability)
    * 3.2. [Notification Scheduling](#NotificationScheduling)
    * 3.3. [Databases and Storage](#DatabasesandStorage)
    * 3.4. [Authentication and Authorization](#AuthenticationandAuthorization)
    * 3.5. [User Interface](#UserInterface)
    * 3.6. [TypeScript Integration](#TypeScriptIntegration)
* 4. [Creating and Updating Migrations](#CreatingandUpdatingMigrations)
* 5. [4. Known Issues](#KnownIssues)

<!-- vscode-markdown-toc-config
	numbering=true
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->

![App Image](imgs/example.png)

## 1. <a name='Introduction'></a>Introduction

NotifyMe.App is a blazor server app that is focused on providing a self-hostable open source solution that allows users to save and schedule notifications that can be sent to any device which supports web push notifications.

A demo of the application is hosted at [notifymeapp.callums-stuff.net](https://notifymeapp.callums-stuff.net). This demo is built on every commit to the master branch rather than on each stable release, so could be prone to bugs.

It also demonstrates how a more complicated progessive web application can be created using Blazor that interops with modern JavaScript (transpiled from TypeScript) web functionality that can't be done in pure C#.

The app is currently in active development, with the intention of integrating with the Pushy.App web API to support iOS devices.

You should be able to clone the repository and then within the notifyme.server project create a `appsettings.Development.json` file and include the following snippet (replacing any secrets):

```json
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "VAPID": {
    "subject": "{VAPIDSUBJECTSECRET}",
    "publicKey": "{VAPIDPUBLICKEY}",
    "privateKey": "{VAPIDPRIVATEKET}"
  },
  },
  "ConnectionStrings": {
    "NotifyMeDB": "{NOTIFYMEDBCONNECTIONSTRING}"
  },
  "Quartz": {
    "SQLiteDataSourcePath": "Data Stores/jobstore.db",
    "SQLiteDataSourceTemplatePath": "../notifyme.scheduler/DB Templates/jobstore-template.db",
    "quartz.scheduler.instanceName": "NotifyMeScheduler",
    "quartz.threadPool.maxConcurrency": "3",
    "quartz.serializer.type": "json",
    "quartz.jobStore.type": "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
    "quartz.jobStore.misfireThreshold": "60000",
    "quartz.jobStore.lockHandler.type": "Quartz.Impl.AdoJobStore.UpdateLockRowSemaphore, Quartz",
    "quartz.jobStore.useProperties": "true",
    "quartz.jobStore.dataSource": "jobstore",
    "quartz.jobStore.tablePrefix": "QRTZ_",
    "quartz.jobStore.driverDelegateType": "Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz",
    "quartz.dataSource.jobstore.connectionString": "Data Source=Data Stores/jobstore.db;Version=3;Foreign Keys=ON;",
    "quartz.dataSource.jobstore.provider": "SQLite"
  }
}
```

The app should be configured to use a SQLite DB for both the Quartz.NET job store and for the NotifyMe apps database. On running the app for the first time both databases will be created automatically and migrations will be applied.

Make sure that the "SQLiteDataSourcePath" configuration in the Quartz section of `appsettingss.Development.json` matches the connection string in "quartz.dataSource.jobstore.connectionString".

The NotifyMeDB will be seeded with with an admin user with the login:
UserName: admin@test.com
Email: admin@test.com
Password: Ch@ngeMe1!

You can change this, but as this is for development only it shouldn't matter.

You should be able to run the NotifyMe app, login, and start using it.

## 2. <a name='Self-Hosting'></a>Self-Hosting

The docker image is available on [Docker Hub](https://hub.docker.com/r/callumhoughton22/notifymeserver), and can be easily ran either from the command line or via docker-compose like:

```yaml
version: '3.4'

services:
  notifymeserver:
    user: appuser
    image: callumhoughton22/notifymeserver:latest
    networks:
        - reverse-proxy-network
    volumes:
      - notifymeappdata:/app/Data Stores

volumes:
  notifymeappdata:

networks:
    reverse-proxy-network:
        external: true

```

Which shows how the application can be ran and exposed via the reverse proxy network, the container interally runs the app on port 5000 so the reverse proxy will have to be setup to account for this.

## 3. <a name='Architecture'></a>Architecture

The app has been architectured attempting to follow the best practices outlined for ASP.NET and Blazor applications, and mirrors [how other sample applications have been setup](https://github.com/dotnet-architecture/eShopOnWeb).

The app uses the MVVM design pattern along with the built in dependency injection provided by ASP.NET and so shouldn't be too difficult to follow.

### 3.1. <a name='Reusability'></a>Reusability

A lot of the code for this project is within the notifyme.shared project which is a .NET Standard project, and so can be reused. The ViewModel code and service interfaces are all defined here.

### 3.2. <a name='NotificationScheduling'></a>Notification Scheduling

The app uses Quartz.NET to schedule jobs that when ran will push saved notifications to all the users registered browsers any their devices, the notifyme.scheduler project demonstrates how this is done, and is sandboxed into using its own database.

### 3.3. <a name='DatabasesandStorage'></a>Databases and Storage

The app uses Entity Framework Core for data storage, and for development is configured using SQLite. Realistically as the userbase for a self hosted app should be small, the production application could also be configured using SQLite.

### 3.4. <a name='AuthenticationandAuthorization'></a>Authentication and Authorization

The app uses ASP.NET Core Identity for user management as this integrates well with Blazor. The identity database is set to be the same as the current development database (`NotifyMeDB.db`) to keep it simple.

### 3.5. <a name='UserInterface'></a>User Interface

The UI is designed using the [MudBlazor library](https://mudblazor.com/). Common UI elements should be broken down into components, and are stored within the Shared directory in the notifyme.server project.

### 3.6. <a name='TypeScriptIntegration'></a>TypeScript Integration

The application is setup to use TypeScript, which on each build converts and deploys any TypeScript code in the `notifyme.server/Scripts` directory into `notifyme.server/wwwroot/js`. The relevant JavaScript functions can then be called using JSInterop in Blazor.

The TypeScript required for the project should be kept to a minimum and should be organized into modules, like how it is done within `PushNotification.ts`.

## 4. <a name='CreatingandUpdatingMigrations'></a>Creating and Updating Migrations

from the notifyme.server directory run:

`dotnet ef migrations add MIGRATIONAME --context notifymecontext -p ../notifyme.infrastructure/notifyme.infrastructure.csproj -s notifyme.server.csproj -o Data/Migrations`

`dotnet ef database update --context notifymecontext -p ../notifyme.infrastructure/notifyme.infrastructure.csproj -s notifyme.server.csproj`

## 5. <a name='KnownIssues'></a>4. Known Issues

Currently, PWA Push Notifications are not available on iOS, and so this application will not work on iOS devices. There are also [known web push notification issues on FireFox for Android](https://github.com/mozilla-mobile/fenix/issues/19152). So it is advised to use Chrome for Android for the time being to setup push notifications. Incredibly annoying I know.
