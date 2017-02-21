using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MatioCMS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(int status)
        {
            switch (status)
            {
                case 404:
                    return this.NotFound();
                case 500:
                    return this.InternalServer();
                default:
                    return Content($"HTTP {status}");
            }
        }

        public new ViewResult NotFound()
        {
            return View();
        }

        public ViewResult InternalServer()
        {
            return View();
        }
    }
}
