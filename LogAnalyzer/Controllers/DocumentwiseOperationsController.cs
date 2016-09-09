using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
      var operationNamePart = "createentity";
      var operations = Repository.GetOpertaionRecords(tenant, fromDate, toDate)
        .Where(x => interfaces.Contains(x.EntityType))
        .Where(x => x.OperationName.ToLower().Contains(operationNamePart));

      var points = SharedHelpers.GetSeriesByEntityType(operations);
      foreach (var point in points)
      {
        point.Events = new PointEvents()
        {
          Click = DetalizationByOperationName(operationNamePart, true, point.Name)
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
                  Name = Resources.Resources.CreatedDocuments,
                  Data = new Data(points),
                });

      ViewBag.Chart = chart;
      return PartialView("CreateOperationStatisticsChart", operations);
    }

    public ActionResult EditOperationStatisticsChart(string tenant = "undefined", DateTime? fromDate = null, DateTime? toDate = null)
    {
      var types = EntityTypes.GetDocumentTypes().ToList();
      var operationNamePart = "open document to edit";
      var operations = Repository.GetOpertaionRecords(tenant, fromDate, toDate)
        .Where(x => types.Contains(x.OperationObjectType))
        .Where(x => x.OperationName.ToLower() == operationNamePart);

      var points = SharedHelpers.GetSeriesByOperationObjectType(operations);
      foreach (var point in points)
      {
        point.Events = new PointEvents()
        {
          Click = DetalizationByOperationName(operationNamePart, true, point.Name)
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
                  Name = Resources.Resources.EditedDocuments,
                  Data = new Data(points)
                });

      ViewBag.EditChart = chart;
      return PartialView("EditOperationStatisticsChart", operations);
    }

    private string DetalizationByOperationName(string operationNamePart = "", bool exactMatch = false, string entityType = "")
    {
      RouteValueDictionary parameters = new RouteValueDictionary();

      if (!string.IsNullOrEmpty(operationNamePart))
      {
        parameters.Add("operationName", operationNamePart);
        parameters.Add("exactMatch", exactMatch);
      }

      if (!string.IsNullOrEmpty(entityType))
        parameters.Add("entityType", entityType);

      return SharedHelpers.GenerateDetalizationLink(this.Url, parameters);
    }
  }
}