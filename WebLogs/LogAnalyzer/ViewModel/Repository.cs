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
    private static List<OperationRecord> _operations;

    public static List<OperationRecord> GetOpertaionRecords()
    {
      _operations = _operations ?? SessionFactory.GetSession().Query<OperationRecord>().ToList();

      return _operations;
    }

    public static DateTime GetFirstDate()
    {
      return SessionFactory.GetSession().Query<OperationRecord>().OrderBy(x => x.Date).Select(x => x.Date).FirstOrDefault().Value;
    }

    public static DateTime GetLastDate()
    {
      return SessionFactory.GetSession().Query<OperationRecord>().OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault().Value;
    }
  }
}