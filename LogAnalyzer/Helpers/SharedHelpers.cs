﻿using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
          Y = x.Count,
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

    public static RouteValueDictionary GenerateDetalizationParameters(string operationNamePart = "", bool exactMatch = false, string tenant = "", string entityType = "", string operationObjectType = "", string user = "")
    {
      RouteValueDictionary parameters = new RouteValueDictionary();

      if (!string.IsNullOrEmpty(tenant))
      {
        parameters.Add("tenant", tenant);
      }

      if (!string.IsNullOrEmpty(operationNamePart))
      {
        parameters.Add("operationName", operationNamePart);
        parameters.Add("exactMatch", exactMatch);
      }

      if (!string.IsNullOrEmpty(entityType))
        parameters.Add("entityType", entityType);

      if (!string.IsNullOrEmpty(operationObjectType))
        parameters.Add("operationObjectType", operationObjectType);

      if (!string.IsNullOrEmpty(user))
        parameters.Add("user", user);

      return parameters;// SharedHelpers.GenerateDetalizationLink(url, parameters);
    }

    public static string GenerateDetalizationLink(UrlHelper url, RouteValueDictionary parameters)
    {
      return "function() { window.location.href = \""
             + url.Action("Index", "Detalization", parameters)
             + "\" " +
             "}";
    }
  }
}