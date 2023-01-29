
### Continue working on project using Gitpod

Click the button below to start a new development environment:

[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io/#https://github.com/elhuk/clean-architecture-proj)

# Clean Architecture Sample Project

The purpose of this project is to build a reference architecture demonstrating how to implement the Clean Architecture Structure when working with .Net solutions.

Clean Architecture is a layered architecture that splits into four layers:

Domain
Application
Infrastructure
Presentation

Here's a visual representation of the Clean Architecture:

{// add clean architecture email here}


## Domain Layer
The Domain layer sits at the core of the Clean Architecture. Here we define things like: entities, value objects, aggregates, domain events, exceptions, repository interfaces, etc.

Here is the folder structure I like to use:

📁 Domain
|__ 📁 DomainEvents
|__ 📁 Entities
|__ 📁 Exceptions
|__ 📁 Repositories
|__ 📁 Shared
|__ 📁 ValueObjects

You can introduce more things here if you think it's required.

One thing to note is that the Domain layer is not allowed to reference other projects in your solution.

## Application Layer
The Application layer sits right above the Domain layer. It acts as an orchestrator for the Domain layer, containing the most important use cases in your application.

You can structure your use cases using services or using commands and queries.

I'm a big fan of the CQRS pattern, so I like to use the command and query approach.

Here is the folder structure I like to use:

📁 Application
|__ 📁 Abstractions
    |__ 📁 Data
    |__ 📁 Email
    |__ 📁 Messaging
|__ 📁 Behaviors
|__ 📁 Contracts
|__ 📁 Entity1
    |__ 📁 Commands
    |__ 📁 Events
    |__ 📁 Queries
|__ 📁 Entity2
    |__ 📁 Commands
    |__ 📁 Events
    |__ 📁 Queries
    
In the Abstractions folder, I define the interfaces required for the Application layer. The implementations for these interfaces are in one of the upper layers.

For every entity in the Domain layer, I create one folder with the commands, queries, and events definitions.


