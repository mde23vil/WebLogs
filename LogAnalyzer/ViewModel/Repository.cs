using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using LogAnalyzer.Models;
using LogAnalyzer.NHibernate;
using NHibernate.Linq;

namespace LogAnalyzer.ViewModel
{
  public class Repository
  {
    public static IQueryable<OperationRecord> GetOpertaionRecords()
    {
      return SessionFactory.GetSession().Query<OperationRecord>();
    }

    public static IQueryable<OperationRecord> GetOpertaionRecords(string tenant, DateTime? fromDate, DateTime? toDate)
    {
      var operations = GetOpertaionRecords();

      if (tenant != "undefined")
        operations = operations.Where(x => x.Tenant == tenant);

      if (fromDate.HasValue)
        operations = operations.Where(x => x.Date > fromDate.Value);

      if (toDate.HasValue)
        operations = operations.Where(x => x.Date < toDate.Value);

      return operations;
    }

    public static DateTime GetFirstDate()
    {
      var session = SessionFactory.GetSession();
      var firstDate = session.Query<OperationRecord>().OrderBy(x => x.Date).Select(x => x.Date).FirstOrDefault().Value;
      session.Close();
      return firstDate;
    }

    public static DateTime GetLastDate()
    {
      var session = SessionFactory.GetSession();
      var lastDate = session.Query<OperationRecord>().OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault().Value;
      session.Close();
      return lastDate;
    }
  }
}