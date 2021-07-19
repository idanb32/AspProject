using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return Content("There was an unexpected error. Please come back later");
        }
    }
}
