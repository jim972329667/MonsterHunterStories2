using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;


namespace MonsterHunterStories2
{
    /// <summary>
    /// AddDB.xaml 的交互逻辑
    /// </summary>
    public partial class AddDB : Window
    {
        private List<KeyValuesInfo> Items { get; set; } = new List<KeyValuesInfo>();

        public static Dictionary<uint, uint> GeneID = new Dictionary<uint, uint>();
        public AddDB()
        {
            InitializeComponent();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(AddDBList.SelectedItem != null)
            {
                string DBName = AddDBList.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                string filename = "TXT File|" + DBName.Substring(0, DBName.Length - 1) + ".txt";
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".txt",
                    Filter = filename
                };
                if (dlg.ShowDialog() != false)
                {
                    if(DBName == "GenePNGIDs")
                    {
                        AppendDic(dlg.FileName);
                        DataBase.AddDB_PNGID(DBName, GeneID);
                        GeneID.Clear();
                    }
                    else
                    {
                        AppendList(dlg.FileName);
                        DataBase.AddDB(DBName, Items);
                        Items.Clear();
                    }
                    MessageBox.Show("Success!");
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
        private static void AppendDic(string FileName)
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
                GeneID.Add(result1, result2);
            }
        }
    }
}
