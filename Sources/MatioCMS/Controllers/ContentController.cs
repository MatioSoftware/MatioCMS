using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace MatioCMS.Controllers
{
    public class ContentController : Controller
    {
        private Database.DB DB;

        public ContentController(Database.DB Database)
        { DB = Database; }

        public IActionResult Index()
        {
            return View();
        }
    }
}