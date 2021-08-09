using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;

namespace MonsterHunterStories2
{
    /// <summary>
    /// AddDB.xaml 的交互逻辑
    /// </summary>
    public partial class AddDB : Window
    {
        private List<KeyValuesInfo> Items { get; set; } = new List<KeyValuesInfo>();
        private static List<DataBase.DB_GenePNGID> GeneID = new List<DataBase.DB_GenePNGID>();
        private static readonly Dictionary<string, uint> WeaponType = new Dictionary<string, uint>()
        {
            {"greatsword.txt",0 },
            {"swordshield.txt",1 },
            {"hammer.txt",2 },
            {"huntinghorn.txt",3 },
            {"gunlance.txt",4 },
            {"bow.txt",5 }
        };
        public AddDB()
        {
            InitializeComponent();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(AddDBList.SelectedItem != null)
            {
                string DBName = AddDBList.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                if(DBName == "Weapons")
                {
                    OpenFileDialog dlg = new OpenFileDialog
                    {
                        Multiselect = true,
                        DefaultExt = ".txt",
                        Filter = "TXT File|greatsword.txt;swordshield.txt;hammer.txt;huntinghorn.txt;gunlance.txt;bow.txt"
                    };
                    if (dlg.ShowDialog() != false)
                    {
                        string[] files = dlg.FileNames;
                        string[] filesName = dlg.SafeFileNames;

                        for (uint i = 0; i < files.Length; i++)
                        {
                            AppendList(files[i]);
                            Items.Sort();
                            DataBase.AddDB_Weapon(DBName, Items, WeaponType[filesName[i].ToLower()]);
                            Items.Clear();
                        }
                        MessageBox.Show(Properties.Resources.MessageSuccess);
                    }
                }
                else
                {
                    string filename = "TXT File|" + DBName.Substring(0, DBName.Length - 1) + ".txt";
                    OpenFileDialog dlg = new OpenFileDialog
                    {
                        DefaultExt = ".txt",
                        Filter = filename
                    };
                    if (dlg.ShowDialog() != false)
                    {
                        if (DBName == "GenePNGIDs")
                        {
                            AppendGenePNGID(dlg.FileName);
                            DataBase.AddDB_PNGID(DBName, GeneID);
                            GeneID.Clear();
                        }
                        else
                        {
                            AppendList(dlg.FileName);
                            DataBase.AddDB(DBName, Items);
                            Items.Clear();
                        }
                        MessageBox.Show(Properties.Resources.MessageSuccess);
                    }
                } 
            }
        }
        private void AppendList(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                string[] lines = System.IO.File.ReadAllLines(filename);
                foreach (string line in lines)
                {
                    if (line.Length < 3) continue;
                    if (line[0] == '#') continue;
                    string[] values = line.Split('\t');
                    if (values.Length < 2) continue;
                    if (string.IsNullOrEmpty(values[0])) continue;
                    KeyValuesInfo type = new KeyValuesInfo();
                    if (type.Line(values))
                    {
                        Items.Add(type);
                    }
                }
            }
        }
        //private void AppendList<Type>(string filename, List<Type> items)
        //    where Type : ILineAnalysis, new()
        //{
        //    if (!System.IO.File.Exists(filename)) return;
        //    string[] lines = System.IO.File.ReadAllLines(filename);
        //    foreach (string line in lines)
        //    {
        //        if (line.Length < 3) continue;
        //        if (line[0] == '#') continue;
        //        string[] values = line.Split('\t');
        //        if (values.Length < 2) continue;
        //        if (string.IsNullOrEmpty(values[0])) continue;

        //        Type type = new Type();
        //        if (type.Line(values))
        //        {
        //            items.Add(type);
        //        }
        //    }
        //    items.Sort();
        //}
        private static void AppendGenePNGID(string FileName)
        {
            if (!System.IO.File.Exists(FileName)) return;
            string[] lines = System.IO.File.ReadAllLines(FileName);
            foreach (string line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                string[] values = line.Split('\t');
                if (values.Length < 2) continue;
                if (string.IsNullOrEmpty(values[0]) || string.IsNullOrEmpty(values[1])) continue;
                _ = uint.TryParse(values[0], out uint result1);
                _ = uint.TryParse(values[1], out uint result2);
                if(!bool.TryParse(values[2], out bool result3)) result3 = false;
                DataBase.DB_GenePNGID returnDB = new DataBase.DB_GenePNGID()
                {
                    ID = result1,
                    PNGID = result2,
                    Legit = result3
                };
                GeneID.Add(returnDB);
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUpdate.IsChecked == true)
            {
                string message = Properties.Resources.MessageCheckUpdate;
                string caption = Properties.Resources.MessageWarning;
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result = MessageBox.Show(this, message, caption, buttons, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Properties.Settings.Default.IsUpdate = true;
                    var x = Properties.Settings.Default.IsUpdate;
                }
                else
                {
                    CheckUpdate.IsChecked = false;
                    Properties.Settings.Default.IsUpdate = false;
                }
            }
        }
    }
}
