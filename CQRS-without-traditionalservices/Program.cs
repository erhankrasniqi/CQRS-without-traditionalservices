

// Recommended approach in CQRS/DDD:
// Create an interface in the application layer.

 public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body);
}
// This interface belongs to the application layer because it represents an operation used by commands.
// The implementation resides in the Infrastructure layer.

public class PostmarkEmailSender : IEmailSender
{
    private readonly PostmarkClient _client;

    public PostmarkEmailSender(IConfiguration config)
    {
        _client = new PostmarkClient(config["Postmark:ApiKey"]);
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        await _client.SendMessageAsync("no-reply@yourapp.com", to, subject, body);
    }
}

// Injection in the DI container:


services.AddScoped<IEmailSender, PostmarkEmailSender>();

// Invocation within Command Handlers where needed.
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IEmailSender _emailSender;

    public RegisterUserCommandHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // user registration...

        await _emailSender.SendEmailAsync(request.Email, "Welcome", "Thank you!");
        return Result.Success();
    }
}

/*
 * 
Why This Approach Aligns with CQRS and DDD Principles

Separation of Concerns: 
There's no need for a shared EmailService containing mixed responsibilities. Each component has a clearly defined role.

Single Responsibility Principle (SRP): 
The interface adheres to SRP by encapsulating only the functionality required for a specific use case.

Technical Service Layer: 
The implementation resides in the infrastructure layer, indicating it is a purely technical concern and not part of the domain or business logic.

Decoupled Command Handlers: 
Each command handler injects only the dependencies it requires, promoting modularity, testability, and maintainability.
 
 */