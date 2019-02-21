using System;
using Microsoft.AspNetCore.Mvc;

namespace CoderAndy.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("Blog/{category}/{name}")]
        public IActionResult ViewPost(int a_id)
        {
            throw new NotImplementedException();
        }
    }
}