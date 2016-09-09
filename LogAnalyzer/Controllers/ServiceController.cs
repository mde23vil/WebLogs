using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogAnalyzer.Controllers
{
  public class ServiceController : Controller
  {
    // GET: Service
    public ActionResult ChangeCulture(string lang)
    {
      string returnUrl = Request.UrlReferrer.AbsolutePath;
      HttpCookie cookie = Request.Cookies["lang"];
      List<string> availableCultures = new List<string>() { "ru", "en" };

      if (!availableCultures.Contains(lang))
      {
        lang = "ru";
      }

      if (cookie != null)
      {
        cookie.Value = lang;
      }
      else
      {
        cookie = new HttpCookie("lang")
        {
          HttpOnly = false,
          Value = lang,
          Expires = DateTime.Now.AddYears(1)
        };
      }

      Response.Cookies.Add(cookie);
      return Redirect(returnUrl);
    }
  }
}