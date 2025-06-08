using Microsoft.IdentityModel.Tokens;
using REST_project.Controllers.Models;
using REST_project.Controllers.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace REST_project.Controllers.Services
{
    public class UserMockService : IUserMockService
    {
        private readonly List<UserDTO> _users = new List<UserDTO>();
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contexAccessor;

        public UserMockService(IConfiguration configuration, IHttpContextAccessor contexAccessor)
        {
            _configuration = configuration;
            _contexAccessor = contexAccessor;
        }

        public bool Register(UserDTO user)
        {
            if (_users.Any(u => u.Username == user.Username))
                return false;

            _users.Add(user);
            return true;
        }

        public string? Authinticate(UserDTO user)
        {
            UserDTO? existingUser = _users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (existingUser == null)
                return null;

            string key = _configuration.GetRequiredSection("JwtSettings")["SecretKey"] 
                         ?? throw new ArgumentNullException("JwtSettings:SecretKey not found");

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserDTO? getCurrentUser()
        {
            var userName = _contexAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            return _users.FirstOrDefault(u => u.Username == userName);
        }

        public bool PutUser(UserDTO updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Username == updatedUser.Username);
            if (user == null)
                return false;

            user.Password = updatedUser.Password;
            return true;
        }

        public bool DeleteUser(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return false;

            _users.Remove(user);
            return true;
        }
    }
}
