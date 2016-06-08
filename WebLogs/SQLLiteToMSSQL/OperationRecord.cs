using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace SQLLiteToMSSQL
{
  public class OperationRecord
  {
    private DateTime _startTime;
    private DateTime _date;
    private DateTime _binaryLoadStartTime;

    public virtual long Id { get; protected set; }
    public virtual DateTime Date
    {
      get
      {
        return DateTime.Parse(this.LogDate + " " + this.LogTime);
      }
      set { _date = value; }
    }

    public virtual string LogDate { get; set; }
    public virtual string LogTime { get; set; }
    public virtual string Application { get; set; }
    public virtual string Version { get; set; }
    public virtual string HostName { get; set; }
    public virtual string User { get; set; }
    public virtual int ProcessId { get; set; } // Сомневаюсь что нужен.
    public virtual int ThreadNumber { get; set; } // Сомневаюсь что нужен.
    public virtual string Tenant { get; set; }
    public virtual string LogLevel { get; set; }
    public virtual string Logger { get; set; }
    public virtual string Details { get; set; }
    public virtual DateTime StartTime
    {
      get
      {
        return _startTime.Year < 1753 ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : _startTime;
      }
      set { _startTime = value; }
    }
    public virtual string BadStatus { get; set; } // Enumeration?: failed, canceled
    public virtual long Duration { get; set; }
    public virtual string OperationName { get; set; }
    public virtual string OperationObjectType { get; set; }
    public virtual string OperationObjectId { get; set; }
    public virtual bool FromCache { get; set; }
    public virtual string FromCard { get; set; }
    public virtual int FromCardId { get; set; }
    public virtual string FromList { get; set; }
    public virtual string FromFolder { get; set; }
    public virtual int FromFolderId { get; set; }
    public virtual string BinaryDataGuid { get; set; }
    public virtual long BinaryDataSize { get; set; }
    public virtual int BinaryLoadDuration { get; set; }
    public virtual DateTime BinaryLoadStartTime
    {
      get
      {
        return _binaryLoadStartTime.Year < 1753 ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : _startTime;
      }
      set { _binaryLoadStartTime = value; }
    }
    public virtual int DocumentVersion { get; set; }
    public virtual bool SaveCardWithBinary { get; set; }
    public virtual string EntityType { get; set; }
    public virtual int EntitiesCount { get; set; }
    public virtual string Source { get; set; }
  }
}
