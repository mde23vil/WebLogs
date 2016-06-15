using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogAnalyzer.Helpers;
using LogAnalyzer.Models;
using LogAnalyzer.NHibernate;
using NHibernate.Linq;

namespace LogAnalyzer.Controllers
{
  public class UserwiseOperationsController : Controller
  {
    // GET: UserwiseOperations
    public ActionResult Index()
    {
      var session = SessionFactory.GetSession();
      var operations = session.Query<OperationRecord>()
        .Where(x => EntityTypes.GetDocumentInterfaces().Contains(x.EntityType) ||
        EntityTypes.GetDocumentTypes().Contains(x.OperationObjectType));
      ViewBag.Tenants = operations.Select(x => x.Tenant).ToList().Distinct();
      ViewData["Tenants"] = StringsToSelectList(ViewBag.Tenants);
      return View();
    }




    private static SelectList StringsToSelectList(IEnumerable<string> strings)
    {
      var items = strings.Select(x => new { Id = x, Text = x }).ToList();
      items.Add(new { Id = "undefined", Text = "undefined" });
      return new SelectList(items, "Id", "Text", "undefined");
    }
  }
}