using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts.Options;
using LogAnalyzer.Models;

namespace LogAnalyzer.Helpers
{
  public class SharedHelpers
  {
    public static Point[] GetSeriesByOperationObjectType(IEnumerable<OperationRecord> operations)
    {
      var group = operations.ToList().GroupBy(x => x.OperationObjectType)
        .Select(x => new Point
        {
          Name = x.Key,
          Y = x.Count(),
        }).ToArray();

      return group;
    }

    public static SelectList StringsToSelectList(IEnumerable<string> strings)
    {
      var items = strings.Select(x => new { Id = x, Text = x }).ToList();
      return new SelectList(items, "Id", "Text", items.First().Id);
    }
  }
}