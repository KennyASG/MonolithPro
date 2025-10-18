using Microsoft.AspNetCore.Mvc;

namespace MonolithPro.Modules.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<List<User>> GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        
        if (user == null)
        {
            return NotFound(new { message = $"User with ID {id} not found" });
        }
        
        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> CreateUser([FromBody] CreateUserRequest request)
    {
        var user = new User(0, request.Name, request.Email);
        var createdUser = _userService.AddUser(user);
        
        return CreatedAtAction(
            nameof(GetUserById), 
            new { id = createdUser.Id }, 
            createdUser
        );
    }
}

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}