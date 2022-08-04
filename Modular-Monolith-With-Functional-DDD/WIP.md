# Work in Progress

Progress of strategic vs tactical DDD patterns implementation. There quite a difference between the OOP and the F# implementation.

- Business rules:
  - Impossible entity states enforced by types and valdiation
  - Add BDD tests to make explicit the rules required by the users. Check: [Open Source .NET Behavior-Driven Development (BDD) Tools](https://www.softwaretestingmagazine.com/tools/open-source-net-behavior-driven-development-bdd-tools/)

## Backlog

- Layers: how to check no layer call violation
  - Naming namespace?
- Simple Constrained Type: have at least a value and create
- Common or BuildingBlocks?
  How to qualify? only interfaces and types?
- Simple vs Compound Types
- Internal vs Public Types = in different modules?
- Build scripts
- Test scripts
- Result type

## Architecture Tests

### Domain

- Domain event should be immutable
- Value object should be immutable
- Entity which is not aggregate root cannot have public members
- Entity cannot have reference to other aggregate roots
- Entity should have parameterless private constructor
- Domain object should have only private constructors
- Value object should have private constructor with parameters for his state
- Domain event should have DomainEvent postfix
- Business rule should have BusinessRule postfix

### Module

- Domain layer does not have dependency to application layer
- Domain layer does not have dependency to infrastructure layer
- Application layer does not have dependency to infrastructure layer
+ No dependency between the modules

### Application

- Command should be immutable
- Query should be immutable
- CommandHandler should have name ending with CommandHandler
- QueryHandler should have name ending with QueryHandler