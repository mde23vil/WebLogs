using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using LogAnalyzer.Filters;
using LogAnalyzer.Helpers;
using LogAnalyzer.Models;
using LogAnalyzer.NHibernate;
using LogAnalyzer.ViewModel;
using NHibernate.Linq;

namespace LogAnalyzer.Controllers
{
  [Culture]
  public class UserwiseOperationsController : Controller
  {
    // GET: UserwiseOperations
    public ActionResult Index()
    {
      ViewBag.Tenants = Tenants.ObtainTenantList();
      ViewBag.DetailsPageSubTitle = "Operations by users";
      ViewData["Tenants"] = SharedHelpers.StringsToSelectList(ViewBag.Tenants);
      return View();
    }

    public ActionResult UserCreateOperations(string tenant, DateTime? fromDate = null, DateTime? toDate = null)
    {
      var operationNamePart = "create";
      var operations = Repository.GetOpertaionRecords(tenant, fromDate, toDate)
        .Where(x => x.OperationName.ToLower().Contains("create"));

      var points = GetSeriesByUser(operations);
      foreach (var point in points)
      {
        point.Events = new PointEvents()
        {
          Click = SharedHelpers.GenerateDetalizationLink(this.Url, SharedHelpers.GenerateDetalizationParameters(operationNamePart, false, tenant, user: point.Name))
        };
      }

      var chart = new Highcharts("CreationOperations")
                .InitChart(new Chart { PlotShadow = false, PlotBackgroundColor = null, PlotBorderWidth = null, MarginTop = 50 })
                .SetExporting(new Exporting() { Enabled = false })
                .SetTitle(new Title { Text = "", Align = HorizontalAligns.Left })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.y; }" })
                .SetLegend(new Legend { ItemStyle = "fontWeight: 'normal'" })
                .SetPlotOptions(new PlotOptions
                {
                  Column = new PlotOptionsColumn()
                  {
                    AllowPointSelect = true,
                    Cursor = Cursors.Pointer,
                    ShowInLegend = true
                  }
                })
                .SetXAxis(new XAxis
                {
                  Categories = points.Select(x => x.Name).ToArray()
                })
                .SetSeries(new Series
                {
                  Type = ChartTypes.Column,
                  Name = Resources.Resources.CreatedEntities,
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("UserCreateOperations", operations);
    }

    public ActionResult UserEditOperations(string tenant, DateTime? fromDate = null, DateTime? toDate = null)
    {
      var operationNamePart = "edit";
      var operations = Repository.GetOpertaionRecords(tenant, fromDate, toDate)
        .Where(x => x.OperationName.ToLower().Contains(operationNamePart));
      
      var points = GetSeriesByUser(operations);
      foreach (var point in points)
      {
        point.Events = new PointEvents()
        {
          Click = SharedHelpers.GenerateDetalizationLink(this.Url, SharedHelpers.GenerateDetalizationParameters(operationNamePart, false, tenant, user: point.Name))
        };
      }

      var chart = new Highcharts("EditOperations")
                .InitChart(new Chart { PlotShadow = false, PlotBackgroundColor = null, PlotBorderWidth = null, MarginTop = 50 })
                .SetExporting(new Exporting() { Enabled = false })
                .SetTitle(new Title { Text = "", Align = HorizontalAligns.Left })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.y; }" })
                .SetLegend(new Legend { ItemStyle = "fontWeight: 'normal'" })
                .SetPlotOptions(new PlotOptions
                {
                  Column = new PlotOptionsColumn()
                  {
                    AllowPointSelect = true,
                    Cursor = Cursors.Pointer,
                    ShowInLegend = true
                  }
                })
                .SetXAxis(new XAxis
                {
                  Categories = points.Select(x => x.Name).ToArray()
                })
                .SetSeries(new Series
                {
                  Type = ChartTypes.Column,
                  Name = Resources.Resources.EditedEntities,
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("UserEditOperations", operations);
    }

    public ActionResult UserDocumentBodyOperations(string tenant, DateTime? fromDate = null, DateTime? toDate = null)
    {
      var operationNamePart = "open document to";
      var operations = Repository.GetOpertaionRecords(tenant, fromDate, toDate)
        .Where(x => x.OperationName.ToLower().Contains(operationNamePart));

      var points = GetSeriesByUser(operations);
      foreach (var point in points)
      {
        point.Events = new PointEvents()
        {
          Click = SharedHelpers.GenerateDetalizationLink(this.Url, SharedHelpers.GenerateDetalizationParameters(operationNamePart, false, tenant, user: point.Name))
        };
      }

      var chart = new Highcharts("DocumentBodyOperations")
                .InitChart(new Chart { PlotShadow = false, PlotBackgroundColor = null, PlotBorderWidth = null, MarginTop = 50 })
                .SetExporting(new Exporting() { Enabled = false })
                .SetTitle(new Title { Text = "", Align = HorizontalAligns.Left })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.y; }" })
                .SetLegend(new Legend { ItemStyle = "fontWeight: 'normal'" })
                .SetPlotOptions(new PlotOptions
                {
                  Column = new PlotOptionsColumn()
                  {
                    AllowPointSelect = true,
                    Cursor = Cursors.Pointer,
                    ShowInLegend = true
                  }
                })
                .SetXAxis(new XAxis
                {
                  Categories = points.Select(x => x.Name).ToArray()
                })
                .SetSeries(new Series
                {
                  Type = ChartTypes.Column,
                  Name = Resources.Resources.VersionEditEntities,
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("UserDocumentBodyOperations", operations);
    }

    private Point[] GetSeriesByUser(IQueryable<OperationRecord> operations)
    {
      var group = operations.GroupBy(x => x.User)
        .Select(g => new { User = g.Key, Count = g.Count() })
        .OrderByDescending(p => p.Count)
        .ToList();

      var maxValue = group.Select(g => g.Count).Max();
      var minValue = maxValue * 0.05;

      var points = group.Where(g => g.Count > minValue).Select(x => new Point
        {
          Name = x.User,
          Y = x.Count,
          Events = new PlotOptionsSeriesPointEvents { Click =
            "function() {" + 
            "var tenant = $('#tenant')[0].options[$('#tenant')[0].selectedIndex].value;" +
            $"return $.get('/UserwiseOperations/CreateOperationsByUser?tenant=' + tenant + '&user={x.User}').html(); " +
            "}"
          }
        })
        .ToArray();

      return points;
    }
  }
}