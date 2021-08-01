using LiteDB;
using System;
using System.Collections.Generic;

namespace MonsterHunterStories2
{
    class DataBase
    {
        public class DB_General
        {
            [BsonId]
            public uint ID { get; set; }
            public string[] LanguageList { get; set; }
        }
        public class DB_GenePNGID
        {
            [BsonId]
            public uint ID { get; set; }
            public uint PNGID { get; set; }

        }
        public class KeyWeapon
        {
            public uint ID { get; set; }
            public uint Type { get; set; }

        }
        public class DB_Equipment
        {
            [BsonId]
            public KeyWeapon Keys { get; set; }
            public string[] LanguageList { get; set; }
        }
        public class ConverList : IComparable
        {
            public uint Key { get; set; }
            public string Value { get; set; }
            public int CompareTo(object obj)
            {
                var dist = obj as ConverList;
                if (dist == null) return 0;

                if (Key < dist.Key) return -1;
                else if (Key > dist.Key) return 1;
                else return 0;
            }
        }
        public class EquipmentConverList : IComparable
        {
            public uint Key { get; set; }
            public uint Type { get; set; }
            public string Value { get; set; }
            public int CompareTo(object obj)
            {
                var dist = obj as EquipmentConverList;
                if (dist == null) return 0;

                if (Key < dist.Key) return -1;
                else if (Key > dist.Key) return 1;
                else return 0;
            }
        }

        public static string Path = @".\info\MHS2.db";

        public static LiteDatabase db = new LiteDatabase("Filename = info\\MHS2.db; ReadOnly=true");
        
        private static readonly string[] DBTABLE = new string[]
        {
            "ItemDescriptions",
            "GenePNGIDs",
            "GeneSkills",
            "Genes",
            "Talisman_skills",
            "Items",
            "Monsters",
            "Rides",
            "Weapons",
            "Armors",
            "Talismans"
        };
        private static int GetMax(int num1, int num2)
        {
            return num1 > num2 ? num1 : num2;
        }
        public static void AddDB(string DBName , List<KeyValuesInfo> items)
        {
            db.Dispose();
            var newdb = new LiteDatabase(Path);
            var col = newdb.GetCollection<DB_General>(DBName);
            foreach (KeyValuesInfo x in items)
            {
                var old = col.FindOne(a => a.ID == x.Key);
                if(old != null)
                {
                    int maxlength = GetMax(old.LanguageList.Length, x.Values.Length);
                    string[] returnvalue = new string[maxlength];
                    for (int i = 0; i < maxlength; i++)
                    {
                        if (i < x.Values.Length && i < old.LanguageList.Length)
                        {
                            if (x.Values[i] == "" || x.Values[i] == null)
                                returnvalue[i] = old.LanguageList[i];
                            else returnvalue[i] = x.Values[i];
                        }
                        else if (i >= x.Values.Length && i < old.LanguageList.Length)
                        {
                            returnvalue[i] = old.LanguageList[i];
                        }
                        else
                        {
                            returnvalue[i] = x.Values[i];
                        }
                    }
                    var customer = new DB_General
                    {
                        ID = x.Key,
                        LanguageList = returnvalue
                    };
                    col.EnsureIndex(a => a.ID);
                    col.Upsert(customer);
                }
                else
                {
                    var customer = new DB_General
                    {
                        ID = x.Key,
                        LanguageList = x.Values
                    };
                    col.EnsureIndex(a => a.ID);
                    col.Upsert(customer);
                }    
            }
            newdb.Dispose();
            db = new LiteDatabase("Filename = info\\MHS2.db; ReadOnly=true");

        }
        public static void AddDB_Weapon(string DBName, List<KeyValuesInfo> items, uint type)
        {
            db.Dispose();
            var newdb = new LiteDatabase(Path);
            var col = newdb.GetCollection<DB_Equipment>(DBName);
            foreach (KeyValuesInfo x in items)
            {
                var oldkey = new KeyWeapon
                {
                    ID = x.Key,
                    Type = type
                };
                var old = col.FindOne(a => a.Keys == oldkey);
                
                if (old != null)
                {
                    int maxlength = GetMax(old.LanguageList.Length, x.Values.Length);
                    string[] returnvalue = new string[maxlength];
                    for (int i = 0; i < maxlength; i++)
                    {
                        if (i < x.Values.Length && i < old.LanguageList.Length)
                        {
                            if (x.Values[i] == "" || x.Values[i] == null)
                                returnvalue[i] = old.LanguageList[i];
                            else returnvalue[i] = x.Values[i];
                        }
                        else if(i >= x.Values.Length && i < old.LanguageList.Length)
                        {
                            returnvalue[i] = old.LanguageList[i];
                        }
                        else
                        {
                            returnvalue[i] = x.Values[i];
                        }
                    }
                    var customer = new DB_Equipment
                    {
                        Keys = oldkey,
                        LanguageList = returnvalue
                    };
                    col.EnsureIndex(a => a.Keys);
                    col.Upsert(customer);
                }
                else
                {
                    var customer = new DB_Equipment
                    {
                        Keys = new KeyWeapon
                        {
                            ID = x.Key,
                            Type = type
                        },
                        LanguageList = x.Values
                    };
                    col.EnsureIndex(a => a.Keys);
                    col.Upsert(customer);
                }
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
                col.EnsureIndex(a => a.ID);
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
        public static string GetWeaponConver(uint ID, uint Type)
        {
            var table = db.GetCollection<DB_Equipment>("Weapons");
            var WeaponKey = new KeyWeapon
            {
                ID = ID,
                Type = Type
            };
            var info = table.FindOne(a => a.Keys == WeaponKey);//Linq表达式
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
            ReturnList.Sort();
            return ReturnList;
        }
        public static List<ConverList> GetWeaponConverList(string DBName, uint type)
        {
            var ReturnList = new List<ConverList>();
            int x = Properties.Settings.Default.Langage;
            var info = db.GetCollection<DB_Equipment>(DBName).FindAll();
            foreach (DB_Equipment i in info)
            {
                if (x >= i.LanguageList.Length) x = 1;
                if(i.Keys.Type == type)
                {
                    ConverList list = new ConverList()
                    {
                        Key = i.Keys.ID,
                        Value = i.LanguageList[x]
                    };
                    ReturnList.Add(list);
                }
            }
            ReturnList.Sort();
            return ReturnList;
        }
        public static string ChickDB()
        {
            string LosingTXT = null;
            for (int i = 0; i < DBTABLE.Length; i++)
            {
                if(!db.CollectionExists(DBTABLE[i]))
                {
                    LosingTXT += Properties.Resources.ErrorLosingTXTFile + DBTABLE[i].Substring(0, DBTABLE[i].Length - 1) + ".txt\n";
                }
            }
            return LosingTXT;
        }
        public static void UpdateDB_Weapon(List<DB_Equipment> items)
        {
            int language = Properties.Settings.Default.Langage;
            db.Dispose();
            var newdb = new LiteDatabase(Path);
            var col = newdb.GetCollection<DB_Equipment>("Weapons");
            foreach (DB_Equipment x in items)
            {
                var old = col.FindOne(a => a.Keys == x.Keys);
                if (old == null)
                {
                    col.EnsureIndex(a => a.Keys);
                    col.Upsert(x);
                }
                else
                {
                    if (x.LanguageList.Length <= language) language = 0;
                    if (old.LanguageList[language] == "" || old.LanguageList[language] == null)
                    {
                        col.EnsureIndex(a => a.Keys);
                        col.Upsert(x);
                    }
                }
            }
            newdb.Dispose();
            db = new LiteDatabase("Filename = info\\MHS2.db; ReadOnly=true");
        }
        public static void UpdateDB_General(string DBName, List<DB_General> items)
        {
            int language = Properties.Settings.Default.Langage;
            db.Dispose();
            var newdb = new LiteDatabase(Path);
            var col = newdb.GetCollection<DB_General>(DBName);
            foreach (DB_General x in items)
            {
                var old = col.FindOne(a => a.ID == x.ID);
                if (old == null)
                {
                    col.EnsureIndex(a => a.ID);
                    col.Upsert(x);
                }
                else
                {
                    if (x.LanguageList.Length <= language) language = 0;
                    if (old.LanguageList[language] == ""|| old.LanguageList[language] == null)
                    {
                        col.EnsureIndex(a => a.ID);
                        col.Upsert(x);
                    }
                }
            }
            newdb.Dispose();
            db = new LiteDatabase("Filename = info\\MHS2.db; ReadOnly=true");
        }
        public static void GetUpdate(bool IsOpen)
        {
            string[] WeaponType = new string[]
            {
                Properties.Resources.MainEquipmentWeaponGS,
                Properties.Resources.MainEquipmentWeaponSS,
                Properties.Resources.MainEquipmentWeaponH,
                Properties.Resources.MainEquipmentWeaponHH,
                Properties.Resources.MainEquipmentWeaponG,
                Properties.Resources.MainEquipmentWeaponB
            };
            if (IsOpen && Properties.Settings.Default.IsUpdate)
            {
                int MaxLanguage = MainWindow.MaxLanguage;
                int language = Properties.Settings.Default.Langage;
                string[] names = new string[MaxLanguage];
                List<DB_General> items = new List<DB_General>();
                List<DB_Equipment> weapons = new List<DB_Equipment>();
                ViewModel viewmodel = new ViewModel();
                var Weapondata = viewmodel.Weapons;
                foreach (Weapon NewWeapon in Weapondata)
                {
                    if (NewWeapon.Type <= 5)
                    {
                        string name = Properties.Resources.ErrorUnknow + WeaponType[NewWeapon.Type] + ": " + NewWeapon.ID.ToString();
                        names[language] = name;
                        var ReturnItem = new DB_Equipment
                        {
                            Keys = new KeyWeapon
                            {
                                ID = NewWeapon.ID,
                                Type = NewWeapon.Type
                            },
                            LanguageList = names
                        };
                        weapons.Add(ReturnItem);
                        names = new string[MaxLanguage];
                    }
                }
                UpdateDB_Weapon(weapons);
                weapons.Clear();
                //Update Items
                var Itemdata = viewmodel.Items;
                foreach(Item NewItem in Itemdata)
                {
                    string name = Properties.Resources.ErrorUnknowItem + ": " + NewItem.ID.ToString();
                    names[language] = name;
                    var ReturnItem = new DB_General
                    {
                        ID = NewItem.ID,
                        LanguageList = names
                    };
                    items.Add(ReturnItem);
                    names = new string[MaxLanguage];
                }
                UpdateDB_General("Items", items);
                items.Clear();
                var Monsterdata = viewmodel.Monsters;
                foreach(Monster NewMonster in Monsterdata)
                {
                    string name = Properties.Resources.ErrorUnknowMonster + ": " + NewMonster.ID.ToString();
                    names[language] = name;
                    var ReturnItem = new DB_General
                    {
                        ID = NewMonster.ID,
                        LanguageList = names
                    };
                    items.Add(ReturnItem);
                    names = new string[MaxLanguage];
                }
                UpdateDB_General("Monsters", items);
                items.Clear();
                //Update Armors
                var Armordata = viewmodel.Armors;
                foreach (Armor NewArmor in Armordata)
                {
                    if (NewArmor.ID != 0)
                    {
                        string name = Properties.Resources.ErrorUnknowArmor + ": " + NewArmor.ID.ToString();
                        names[language] = name;
                        var ReturnItem = new DB_General
                        {
                            ID = NewArmor.ID,
                            LanguageList = names
                        };
                        items.Add(ReturnItem);
                        names = new string[MaxLanguage];
                    }
                }
                UpdateDB_General("Armors", items);
                items.Clear();
                //Update Talismans
                var Talismandata = viewmodel.Talismans;
                foreach (Talisman NewTalisman in Talismandata)
                {
                    if (NewTalisman.ID != 0)
                    {
                        string name = Properties.Resources.ErrorUnknowTalisman + ": " + NewTalisman.ID.ToString();
                        names[language] = name;
                        var ReturnItem = new DB_General
                        {
                            ID = NewTalisman.ID,
                            LanguageList = names
                        };
                        items.Add(ReturnItem);
                        names = new string[MaxLanguage];
                    }
                }
                UpdateDB_General("Talismans", items);
                items.Clear();
                //Update Talisman_skills
                foreach (Talisman NewTalisman in Talismandata)
                {
                    if (NewTalisman.ID != 0)
                    {
                        if(NewTalisman.Skill1 != 0 || NewTalisman.Skill2 != 0)
                        {
                            if(NewTalisman.Skill1 == 0)
                            {
                                string name = Properties.Resources.ErrorUnknowTalismanSkill + ": " + NewTalisman.Skill2.ToString();
                                names[language] = name;
                                var ReturnItem = new DB_General
                                {
                                    ID = NewTalisman.Skill2,
                                    LanguageList = names
                                };
                                items.Add(ReturnItem);
                                names = new string[MaxLanguage];
                            }
                            else if(NewTalisman.Skill2 == 0)
                            {
                                string name = Properties.Resources.ErrorUnknowTalismanSkill + ": " + NewTalisman.Skill1.ToString();
                                names[language] = name;
                                var ReturnItem = new DB_General
                                {
                                    ID = NewTalisman.Skill1,
                                    LanguageList = names
                                };
                                items.Add(ReturnItem);
                                names = new string[MaxLanguage];
                            }
                            else
                            {
                                string name = Properties.Resources.ErrorUnknowTalismanSkill + ": " + NewTalisman.Skill1.ToString();
                                names[language] = name;
                                var ReturnItem = new DB_General
                                {
                                    ID = NewTalisman.Skill1,
                                    LanguageList = names
                                };
                                items.Add(ReturnItem);
                                names = new string[MaxLanguage];
                                name = Properties.Resources.ErrorUnknowTalismanSkill + ": " + NewTalisman.Skill2.ToString();
                                names[language] = name;
                                ReturnItem = new DB_General
                                {
                                    ID = NewTalisman.Skill2,
                                    LanguageList = names
                                };
                                items.Add(ReturnItem);
                                names = new string[MaxLanguage];
                            }
                        } 
                    }
                }
                UpdateDB_General("Talisman_skills", items);
                items.Clear();
            }
        }
    }
}
