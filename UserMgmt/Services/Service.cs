namespace UserMgmt.Services;

using BCrypt.Net;
using System.Text.RegularExpressions;
using UserMgmt.Authorization;
using UserMgmt.Helpers;
using UserMgmt.Models;

public interface IService
{
    AuthenticateResponse GetToken(string id, string secret);
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    User AddNewUser(User user, string password);
    void UpdateUser(User userParam);
    void Delete(int id);
}

public class Service : IService
{
    private DataContext dbContext;
    private IJwtUtils _jwtUtils;

    public Service(DataContext context, IJwtUtils jwtUtils)
    {
        dbContext = context;
        _jwtUtils = jwtUtils;
    }

    public AuthenticateResponse GetToken(string id, string secret)
    {
        var user = dbContext.Users.SingleOrDefault(x => x.Username.ToLower() == id.ToLower());

        // validate username and password
        if (user == null || !BCrypt.Verify(secret, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication successful so generate jwt token
        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        return new AuthenticateResponse(user, jwtToken);
    }

    public IEnumerable<User> GetAllUsers()
    {
        return dbContext.Users;
    }

    public User GetUserById(int id)
    {
        var user = dbContext.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    public User AddNewUser(User user, string password)
    {
        #region Validations
        if (string.IsNullOrEmpty(user.Username))
            throw new AppException("Please enter a valid username");

        if (string.IsNullOrWhiteSpace(password))
            throw new AppException("Password is mandatory");

        if (dbContext.Users.Any(x => x.Username.ToLower() == user.Username.ToLower()))
            throw new AppException("Username \"" + user.Username + "\" is already taken");

        if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
            throw new AppException("All fields are mandatory");
        
        Role role;
        if (!Enum.TryParse<Role>(user.Role.ToString(), true, out role))
            throw new AppException("Not a valid role, Please choose between View, Edit, Delete");
        #endregion
        var newID = dbContext.Users.Select(x => x.Id).Max() + 1;
        user.Id = newID;
        user.PasswordHash = BCrypt.HashPassword(user.PasswordHash);

        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        return user;
    }

    public void UpdateUser(User userParam)
    {
        var user = dbContext.Users.Find(userParam.Id);

        if (user == null)
            throw new AppException("User not found for give ID.");
        else
        {
            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username))
            {
                // throw error if the new username is already taken
                var users = dbContext.Users.Where(a => a.Username.ToLower() != user.Username.ToLower()).ToList();
                if (users.Any(x => x.Username.ToLower() == userParam.Username.ToLower()))
                    throw new AppException("Username " + userParam.Username + " is already taken");
                else
                user.Username = userParam.Username;
            }
            else
                throw new AppException("Please provide a valid UserName for given ID");

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;
            else
                throw new AppException("Please provide a valid First Name");

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;
            else
                throw new AppException("Please provide a valid Last Name");

            Role role;
            if (!Enum.TryParse<Role>(user.Role.ToString(), true, out role))
                throw new AppException("Not a valid role, Please choose between View, Edit, Delete");
            else
                user.Role = userParam.Role;

            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var user = dbContext.Users.Find(id);
        if (user != null)
        {
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }        
        else
            throw new AppException("User with given ID does not exist");
    }

    public User GetUserByName(string name)
    {
        var user = dbContext.Users.SingleOrDefault(x => x.Username == name);
        return user ?? new User();
    }

    public void ChangePassword(string userName, string password)
    {

        if (string.IsNullOrEmpty(userName))
            throw new Exception("User not found");
        var user = dbContext.Users.Find(userName);
        if (user != null)
        {
            Regex rg = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");
            if (rg.Matches(password).Any())
                throw new Exception("Password should have atlease 1 number,1 lowercase,1 uppercase rule and between 8 to 15 characters");
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }
    }
}
