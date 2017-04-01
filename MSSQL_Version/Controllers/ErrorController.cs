using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MSSQL_Version.Controllers
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