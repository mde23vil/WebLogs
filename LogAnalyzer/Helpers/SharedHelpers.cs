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
    public static Point[] GetSeriesByEntityType(IQueryable<OperationRecord> operations)
    {
      var group = operations.GroupBy(x => x.EntityType)
        .Select(g => new { EntityType = g.Key, Count = g.Count() })
        .OrderByDescending(p => p.Count)
        .ToList();

      var points = group
        .Select(x => new Point
        {
          Name = x.EntityType,
          Y = x.Count
        }).ToArray();

      return points;
    }

    public static Point[] GetSeriesByOperationObjectType(IQueryable<OperationRecord> operations)
    {
      var group = operations.GroupBy(x => x.OperationObjectType)
        .Select(g => new { OperationObjectType = g.Key, Count = g.Count() })
        .OrderByDescending(p => p.Count)
        .ToList();


      var maxValue = group.Select(g => g.Count).Max();
      var minValue = maxValue * 0.05;

      var points = group.Where(g => g.Count > minValue)
        .Select(x => new Point
        {
          Name = x.OperationObjectType,
          Y = x.Count,
        }).ToArray();

      return points;
    }

    public static SelectList StringsToSelectList(IEnumerable<string> strings)
    {
      var items = strings.Select(x => new { Id = x, Text = x }).ToList();
      return new SelectList(items, "Id", "Text", items.First().Id);
    }
  }
}