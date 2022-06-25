

# Introduction
Template project conducted by me! This project developed using ASP.NET Core as Backend and Blazor WebAssembly as Frontend and SQLServer as database.

- Also the Bit framework has been used both in Backend and Frontend for easy and fast development of the project.
- CQRS and MediatR with Clean Architercture.
- Fluent Validation added.


<a href="https://github.com/bitfoundation/bitplatform">Bit Platform</a>


# Architecture
 It is Clean Architechture, the one more aligned withJson Tylor and Nick Chapsas.

### Domain:
 - Things related to the domain (a little like DDD). No logic comes here. 
 - This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.
 - All Entities Properties has private set! Because only with class APIs they can change.

### Application:
 - Orchestrate the application. This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers and DTOs. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure:
 - This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.
 - All classes be internal because it do not have any effect on other projects, only in ConfigureServices have and it has its own method.

### WebUI:
 - This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only Startup.cs should reference Infrastructure.
 - They depend to Infrastrucutre project only for Service registration and nothing else!

