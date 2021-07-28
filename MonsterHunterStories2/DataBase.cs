using LiteDB;
using System.Collections.Generic;
using System.Windows;

namespace MonsterHunterStories2
{

    public class DB_GenePNGID
    {
        public uint ID { get; set; }
        public uint PNGID { get; set; }
        
    }
    public class DB_General
    {
        public uint ID { get; set; }
        public string[] LanguageList { get; set; }
    }
    class DataBase
    {
        public static string Path = @".\info\text.db";
        public static LiteDatabase db = new LiteDatabase(Path);
        public static void Init()
        {

        }
        public static void AddDB(string DBName , List<KeyValuesInfo> items)
        {
            var col = db.GetCollection<DB_General>(DBName);
            foreach (KeyValuesInfo x in items)
            {
                var customer = new DB_General
                {
                    ID = x.Key,
                    LanguageList = x.Values
                };
                col.EnsureIndex(a => a.ID, true);
                try
                {
                    col.Insert(customer);
                }
                catch
                {
                    col.Update(customer);
                }
            }

        }
        public static void AddDB_PNGID(string DBName, Dictionary<uint,uint> GeneID)
        {
            var col = db.GetCollection<DB_GenePNGID>(DBName);
            foreach(var x in GeneID)
            {
                var customer = new DB_GenePNGID
                {
                    ID = x.Key,
                    PNGID = x.Value
                };
                col.EnsureIndex(a => a.ID, true);
                try
                {
                    col.Insert(customer);
                }
                catch
                {
                    col.Update(customer);
                }
            }
        }
        public static uint GetPNGID(uint ID)
        {
            var table = db.GetCollection<DB_GenePNGID>("GenePNGIDs");
            var info = table.FindOne(a => a.ID == ID);//Linq表达式
            if (info == null)
            {
                //MessageBox.Show("丢失数据库");
                return 0;
            }
            return info.PNGID;


        }
        public static string GetConver(uint ID, string DBName)
        {
            var table = db.GetCollection<DB_General>(DBName);
            var info = table.FindOne(a => a.ID == ID);//Linq表达式
            int x = Properties.Settings.Default.Langage;
            if (info == null)
            {
                //MessageBox.Show("丢失数据库");
                return null;
            }
            if (x >= info.LanguageList.Length) x = 1;

            return info.LanguageList[x];
        }
    }
}
