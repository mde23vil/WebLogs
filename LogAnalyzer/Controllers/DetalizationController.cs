using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogAnalyzer.Filters;
using LogAnalyzer.Models;
using LogAnalyzer.ViewModel;
using NHibernate.Criterion;
using NHibernate.Type;

namespace LogAnalyzer.Controllers
{
  [Culture]
  public class DetalizationController : Controller
  {
    // GET: Detalisation
    public ActionResult Index(string operationName = "", string tenant = "undefined", DateTime? beginDate = null, DateTime? endDate = null, string entityType = "", int page = 1)
    {
      var entriesPerPage = 25;
      var operations = Repository.GetOpertaionRecords(tenant, beginDate, endDate);
      ViewBag.PagesAmount = operations.Count() / entriesPerPage + 1;
      ViewBag.CurrentPage = page;
      
      operations = operations.OrderBy(x => x.Date);

      operations = operations.Skip((page - 1) * entriesPerPage).Take(entriesPerPage);
      
      return View(operations);
    }
  }
}