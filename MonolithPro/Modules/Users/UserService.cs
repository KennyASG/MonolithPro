using MonolithPro.Shared;

namespace MonolithPro.Modules.Users;

public class UserService
{
    private readonly List<User> _users;
    private readonly DatabaseContext _dbContext;
    private readonly LoggerService _logger;
    private readonly EmailService _emailService;
    private int _nextId = 4;

    public UserService(DatabaseContext dbContext, LoggerService logger, EmailService emailService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _emailService = emailService;
        
        // Datos iniciales en memoria
        _users = new List<User>
        {
            new User(1, "Juan Pérez", "juan@example.com"),
            new User(2, "María García", "maria@example.com"),
            new User(3, "Carlos López", "carlos@example.com")
        };
        
        _logger.Log("UserService initialized with sample data");
    }

    public List<User> GetAllUsers()
    {
        _logger.Log("Getting all users");
        return _users;
    }

    public User GetUserById(int id)
    {
        _logger.Log($"Getting user by ID: {id}");
        var user = _users.FirstOrDefault(u => u.Id == id);
        
        if (user == null)
        {
            _logger.LogWarning($"User with ID {id} not found");
        }
        
        return user;
    }

    public User AddUser(User user)
    {
        _logger.Log($"Adding new user: {user.Name}");
        user.Id = _nextId++;
        _users.Add(user);
        _dbContext.SaveChanges();
        
        // Enviar email de bienvenida
        _emailService.SendWelcomeEmail(user.Email, user.Name);
        
        _logger.Log($"User created with ID: {user.Id}");
        return user;
    }

    public bool UserExists(int userId)
    {
        return _users.Any(u => u.Id == userId);
    }
}