using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
      var operations = _operations ?? SessionFactory.GetSession().Query<OperationRecord>().ToList();

      return operations;
    } 
  }
}