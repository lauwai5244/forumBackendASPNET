using forum.DTO;
using forum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace forum.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]

    public class ArticleController : ControllerBase
    {
        // GET: api/<articleController>
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ForumContext _context;
        // GET: api/<UserController>
        public ArticleController(ForumContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
       
        // 全部
        public ActionResult<IEnumerable<Article>> Get()
        {
            return _context.Articles;
        }

        // GET api/<articleController>/5

        
        [HttpGet("GetArticleFromUserId")]
        // 用戶id
        public IEnumerable<GetArticleListDto> GetArticleFromUserId()
        {
            string v = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
            Guid id = Guid.Parse(v);
            var result = _context.Articles
             .Select(a => new GetArticleListDto
             {
                 Id = a.Id,
                 Title = a.Title,
                 Text = a.Text,
                 CreationUserId = a.CreationUserId
             });

            result = result.Where(a => a.CreationUserId == id);
            return result;
        }

        [HttpGet("select")]
        // 用戶id
        public IActionResult GetArticleFromId(string word)
        {
            string sql = "select * from Article where ";

            if (!string.IsNullOrWhiteSpace(word))
            {
                sql = sql + "title like N'%" + word + "%'";
            }

            var result = _context.Articles.FromSqlRaw(sql);

            return StatusCode(200, result);
            
        }

        // POST api/<articleController>
        [HttpPost]
        //  新增
        public IActionResult Post([FromBody] ArticleDto value)
        {

            string v = _httpContextAccessor.HttpContext.User.FindFirstValue("Id");
            Guid id = Guid.Parse(v);
            Article insert = new Article
            {
                Id = Guid.NewGuid(),
                Title = value.Title,
                Text = value.Text,
                CreationTime = DateTime.Now,
                CreationUserId = id
            };
            _context.Articles.Add(insert);
            _context.SaveChanges();
            return StatusCode(200, value);

        }

        // PUT api/<articleController>/5
        [HttpPut]
        //  修改
        public IActionResult Put( [FromBody] UpdateArticleListDto value)
        {
            var Article = (from a in _context.Articles
                           where a.Id == value.Id
                        select a).SingleOrDefault();

            if(Article == null)
                return NotFound("Not found");

            Article.Title = value.Title;
            Article.Text = value.Text;


            _context.SaveChanges();
            return StatusCode(200, value);

        }


        // DELETE api/<articleController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = (from a in _context.Articles
                           where a.Id == id
                           select a).SingleOrDefault();

            if (result == null)
            {
                return NotFound(" Not found");
            }

            _context.Articles.Remove(result);
            _context.SaveChanges();

            return StatusCode(200, "Delect success");
        }
    }
}
