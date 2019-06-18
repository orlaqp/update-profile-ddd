## Demo Structure

The differents solutions in this repo work together to solve a single requirement, updating the email address
of a customer. It is not that simple though, this email address has to be updated in two different data stores that belong
to two completely different systems. In addition, the customer need to be notify of such change and both the notification and
the update of the email address should be tried up to three times in case of failures.

## How is our code organized?

- **ui**: This is a small project built with Angular. It has just a single page that allow a user to enter a new email address
that's it.

- **patron**: This project was built using .Net Core and it is basically the entry point to our backend functionality of changing
the email address of our customer.

- **cmp**: The secondary API that will take care of updating the email address in that secondary store.

- **notification**: A backend system which only responsiblity is to send notifications out.

## Solution architecture

![Architecture](/assets/architecture-diagram.png)

## Patron service layers architecture

![Patron Service](/assets/patron-service.png)

## Third Party Tools

- RabbitMQ
- SQL Server
- Docker
- Redis

## Topics/Patterns we are going to discuss

- SOLID Principles
- Domain Driven Design
- Command Query Responsibility Segregation
- Repository Pattern
- Unit of Work
- .NET Code
- Dependency Injection
- Reflection
- Entity Framewrok Core

## How to follow the code in this repo ?

### Patron Solution (src/patron)

As you can see, this solution is composed of multiple projects and if you are not familiar with some of the patterns used here it may be a little hard to follow the code. So lets start all the way at the top and then I will explain the function of every layer and how to use it.


### API Layer

As with any other API you may have seen before, this layer allow your users to interact with your service. This is a very small representation of what a REST API would be but in essence is composed of Controller and Actions with the corresponding HTTP verbs.

Now, the way the actions are implemented is what may be a little different to what you are used to see in other APIs. At first you may think that the way the code was written is a little bit overkill and that may hold true for very simple applications but the idea behind this architecture was not design for such applications. Instead, you can think of this repo as a enterprise skeleton architecture with a very simple example of a way to execute a command.

Enough for introductions already, let's start analysing some code:

```c#
namespace API.Controllers
{
    [Route("email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CommandBus commandBus;

        public EmailController(IUnitOfWork unitOfWork, CommandBus commandBus)
        {
            this.unitOfWork = unitOfWork;
            this.commandBus = commandBus;
        }

        /// <summary>
        /// Updates patron's email address
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CommandResult>> Put(Guid id, [FromBody] string email)
        {
            var command = new UpdateEmailCommand(id, email);
            await commandBus.Run(command);
            await unitOfWork.Commit();

            return Ok(command.Result);
        }

    }
}
```

Pretty simple and straight forward right? We have a simple controller with an action that according to its documentation is supposed to update the email address for a Patron/Customer. Also notice how the constructor of the controller receives two parameters, a `UnitOfWork` and a `CommandBus`. These two services are key and in order to explain how they are used lets concentrate ourselves on the Put's method:

```c#
public async Task<ActionResult<CommandResult>> Put(Guid id, [FromBody] string email)
{
    var command = new UpdateEmailCommand(id, email);
    await commandBus.Run(command);
    await unitOfWork.Commit();

    return Ok(command.Result);
}
```

We receive the customer's id and email and create a command with that:

```c#
var command = new UpdateEmailCommand(id, email);
```

This idea is coming from a Pattern called Command and Query Responsibility Segregation (CQRS). When following this pattern every `action` that somehow needs to change the data should enclosed in a `Command`. Commands are pretty simple classes that contain the data the command needs to do its job. In this case, we need the Patron/Customer Id and the new email address.

Then we have this:

```c#
await commandBus.Run(command);
```

The commands bus is coming from the `core` project and it has the responsibility of finding the right command handler(s) `registered` for this command and execute them. I will expand on this topic later but for now let's say that the "wiring" is done using `Dependency Injection`. Also notice how this code using `Async` programming.

After running the command we have the following:

```c#
await unitOfWork.Commit();
```

What is this and why do we need it? The idea behind the `Unit of Work` pattern is to group one or more operations (usually db operations) into a `single` transaction or `unit of work` so all our operations either pass or fail. So basically what we do when running a command is make changes in memory but nothing get persisted to our storage until we `commit` our changes. This is a key feature of this architecture.

Then in the last line we just give our customers the result of our command. Does it make sense?

**Swagger**

If you are not familiar with `Swagger` it basically allow you to describe the structure of your APIs so machines can read them. The ability of APIs to describe their own structure is the root of all awesomeness in Swagger. Why is it so great? Well, by reading your API’s structure, we can automatically build beautiful and interactive API documentation. We can also automatically generate client libraries for your API in many languages and explore other possibilities like automated testing. Swagger does this by asking your API to return a YAML or JSON that contains a detailed description of your entire API.

(extracted from: https://swagger.io/docs/specification/2-0/what-is-swagger/)

In our case we describe our API while developing which is even better by using this package: `Swashbuckle.AspNetCore` which basically ready our C# code and create the Swagger schema for us. If you run this app and navigates to: https://localhost:5001/swagger/index.html you will get the Swagger UI.


### Commands Layer

You can think of this layer as a description of what you code can do from a business perspective, remember that I mentioned above that we were going to be using Domain Driven Design (DDD) so the classes in this layer should be a direct result of you conversion(s) with you domain expert(s). This file structure should be readable by developers and business people because just by looking at the file names should be enough to tell you what the commands is all about. Some people also use this layer as documentation also.

Now, let's see how we define a command:


**UpdateEmailCommand.cs**

```c#
namespace Commands.Email
{
    public class UpdateEmailCommand : UpdateCommand<Guid>
    {
        public UpdateEmailCommand(Guid id, string email) : base(id)
        {
            Email = email;
        }
        
        public string Email { get; }
    }
}
```

As you can see is a simple class that basically contain the information needed to execute the command. In addition, we are inheriting from UpdateCommand which is shortcut because most update operations need an Id so instead of repeating the Id property, it is encapsulated in the `UpdateCommand` class. Also, notice that the `Email` property es readonly ideally this command should not change their input values once the are created, only the `Result` which is included in the base class for all commands which surprise surprise is called `Command` class :-)

As I mentioned before, this is layer is pretty simple and it serve also as documentation of our business actions (commands).

### Command Handlers Layer

When presenting the API layer we briefly talked about how the `Command Bus` somehow connected a command with its handler(s) and execute it. Now let's take a deep dive into this section and explain exactly how it happens.

In order to that I think we should start by looking at the code of a handler:

```c#
namespace CommandHandlers.Email
{
    public class UpdateEmailCommandHandler : CommandHandler<UpdateEmailCommand>
    {
        private readonly IPatronRepository patrons;

        public UpdateEmailCommandHandler(
            ILogger logger,
            IPatronRepository patrons
        ) : base(logger)
        {
            this.patrons = patrons;
        }

        public async override Task Run(UpdateEmailCommand command)
        {
            var email = new EmailAddress(command.Email);
            var patron = await patrons.GetById(command.Id);

            if (patron == null) {
                throw new BusinessException("PatronNotFound", $"Patron {command.Id} not found");
            }

            patron.UpdateEmail(email);
        }
    }
}
```

Starting at the class definition we have the following:

```c#
public class UpdateEmailCommandHandler : CommandHandler<UpdateEmailCommand>
```

This line is KEY and is used to connect commands with their handler(s). It can be one or more even though in real world applications you rarely associate a single command to multiple handlers. First, notice how this class inherit from `CommandHandler`, this is NOT optional, you have to inherit from this class and indicate which command you handling passing it as a generic argument to the CommandHandler class.

When the application starts we dynamically create the associations between cammands and their handlers with the following code:

```c#
private static void RegisterCommandHandlers(IServiceCollection services)
{
    // Make sure we are referencing commands and command handlers so reflection can pick up the types
    var handlerType = typeof(UpdateEmailCommandHandler);
    var commandHandlers = ReflectionHelpers.GetSubClassesOf(typeof(CommandHandler<>));
    
    foreach (var commandHandlerType in commandHandlers)
    {
        var parentType = commandHandlerType.BaseType;
        var commandType = parentType
            .GetGenericArguments()
            .Where(t => t.IsSubclassOf(typeof(Command)))
            .FirstOrDefault();

        if (commandType == null) {
            throw new InvalidOperationException("Command type not found");
        }
        
        services.AddScoped(commandType, commandHandlerType);
    }
}
```

Basically what this code does is to look for all types that inherit from `CommandHandler<>` and have a generic argument that is subclass of the `Command` class. This code allow the `CommandBus` later on to retrieve a handler based on a Command all this is accomplish with the help of `Reflection` (https://docs.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/reflection) and .NET Core dependency injection (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2)

So what really does the run method of the command bus class?? Here it is (check the code comments):

```c#
public async Task Run(Command command)
{
    try
    {
        // Ask the dependency injection container for a handler
        // of the command that passed as a parameter
        var commandType = command.GetType();
        var handler = services.GetService(commandType);

        // The handler should never be null so if that happens
        // we should stop here
        if (handler == null) {
            logger.Error("Handler not found for: " + commandType.FullName);
        }

        // Get a reference to the Run method of the Command Handler
        var runMethod = handler.GetType().GetMethod("Run");
        // Invoke the Run method passing it the command
        Task result = (Task)runMethod.Invoke(handler, new object[] { command });    
        // Wait for it to complete
        await result;
    }
    catch (BusinessException ex) {
        // In case of a Business exception set the
        // command result with the exception's code and message
        command.Result.Error(ex.Code, ex.Message);
    }
    catch (Exception ex)
    {
        // In case of a validation exception we also set the response accordingly
        if (ex.InnerException != null && ex.InnerException.GetType() == typeof(ValidationException)) {
            command.Result.ValidationErrors(ex.InnerException as ValidationException);
        } else {
            logger.Error(ex, $"Error executing command {command.GetType().FullName}");
            command.Result.UnexpectedError();
        }
    }
}
```

This is very simple implementation of the `CommandBus` but this class is in a very strategic place for the application and it can be used for other concerns such as auth, logging, etc. A key part of this implementation is that it uses the DI container to create the handler's instances which allow you to use dependency injection in your handler's constructors.

Let check our handler's constructor:

```c#
public UpdateEmailCommandHandler(
    ILogger logger,
    IPatronRepository patrons) : base(logger)
{
    this.patrons = patrons;
}
```

`ILogger` and `IPatronRepository` are both instantiated and provided to our handler "magically" :-).


What about the implementation of the run method??

```c#
public async override Task Run(UpdateEmailCommand command)
{
    var email = new EmailAddress(command.Email);
    var patron = await patrons.GetById(command.Id);

    if (patron == null) {
        throw new BusinessException("PatronNotFound", $"Patron {command.Id} not found");
    }

    patron.UpdateEmail(email);
}
```

This small piece of code reaches to more than one application layer. On one side, it reaches the domain layer by using the `EmailAddress` value object and the `Patron` entity. On the other hand, by using the `IPatronRepository` through the patrons field, it reaches the `Infrastructure` and `Infrastructure.SQLServer` layers. The `Domain` layers is where we have our business logic and the infrastructure layers help us saving our data so some kind of storage, in this case SQL Server.

### Domain Layer

From a business point of view this is the most important layer of you entire solution. This layer will mainly contain three object types.

- Entities
- Value Objects
- Services

**Note:** If you are no familiar with DDD it would be nice to do some online reading to get some context at least. This would help you a lot understanding this layer.

Let's check our `EmailAddress` value object:

```c#
namespace Domain.Patron.ValueObjects
{
    public class EmailAddress
    {
        // This constructor is used by Entity Framework
        protected internal EmailAddress() { }

        Validator validate = new Validator();

        public EmailAddress(string email)
        {
            validate.IsEmail(nameof(Email), email);
            validate.ThrowValidationExceptionIfInvalid();

            Email = email;
        }

        public string Email { get; private set; }
    }
}
```

A value object is an immutable type that is distinguishable only by the state of its properties. They do not have a uniquely identify like entities do. From a technical point of view there is nothing crazy about this class, it is simple and based on its accessors you can see that it is "immutable" (you can mutate it if you use Reflection).

Our Patron entity on the other hand is defined like this:

```c#
namespace Domain.Patron
{
    public class Patron : AggregateRoot
    {
        protected internal Patron() {}
        private Validator validate = new Validator();

        internal Patron(Guid id, INewPatronData data) : base(id) 
        {
            validate.IsNotNullOrWhiteSpace(data.FirstName);
            validate.IsNotNull(data.Email);

            validate.ThrowValidationExceptionIfInvalid();

            FirstName = data.FirstName;
            LastName = data.LastName;
            EmailAddress = data.Email;
        }

        public string FirstName { get; private set;}
        public string LastName { get; private set; }
        public EmailAddress EmailAddress { get; private set; }

        public void UpdateEmail(EmailAddress email)
        {
            if (email == null) {
                throw new InvalidOperationException("Email cannot be null");
            }

            EmailAddress = email;
            ResetPreferences();

            var emailUpdatedEvent = new PatronEmailUpdatedDomainEvent(
                this.Id,
                this.EmailAddress.Email);
            QueueDomainEvent(emailUpdatedEvent);
        }

        private void ResetPreferences() {
            // reset patron's preferences
        }
    }
}
```

As you can see, this class is inheriting `AggregateRoot` that name is not arbitrary. It is in fact one of DDD "artifacts" and it basically means that it is the entry point to an aggregate. So this class it is not just an `Entity` it is a special type called `Aggregate Root`. An important detail to take into consideration here is that this class does not has any persistence notion. It only deal with business logic in memory which separate your business logic from the "details" of how this information is going to persisted or retrieved.

An extra piece of information in this class that is great to achieve decoupling is the following:

```c#
var emailUpdatedEvent = new PatronEmailUpdatedDomainEvent(
    this.Id,
    this.EmailAddress.Email);
QueueDomainEvent(emailUpdatedEvent);
```

This event mechanism is a very flexible way to notify other parts of the application or even external systems about specific conditions in your business and allow the interested parts to react to them in a decoupled way which also goes hand in hands with the Single Resposibility Principle (SRP) and the Open-Closed Principle. 

### Infrastructure Layer
### Infrastructure SQL Server Layer

