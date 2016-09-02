using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogAnalyzer.Filters;
using LogAnalyzer.ViewModel;
using NHibernate.Criterion;
using NHibernate.Type;

namespace LogAnalyzer.Controllers
{
  [Culture]
  public class DetalizationController : Controller
  {
    // GET: Detalisation
    public ActionResult Index(string operationName = "", string tenant="undefined", DateTime? beginDate = null, DateTime? endDate = null, string entityType = "")
    {
      var operations = Repository.GetOpertaionRecords(tenant, beginDate, endDate);

      // TODO: Delete when pagination will be done.
      if (operations.Count > 100)
        operations = operations.Take(100).ToList();

      return View(operations);
    }
  }
}