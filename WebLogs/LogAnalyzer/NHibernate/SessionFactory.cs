using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LogAnalyzer.NHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace LogAnalyzer.NHibernate
{
  public static class SessionFactory
  {
    private static string _connectionString;
    private static ISessionFactory _sessionFactory;

    private static ISessionFactory CreateSessionFactory(string connectionString = @"Data Source = Ruslan-PC2\SQLEXPRESS; Initial Catalog = LogAnalyzer; User ID = admin; Password = 11111")
    {
      _connectionString = connectionString;
      return Fluently
        .Configure()
        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
        .Mappings(m => m.FluentMappings.Add<OperationRecordMap>())
        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
        .BuildSessionFactory();
    }

    public static ISession GetSession()
    {
      _sessionFactory = _sessionFactory ?? CreateSessionFactory();
      return _sessionFactory.OpenSession();
    }
  }

}