using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogAnalyzer.Models
{
  public class OperationRecord
  {
    public virtual long Id { get; set; }
    public virtual string OperationName { get; set; }
    public virtual DateTime Date { get; set; }
    public virtual string User { get; set; }
    public virtual long Duration { get; set; }
    public virtual string EntityType { get; set; }
    public virtual string Tenant { get; set; }
  }
}