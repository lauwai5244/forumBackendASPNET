using forum.DTO;
using forum.Models;
using forum.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Collections;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace forum.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ControllerBase
    {
        private readonly ForumContext _context;
        private readonly JWTTokenOptions _JWTTokenOptions;
        
        // GET: api/<UserController>
        public UsersController(ForumContext context, IOptionsMonitor< JWTTokenOptions> JWTTokenOptions)
        {
            _context = context;
            _JWTTokenOptions = JWTTokenOptions.CurrentValue;

        }


        [HttpPut]
        //  註冊
        public IActionResult Register([FromBody] UserDto value)
        {
            var sameName = (from a in _context.Users
                            where a.Username == value.Username
                            select a).SingleOrDefault();

            if (sameName != null)
            {
                return StatusCode(400, "User account is duplicated");
            }
            Hashtable ht = new Hashtable();
            var Password = ChangPwd.UserMd5(value.Password);
            User insert = new User
            {
                Id = Guid.NewGuid(),
                Username = value.Username,
                Password = Password,
                Name = null,
                UserImage = null,
                Email = null,
                Sex = null,
                Phone = null,
                CreationTime = DateTime.Now,
                Salt = null ,
            };
            _context.Users.Add(insert);
            _context.SaveChanges();
            return StatusCode(200, "successfully");

        }

        [HttpPost]
        // 登入

        public IActionResult Login([FromBody] UserDto value)
        {
            
            Hashtable ht = new Hashtable();
            var Password = ChangPwd.UserMd5(value.Password);

            var User = (from a in _context.Users
                        where a.Username == value.Username
                        && a.Password == Password
                        select a).SingleOrDefault();
            if (User != null)
            {

                

                    var claims = new[]{
                        new Claim(ClaimTypes.Name, User.Username),
                        new Claim("Id", User.Id.ToString()),
                        };

                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTTokenOptions.SecurityKey));

                    SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: _JWTTokenOptions.Issuer,
                        audience: _JWTTokenOptions.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(300),
                        signingCredentials: creds);

                    string returnToken = new JwtSecurityTokenHandler().WriteToken(token);


                    return StatusCode(200, returnToken);

                }
            else
                {

                    return StatusCode(400, "account or password error");
                }

            

        }


        [HttpDelete]
        public IActionResult Logout()
            
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return StatusCode(200, "logout succeful");
        }

        [HttpGet("NoLogin")]
        public string noLogin()
        {
            return "未登入";
        }

    }

}

/*

 class Hash
{
    public byte[] CreateSalt()
    {
        var buffer = new byte[16];
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(buffer);
        return buffer;
    }
    // Hash 處理加鹽的密碼功能
    public byte[] HashPassword(string password, byte[] salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

        //底下這些數字會影響運算時間，而且驗證時要用一樣的值
        argon2.Salt = salt;
        argon2.DegreeOfParallelism = 8; // 4 核心就設成 8
        argon2.Iterations = 4; // 迭代運算次數
        argon2.MemorySize = 1024 * 1024; // 1 GB

        return argon2.GetBytes(16);
    }

    //驗證
    public bool VerifyHash(string password, byte[] salt, byte[] hash)
    {
        var newHash = HashPassword(password, salt);
        return hash.SequenceEqual(newHash); // LINEQ
    }

}
*/

public class ChangPwd
{
    public static string UserMd5(string str)
    {
        string encode = str;
        string pwd = "";
        MD5 md5 = MD5.Create();
        //加密后就是一个字节类型的数组
        byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(encode));
        for (int i = 0; i < s.Length; i++)
        {
            //将获得的字符串使用十六进制类型格式.格式后的字符串是小写的字母
            pwd += s[i].ToString("x");
        }
        return pwd;
    }
}


