using MonolithPro.Shared;
using MonolithPro.Data;

namespace MonolithPro.Modules.Users;

public class UserService
{
    private readonly MonolithProDbContext _dbContext;
    private readonly LoggerService _logger;
    private readonly EmailService _emailService;

    public UserService(MonolithProDbContext dbContext, LoggerService logger, EmailService emailService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _emailService = emailService;

        _logger.Log("UserService initialized with SQL Server backend");
    }

    public List<User> GetAllUsers()
    {
        _logger.Log("Getting all users from database");
        return _dbContext.Users.ToList();
    }

    public User GetUserById(int id)
    {
        _logger.Log($"Getting user by ID: {id}");
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            _logger.LogWarning($"User with ID {id} not found");
        }

        return user;
    }

    public User AddUser(User user)
    {
        _logger.Log($"Adding new user: {user.Name}");

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        // Enviar email de bienvenida
        _emailService.SendWelcomeEmail(user.Email, user.Name);

        _logger.Log($"User created with ID: {user.Id}");
        return user;
    }

    public bool UserExists(int userId)
    {
        return _dbContext.Users.Any(u => u.Id == userId);
    }
}