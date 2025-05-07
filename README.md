🔍 Why Traditional Service Classes Don’t Fit Well in a CQRS Architecture

In modern architectures like CQRS/DDD, code organization is key for long-term maintainability and testability. Traditional Service classes — containing mixed logic like CreateUser, UpdateUser, and GetUserById — often harm this structure.

🔹 Main reasons:

1. Conflict with CQRS Principles
CQRS encourages a clear separation between commands and queries. A single UserService holding multiple responsibilities violates this principle.

2. Violation of SRP (Single Responsibility Principle)
Instead of having a single class with too many roles, CQRS promotes having one handler per operation, like CreateUserCommandHandler, GetUserByIdQueryHandler, etc.

3. Reduced Testability and Extensibility
Handlers are modular and easy to test. Large, all-purpose services with many dependencies are harder to maintain and extend.

4. Handlers Replace Services
In CQRS, especially with MediatR, each command or query has its own handler that acts like a "service" — but with a focused purpose and fewer dependencies.

💡 What about shared business logic?
In those cases, you can use Domain Services, which encapsulate domain-specific rules that don't belong to a single entity. These are different from Application Services and are usually used inside handlers — not as entry points.

Domain Service	Application Service
Lives in the Domain Layer	Lives in the Application Layer
Models business logic	Coordinates workflows/operations
Stateless	May have technical dependencies
Called from handlers/entities	Called from controllers/handlers

✉️ Practical Example: Using an Email Service in CQRS/DDD

Instead of having a shared EmailService, treat it as a technical service in the infrastructure layer and inject it directly where it's needed:


public interface IEmailSender { Task SendAsync(string to, string subject, string body); }

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand> 
{
    private readonly IEmailSender _emailSender;

    public async Task Handle(RegisterUserCommand command) {
        // Send email
        await _emailSender.SendAsync(command.Email, "Welcome!", "Thank you for registering.");
    }
}


✅ This follows SRP, separates concerns, and improves modularity and testability.

📎 See the full example on GitHub 👉  https://github.com/erhankrasniqi/CQRS-without-traditionalservices

💬 What’s your take on this approach? How do you handle shared logic in CQRS-based systems?
