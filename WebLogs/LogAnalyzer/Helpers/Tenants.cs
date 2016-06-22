using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogAnalyzer.Models;
using LogAnalyzer.NHibernate;
using LogAnalyzer.ViewModel;
using NHibernate.Linq;

namespace LogAnalyzer.Helpers
{
  public class Tenants
  {
    public static List<string> ObtainTenantList()
    {
      var operations = Repository.GetOpertaionRecords();
      return operations.Select(x => x.Tenant).Distinct().ToList();
    }
  }
}