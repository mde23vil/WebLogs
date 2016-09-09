using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using LogAnalyzer.Models;

namespace LogAnalyzer.ViewModel
{
  public class Detalization
  {
    const int entriesPerPage = 25;

    private IEnumerable<OperationRecord> _operations;

    public IEnumerable<OperationRecord> Operations
    {
      get { return _operations.Skip((_currentPage - 1) * entriesPerPage).Take(entriesPerPage); }
      set { _operations = value; }
    }

    private int _currentPage;

    public int CurrentPage
    {
      get { return _currentPage; }
      set { _currentPage = value; }
    }

    private RouteValueDictionary _parameters;

    public RouteValueDictionary Parameters
    {
      get { return _parameters; }
      set { _parameters = value; }
    }

    public int FirstPage => 1;
    public int LastPage => _operations.Count() / entriesPerPage + 1;
    
  }
}