using LiteDB;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MonsterHunterStories2
{
    class DataBase
    {
        private class DB_GenePNGID
        {
            [BsonId]
            public uint ID { get; set; }
            public uint PNGID { get; set; }

        }
        private class DB_General
        {
            [BsonId]
            public uint ID { get; set; }
            public string[] LanguageList { get; set; }
        }
        public class ConverList
        {
            public uint Key { get; set; }
            public string Value { get; set; }
        }

        public static string Path = @".\info\MHS2.db";

        private static LiteDatabase db = new LiteDatabase("Filename = info\\MHS2.db; ReadOnly=true");
        //private static LiteDatabase db = new LiteDatabase(Path);

        private static readonly string[] DBTABLE = new string[]
        {
            "ItemDescriptions",
            "GenePNGIDs",
            "GeneSkills",
            "Genes",
            "Talisman_skills",
            "Items",
            "Monsters",
            "Rides"
        };
        public static void Init()
        {
            
        }
        public static void AddDB(string DBName , List<KeyValuesInfo> items)
        {
            db.Dispose();
            var newdb = new LiteDatabase(Path);
            var col = newdb.GetCollection<DB_General>(DBName);
            foreach (KeyValuesInfo x in items)
            {
                var customer = new DB_General
                {
                    ID = x.Key,
                    LanguageList = x.Values
                };
                col.Upsert(customer);
            }
            newdb.Dispose();
            db = new LiteDatabase("Filename = info\\MHS2.db; ReadOnly=true");

        }
        public static void AddDB_PNGID(string DBName, Dictionary<uint,uint> GeneID)
        {
            db.Dispose();
            var newdb = new LiteDatabase(Path);
            var col = newdb.GetCollection<DB_GenePNGID>(DBName);
            foreach(var x in GeneID)
            {
                var customer = new DB_GenePNGID
                {
                    ID = x.Key,
                    PNGID = x.Value
                };
                col.Upsert(customer);
            }
            newdb.Dispose();
            db = new LiteDatabase("Filename = info\\MHS2.db; ReadOnly=true");
        }
        public static uint GetPNGID(uint ID)
        {
            var table = db.GetCollection<DB_GenePNGID>("GenePNGIDs");
            var info = table.FindOne(a => a.ID == ID);//Linq表达式
            if (info == null)
            {
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
                return null;
            }
            if (x >= info.LanguageList.Length) x = 1;

            return info.LanguageList[x];
        }
        public static List<ConverList> GetConverList(string DBName)
        {
            var ReturnList = new List<ConverList>();
            int x = Properties.Settings.Default.Langage;
            var info = db.GetCollection<DB_General>(DBName).FindAll();
            foreach (DB_General i in info)
            {
                if (x >= i.LanguageList.Length) x = 1;
                ConverList list = new ConverList()
                {
                    Key = i.ID,
                    Value = i.LanguageList[x]
                };
                ReturnList.Add(list);
            }
            return ReturnList;
        }
        public static string ChickDB()
        {
            string LosingTXT = null;
            for (int i = 0; i < DBTABLE.Length; i++)
            {
                if(!db.CollectionExists(DBTABLE[i]))
                {
                    LosingTXT += "丢失数据：" + DBTABLE[i].Substring(0, DBTABLE[i].Length - 1) + ".txt\n";
                }
            }
            return LosingTXT;
        }
    }
}
