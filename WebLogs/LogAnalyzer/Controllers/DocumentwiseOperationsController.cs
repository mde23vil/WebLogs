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
using LogAnalyzer.Filters;
using LogAnalyzer.Helpers;
using LogAnalyzer.Models;
using LogAnalyzer.NHibernate;
using LogAnalyzer.ViewModel;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Linq;

namespace LogAnalyzer.Controllers
{
  [Culture]
  public class DocumentwiseOperationsController : Controller
  {
    // GET: Operations
    public ActionResult Index()
    {
      ViewBag.DetailsPageSubTitle = Resources.Resources.ByDocumentTypesHeader;
      ViewBag.Tenants = Tenants.ObtainTenantList();
      ViewData["Tenants"] = SharedHelpers.StringsToSelectList(ViewBag.Tenants);

      return View(Repository.GetOpertaionRecords());
    }
    
    public ActionResult CreateOperationStatisticsChart(string tenant = "undefined", DateTime? fromDate = null, DateTime? toDate = null)
    {
      var interfaces = EntityTypes.GetDocumentInterfaces().ToList();
      var operations = Repository.GetOpertaionRecords()
        .Where(x => interfaces.Contains(x.EntityType))
        .Where(x => x.OperationName.ToLower().Contains("create"));

      operations = OperationRecord.GlobalFilters(operations, tenant, fromDate, toDate);
      var points = GetSeriesByEntityType(operations);

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
                  Name = Resources.Resources.CreatedDocuments,
                  Data = new Data(points)
                });

      ViewBag.Chart = chart;
      return PartialView("CreateOperationStatisticsChart", operations);
    }
    
    public ActionResult EditOperationStatisticsChart(string tenant = "undefined", DateTime? fromDate = null, DateTime? toDate = null)
    {
      var types = EntityTypes.GetDocumentTypes().ToList();
      var operations = Repository.GetOpertaionRecords()
        .Where(x => types.Contains(x.OperationObjectType))
        .Where(x => x.OperationName.ToLower() == "open document to edit");

      operations = OperationRecord.GlobalFilters(operations, tenant, fromDate, toDate);
      var points = SharedHelpers.GetSeriesByOperationObjectType(operations);

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
                  Name = Resources.Resources.EditedDocuments,
                  Data = new Data(points)
                });

      ViewBag.EditChart = chart;
      return PartialView("EditOperationStatisticsChart", operations);
    }

    private static Point[] GetSeriesByEntityType(IEnumerable<OperationRecord> operations)
    {
      var group = operations.ToList().GroupBy(x => x.EntityType)
        .Select(x => new Point
        {
          Name = x.Key,
          Y = x.Count(),
        }).ToArray();

      return group;
    }
  }
}