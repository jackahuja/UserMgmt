namespace UserMgmt.Controllers;
using Microsoft.AspNetCore.Mvc;
using UserMgmt.Authorization;
using UserMgmt.Models;
using UserMgmt.Services;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IService _service;
    public UsersController(IService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("GetToken")]
    public IActionResult GetToken(string id, string secret)
    {
        var output = _service.GetToken(id, secret);
        return Ok(output);
    }

    [HttpGet("GetAllUsers")]
    public IActionResult GetAllUsers()
    {
        var output = _service.GetAllUsers();
        return Ok(output);
    }

    [Authorize(Role.View)]
    [HttpGet("GetUserByID")]
    public IActionResult GetUserById(int id)
    {
        // only admins can access other user records
        //var currentUser = (User)HttpContext.Items["User"];
        //if (currentUser == null || ( id != currentUser.Id && currentUser.Role != Role.Admin))
        //    return Unauthorized(new { message = "Unauthorized" });
        var output = _service.GetUserById(id);
        return Ok(output);
    }

    [Authorize(Role.Admin)]
    [HttpPost("AddNewUser")]
    public IActionResult AddNewUser(User user, string password)
    {
        _service.AddNewUser(user, password);
        return Ok(new { response = "User added sucessfully" });
    }

    [Authorize(Role.Edit)]
    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(User user)
    {
        _service.UpdateUser(user);
        return Ok(new { response = "User updated sucessfully" });
    }

    [Authorize(Role.Admin)]
    [HttpDelete("Delete")]
    public IActionResult Delete(int id)
    {
        _service.Delete(id);
        return Ok(new { response = "User deleted sucessfully" });
    }
}