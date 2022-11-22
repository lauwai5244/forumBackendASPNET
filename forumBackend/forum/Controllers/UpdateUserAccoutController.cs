using forum.DTO;
using forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using forum.Controllers;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateUserAccoutController : ControllerBase
    {

        // POST api/<UpdateUserAccoutController>

        private readonly ForumContext _context;
        public UpdateUserAccoutController(ForumContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UpdateUserDto value)
        {
            Hashtable ht = new Hashtable();
            var Password = ChangPwd.UserMd5(value.OldPassword);

            var User = (from a in _context.Users
                            where a.Username == value.Username
                            && a.Password == Password
                        select a).SingleOrDefault();
            // 先拿到user資料

            if(value.OldPassword == value.NewPassword)
            {
                return StatusCode(400, "same password");
            }

            if (User == null)
            {
                return StatusCode(400, "account or password error");
            }
           
            //如果成功了就把新的密碼加秘
            else
            {
                //Hashtable ht = new Hashtable();
                var NewPassword = ChangPwd.UserMd5(value.NewPassword);
                User.Password = NewPassword;
               
                _context.SaveChanges();
                return StatusCode(200, value);
            }
            
            

            
        }   

    }
}

