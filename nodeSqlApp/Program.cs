using System;
using System.Linq;
using Dapper;
using DapperExtensions;
using Microsoft.Data.Sqlite;
using System.Text.Json;
using DapperExtensions.Mapper;

namespace nodeSqlApp
{
    class Program
    {
        public class BinancePriceModelMap : ClassMapper<BinancePriceModel>
        {
            public BinancePriceModelMap()
            {
                base.Table("BinancePriceTable");
                base.AutoMap();
              
            }
        }
        public class BinancePriceModel  
        {
            public string payload { get; set; }
            public string topic { get; set; } 
            public string _msgid { get; set; }
        }
    
        static void _writeJson(string jsonStr)
        {

            SqliteConnectionStringBuilder b = new SqliteConnectionStringBuilder();
            
            b.DataSource = clsFile._getExePath_replace("identifier.sqlite");

            SqliteConnection con = new SqliteConnection(b.ConnectionString);
            //SQL形式で書き出す
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqliteDialect();

            DapperExtensions.DapperExtensions.DefaultMapper = typeof(BinancePriceModelMap);
            
            con.Open();
            //dapperで_を有効にする
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            //外部キーの設定
            SqliteCommand command = con.CreateCommand();
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();

            BinancePriceModel BinancePriceModel1 = JsonSerializer.Deserialize<BinancePriceModel>(jsonStr);
            
            con.Insert<BinancePriceModel>(BinancePriceModel1);

            string sql = "select * from BinancePriceTable;";
            var testResult = con.QueryAsync<BinancePriceModel>(sql);
            
            //Console.Clear();
            Console.WriteLine("書き込み数" + testResult.Result.Count());
            
            con.Close();
        }

        static void Main(string[] args)
        {

            //デーコード
            var jesonStr = System.Web.HttpUtility.UrlDecode(args[0]);

            _writeJson(jesonStr);

        }
    }
}