using forum.DTO;
using forum.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserInfoController : ControllerBase
    {
        private readonly ForumContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // GET: api/<UserController>
        public UserInfoController(ForumContext context, IHttpContextAccessor httpContextAccessor)
            
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public IActionResult Get()
        {
            string v = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
            Guid id  = Guid.Parse(v);

            //username = HttpContext.User.FindFirstValue("id");

            /*
            var User = (from a in _context.Users
                        where a.Id == id
                        select a).SingleOrDefault();
            */
            var User = _context.Users.Find(id);
            if (User == null)
                return StatusCode(400, "沒有找到"+ id);
            
            return StatusCode(200, User);
            
            
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]

        public ActionResult<User> GetUser(Guid id)
        {
            var result = _context.Users.Find(id);
            if (result == null)
            {
                return NotFound(" Not found");
            }
            return result;
            
        }

        // POST api/<UserController>
        [HttpPost()]
        public IActionResult Post([FromBody] UserInfoUpdateDto value)
        {
            var update = (from a in _context.Users
                          where a.Id == value.Id
                          select a).SingleOrDefault();
            if (update != null)
            {
                update.Name = value.Name;
                update.Email = value.Email;
                
                update.Sex = value.Sex;
                update.Phone = value.Phone;
                _context.SaveChanges();
                return StatusCode(200, "update successful");
            }

            return StatusCode(400, "update fail");



        }
    }
}
