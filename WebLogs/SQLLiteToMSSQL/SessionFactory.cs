using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SQLLiteToMSSQL.Mappings;

namespace SQLLiteToMSSQL
{
  public class SessionFactory
  {
    //const string databaseName = @"D:\logs_fullday.db";
    const string databaseName = @"D:\logs_fullday_7.db";
    //private static ISessionFactory _sessionFactory;
    //private static ISession _session;

    public static ISessionFactory CreateSQLiteSessionFactory()
    {
      return Fluently
        .Configure()
        .Database(SQLiteConfiguration.Standard.UsingFile(databaseName))
        .Mappings(m => m.FluentMappings.Add<OperationRecordMap>())
        .BuildSessionFactory();
    }

    public static ISessionFactory CreateMSSQLSessionFactory(string connectionString)
    {
      return Fluently
        .Configure()
        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
        .Mappings(m => m.FluentMappings.Add<OperationRecordMap2>())
        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
        .BuildSessionFactory();
    }
  }
}
