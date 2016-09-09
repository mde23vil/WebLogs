using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
    public ActionResult Index(int page = 1, string operationName = "", bool exactMatch = false, string tenant = "undefined", DateTime? beginDate = null, DateTime? endDate = null, string entityType = "", string operationObjectType = "", string user="")
    {
      var viewModel = new Detalization();
      var operations = Repository.GetOpertaionRecords(tenant, beginDate, endDate);

      if (!string.IsNullOrEmpty(operationName))
      {
        operations = exactMatch ? 
          operations.Where(x => x.OperationName == operationName) : 
          operations.Where(x => x.OperationName.ToLower().Contains(operationName));
      }

      if (!string.IsNullOrEmpty(entityType))
        operations = operations.Where(x => x.EntityType == entityType);

      if (!string.IsNullOrEmpty(operationObjectType))
        operations = operations.Where(x => x.OperationObjectType == operationObjectType);

      if (!string.IsNullOrEmpty(user))
        operations = operations.Where(x => x.User == user);

      operations = operations.OrderBy(x => x.Date);

      var parameters = new RouteValueDictionary();

      parameters.Add("page", page);

      if (!string.IsNullOrEmpty(operationName))
      {
        parameters.Add("operationName", operationName);
        parameters.Add("exactMatch", exactMatch);
      }

      if (tenant != "undefined")
        parameters.Add("tenant", tenant);

      if (beginDate != null)
        parameters.Add("beginDate", beginDate);

      if (endDate != null)
        parameters.Add("endDate", endDate);

      if (!string.IsNullOrEmpty(entityType))
        parameters.Add("entityType", entityType);

      if (!string.IsNullOrEmpty(operationObjectType))
        parameters.Add("operationObjectType", operationObjectType);

      return View(new Detalization() { Operations = operations, CurrentPage = page, Parameters = parameters} );
    }
  }
}