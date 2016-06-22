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
using NHibernate.Linq;

namespace LogAnalyzer.Controllers
{
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
      var operations = Repository.GetOpertaionRecords()
        .Where(x => x.OperationName.ToLower().Contains("create"));

      operations = OperationRecord.GlobalFilters(operations, tenant, fromDate, toDate);

      var points = GetSeriesByUser(operations);

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
                  Name = "Количество операций создания",
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("UserCreateOperations", operations);
    }

    public ActionResult UserEditOperations(string tenant, DateTime? fromDate = null, DateTime? toDate = null)
    {
      var operations = Repository.GetOpertaionRecords()
        .Where(x => x.OperationName.ToLower().Contains("edit"));

      operations = OperationRecord.GlobalFilters(operations, tenant, fromDate, toDate);

      var points = GetSeriesByUser(operations);

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
                  Name = "Количество операций редактирования",
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("UserEditOperations", operations);
    }

    public ActionResult UserDocumentBodyOperations(string tenant, DateTime? fromDate = null, DateTime? toDate = null)
    {
      var operations = Repository.GetOpertaionRecords()
        .Where(x => x.OperationName.ToLower().Contains("open document to"));

      operations = OperationRecord.GlobalFilters(operations, tenant, fromDate, toDate);

      var points = GetSeriesByUser(operations);

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
                  Name = "Количество открытий тел документов",
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("UserDocumentBodyOperations", operations);
    }

    private Point[] GetSeriesByUser(IEnumerable<OperationRecord> operations)
    {
      var group = operations.ToList().GroupBy(x => x.User)
        .Select(x => new Point
        {
          Name = x.Key,
          Y = x.Count(),
          Events = new PlotOptionsSeriesPointEvents { Click =
            "function() {" + 
            "var tenant = $('#tenant')[0].options[$('#tenant')[0].selectedIndex].value;" +
            $"return $.get('/UserwiseOperations/CreateOperationsByUser?tenant=' + tenant + '&user={x.Key}').html(); " +
            "}"
          }
        }).ToArray();

      return group;
    }
  }
}