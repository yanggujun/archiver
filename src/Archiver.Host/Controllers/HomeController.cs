using System;
using System.IO;
using Archiver.Common;
using Commons.Collections.Map;
using Commons.Json;
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

        public JsonResult Init()
        {
            return new JsonResult(target.Send<CategoryListReqMsg>());
        }

        public JsonResult GetCategory(string name)
        {
            return new JsonResult(target.Send(new CategoryReqMsg { Name = name }));
        }

        public JsonResult GetFolder(long id)
        {
            var json = target.Send(new ItemReqMsg { Id = id });
            return new JsonResult(json);
        }

        public FileResult GetFile(long id)
        {
            var json = target.Send(new ItemReqMsg { Id = id });
            var dict = JsonMapper.To<HashedMap<string, string>>(json);
            var path = dict["path"];
            var name = dict["name"];

            var fs = new FileStream(path, FileMode.Open);
            return File(fs, "application/any", name);
        }
    }
}
