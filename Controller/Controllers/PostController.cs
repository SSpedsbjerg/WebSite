using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase {
        private const string twitterName = "";
        private const string linkedinName = "";

        [HttpGet]
        [Route("/")]
        public string NoRequest() {
            return "200";
        }

        [HttpGet(Name = "GetAllPosts")]
        [Route("/GetAllPosts")]
        public string GetAllPosts() {
            return "{}";
        }

        [HttpPost(Name = "Post")]
        [Route("/Post")]
        public string AddPost([FromBody] string body) {
            Post? post = JsonSerializer.Deserialize<Post>(body);
            if (post != null) {
                return DatabaseController.AddPost(post) ? "200" : "500";
            }
            else return "500";
        }
    }
}
