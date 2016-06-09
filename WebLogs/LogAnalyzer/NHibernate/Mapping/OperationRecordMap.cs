using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LogAnalyzer.Models;

namespace LogAnalyzer.NHibernate.Mapping
{
  public class OperationRecordMap : ClassMap<OperationRecord>
  {
    public OperationRecordMap()
    {
      Id(x => x.Id);
      Map(x => x.Date);
      Map(x => x.User).Column("`User`");
      Map(x => x.Duration);
      Map(x => x.OperationName);
      Map(x => x.EntityType);
      Map(x => x.Tenant);
      Map(x => x.OperationObjectType);
    }
  }
}