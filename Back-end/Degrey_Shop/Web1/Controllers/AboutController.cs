﻿using Microsoft.AspNetCore.Mvc;

namespace Web1.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
