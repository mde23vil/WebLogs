using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogAnalyzer.Filters;
using LogAnalyzer.ViewModel;

namespace LogAnalyzer.Controllers
{
  [Culture]
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.From = Repository.GetFirstDate();
      ViewBag.To = Repository.GetLastDate();

      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}