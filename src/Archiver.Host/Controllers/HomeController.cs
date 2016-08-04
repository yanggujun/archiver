using System;
using Archiver.Common;
using Commons.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Archiver.Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITarget target;
        public HomeController()
        {
            target = new MessageTarget(Constants.ServerUri);
        }

        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
