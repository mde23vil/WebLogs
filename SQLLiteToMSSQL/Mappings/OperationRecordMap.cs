using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace SQLLiteToMSSQL.Mappings
{
  internal class OperationRecordMap : ClassMap<OperationRecord>
  {
    public OperationRecordMap()
    {
      Id(x => x.Id).Column("id");
      Map(x => x.LogDate).Column("log_date");
      Map(x => x.LogTime).Column("log_time");
      Map(x => x.Application).Column("application");
      Map(x => x.Version).Column("version");
      Map(x => x.HostName).Column("host");
      Map(x => x.User).Column("user");
      Map(x => x.ProcessId).Column("process"); // Сомневаюсь что нужен.
      Map(x => x.ThreadNumber).Column("thread"); // Сомневаюсь что нужен.
      Map(x => x.Tenant).Column("tenant");
      Map(x => x.LogLevel).Column("level");
      Map(x => x.Logger).Column("logger");
      Map(x => x.Details).Column("details");
      Map(x => x.StartTime).Column("start_time");
      Map(x => x.BadStatus).Column("bad_status"); // Enumeration?: failed, canceled
      Map(x => x.Duration).Column("duration");
      Map(x => x.OperationName).Column("operation_name");
      Map(x => x.OperationObjectType).Column("operation_object_type");
      Map(x => x.OperationObjectId).Column("operation_object_id");
      Map(x => x.FromCache).Column("from_cache");
      Map(x => x.FromCard).Column("from_card");
      Map(x => x.FromCardId).Column("from_card_id");
      Map(x => x.FromList).Column("from_list");
      Map(x => x.FromFolder).Column("from_folder");
      Map(x => x.FromFolderId).Column("from_folder_id");
      Map(x => x.BinaryDataGuid).Column("binary_data_guid");
      Map(x => x.BinaryDataSize).Column("binary_data_size");
      Map(x => x.BinaryLoadDuration).Column("binary_load_duration");
      Map(x => x.BinaryLoadStartTime).Column("binary_load_start_time");
      Map(x => x.DocumentVersion).Column("doc_version");
      Map(x => x.SaveCardWithBinary).Column("save_card_with_binary");
      Map(x => x.EntityType).Column("entity_type");
      Map(x => x.EntitiesCount).Column("entities_count");
      Map(x => x.Source).Column("source");
      Table("Operation");
    }
  }

  internal class OperationRecordMap2 : ClassMap<OperationRecord>
  {
    public OperationRecordMap2()
    {
      Id(x => x.Id);
      Map(x => x.Date);
      Map(x => x.Application);
      Map(x => x.Version);
      Map(x => x.HostName);
      Map(x => x.User).Column("`User`");
      Map(x => x.Tenant);
      Map(x => x.LogLevel);
      Map(x => x.Logger);
      Map(x => x.Details);
      Map(x => x.StartTime);
      Map(x => x.BadStatus); // Enumeration?: failed, canceled
      Map(x => x.Duration);
      Map(x => x.OperationName);
      Map(x => x.OperationObjectType);
      Map(x => x.OperationObjectId);
      Map(x => x.FromCache);
      Map(x => x.FromCard);
      Map(x => x.FromCardId);
      Map(x => x.FromList);
      Map(x => x.FromFolder);
      Map(x => x.FromFolderId);
      Map(x => x.BinaryDataGuid);
      Map(x => x.BinaryDataSize);
      Map(x => x.BinaryLoadDuration);
      Map(x => x.BinaryLoadStartTime);
      Map(x => x.DocumentVersion);
      Map(x => x.SaveCardWithBinary);
      Map(x => x.EntityType);
      Map(x => x.EntitiesCount);
      Map(x => x.Source).Length(4001); // Everything that larger than 4000 is nvarchar(max).
    }
  }
}
