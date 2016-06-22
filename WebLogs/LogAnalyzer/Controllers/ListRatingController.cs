using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using LogAnalyzer.Helpers;
using LogAnalyzer.Models;
using LogAnalyzer.NHibernate;
using LogAnalyzer.ViewModel;

namespace LogAnalyzer.Controllers
{
  public class ListRatingController : Controller
  {
    // GET: ListRating
    public ActionResult Index()
    {
      ViewBag.DetailsPageSubTitle = "List use rating";
      ViewBag.Tenants = Tenants.ObtainTenantList();
      ViewData["Tenants"] = SharedHelpers.StringsToSelectList(ViewBag.Tenants);

      return View(Repository.GetOpertaionRecords());
    }

    public ActionResult ListRatingChart(string tenant = "undefined", DateTime? fromDate = null, DateTime? toDate = null)
    {
      var operations = Repository.GetOpertaionRecords()
        .Where(x => x.OperationName.ToLower().Contains("open list"));

      operations = OperationRecord.GlobalFilters(operations, tenant, fromDate, toDate);
      var points = SharedHelpers.GetSeriesByOperationObjectType(operations);

      var chart = new Highcharts("ListOpenings")
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
                  Name = "Количество раз",
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("ListRatingChartView", operations);
    }

    public ActionResult ListRatingByTimeChart(string tenant = "undefined", DateTime? fromDate = null, DateTime? toDate = null)
    {
      var operations = Repository.GetOpertaionRecords()
        .Where(x => x.OperationName.ToLower().Contains("open list"));

      operations = OperationRecord.GlobalFilters(operations, tenant, fromDate, toDate);
      var points = GetSeriesByOperationObjectTypeAverageDuration(operations);

      var chart = new Highcharts("ListTimings")
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
                  Name = "Среднее время открытия",
                  Data = new Data(points)
                });

      ViewBag.ByTimeChart = chart;
      return PartialView("ListRatingChartByTimeView", operations);
    }

    public static Point[] GetSeriesByOperationObjectTypeAverageDuration(IEnumerable<OperationRecord> operations)
    {
      var group = operations.ToList().GroupBy(x => x.OperationObjectType)
        .Select(x => new Point
        {
          Name = x.Key,
          Y = x.Sum(z => z.Duration) / x.Count(),
        }).ToArray();

      return group;
    }
  }
}