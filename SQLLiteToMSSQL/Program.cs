using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SQLite;
using NHibernate.Linq;
using SQLLiteToMSSQL;

namespace SQLiteToMSSQL
{
  class Program
  {

    static void Main(string[] args)
    {
      /*const string databaseName = @"D:\logs_fullday.db";
      var tables = new List<DataTable>();
      using (var sqliteReader = new SQLiteReader(databaseName))
      {
        tables = sqliteReader.GetData();
      }
      var mssqlWriter = new MSSQLWriter(ConfigurationManager.AppSettings["MSSQLConnectionString"]);
      mssqlWriter.WriteTables(tables);
      Console.WriteLine("Done");*/
      var operations = new List<OperationRecord>();

      using (var session = SessionFactory.CreateSQLiteSessionFactory().OpenSession())
      {
        operations = session.Query<OperationRecord>().ToList();
        operations.ForEach(x => session.Evict(x));
        Console.ReadLine();
      }

      using (
        var session =
          SessionFactory.CreateMSSQLSessionFactory(ConfigurationManager.AppSettings["MSSQLConnectionString"])
            .OpenSession())
      {
        using (var transaction = session.BeginTransaction())
        {
          foreach (var operationRecord in operations)
          {
            session.Save(operationRecord);
          }
          transaction.Commit();
        }
      }


      return;

      /*Console.WriteLine("Creating tables...");
      using (
        SqlConnection msSqlConnection =
          new SqlConnection(@"Data Source = Ruslan-PC2\SQLEXPRESS; Initial Catalog = LogAnalyzer; User ID = sa; Password = 031291")
        )
      {
        msSqlConnection.Open();
        foreach (var table in tables)
        {
          if (table.TableName != "Operation")
            continue;
          var msSqlCommand = msSqlConnection.CreateCommand();
          var commandString = $@"CREATE TABLE {table.TableName} ";
          commandString += "(";
          var columns = table.Columns;
          for (int i=0; i < columns.Count; i++)
          {
            commandString += $" [{columns[i].ColumnName}] {netToMSSQLTypesMapping[columns[i].DataType]},";
          }
          commandString = commandString.Substring(0, commandString.Length - 1);
          commandString += ")";
          msSqlCommand.CommandText = commandString;
          msSqlCommand.ExecuteNonQuery();
        }
        msSqlConnection.Close();
      }
      Console.WriteLine("Done");
      Console.ReadKey(true);*/
    }
  }
}
