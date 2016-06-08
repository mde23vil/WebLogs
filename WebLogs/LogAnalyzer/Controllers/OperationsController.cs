using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using LogAnalyzer.Helpers;
using LogAnalyzer.Models;
using LogAnalyzer.NHibernate;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Linq;

namespace LogAnalyzer.Controllers
{
  public class OperationsController : Controller
  {
    // GET: Operations
    public ActionResult Index()
    {
      var session = SessionFactory.GetSession();
      var operations = session.Query<OperationRecord>().Where(x => x.OperationName.ToLower().Contains("create")).Where(x => EntityTypes.GetDocumentTypes().Contains(x.EntityType));
      ViewBag.Tenants = operations.Select(x => x.Tenant).ToList().Distinct();
      ViewData["Tenants"] = StringsToSelectList(ViewBag.Tenants);

      return View(operations);

    }

    public ActionResult CreateOperationStatisticsChart(string tenant = "undefined")
    {
      var session = SessionFactory.GetSession();
      var operations = session.Query<OperationRecord>()
        .Where(x => EntityTypes.GetDocumentTypes().Contains(x.EntityType))
        .Where(x => x.OperationName.ToLower().Contains("create"));

      if (tenant != "undefined")
        operations = operations.Where(x => x.Tenant == tenant);

      var points = GetSeries(operations);

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
                  Name = "Типы сущностей",
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("CreateOperationStatisticsChart", operations);
    }

    private static Point[] GetSeries(IEnumerable<OperationRecord> operations)
    {
      var group = operations.GroupBy(x => x.EntityType)
        .Select(x => new Point
        {
          Name = x.Key,
          Y = x.Count(),
        }).ToArray();

      return group;
    }

    private static SelectList StringsToSelectList(IEnumerable<string> strings)
    {
      var items = strings.Select(x => new { Id = x, Text = x }).ToList();
      items.Add(new { Id = "undefined", Text = "undefined" });
      return new SelectList(items, "Id", "Text", "undefined");
    }
  }
}