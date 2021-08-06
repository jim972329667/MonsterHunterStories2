using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using LiteDB;
using System.Linq;

namespace MonsterHunterStories2
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly byte[] eggx = System.Text.Encoding.Default.GetBytes("MHS2_EGGx");
		private static readonly int EggLength = eggx.Length % 4 == 0 ? eggx.Length : (eggx.Length / 4 * 4) + 4;
		public static int MaxLanguage = 0;
		public static bool IsOpen = false;

		public MainWindow()
		{
			Init();
			InitializeComponent();
			GetMaxLanguage();
		}
		private void Init()
        {
			//初始化图片
			GenePNG.Bitmaps();
			//检查数据库
			if (!File.Exists(@".\info\MHS2.db"))
			{
				MessageBox.Show(Properties.Resources.ErrorLosingDBFile);
				Directory.CreateDirectory(@".\info\");
				LiteDatabase db = new LiteDatabase(@".\info\MHS2.db");
				db.Dispose();
				if (DataBase.ChickDB() != null) MessageBox.Show(DataBase.ChickDB());
			}
			else
			{
				if (DataBase.ChickDB() != null) MessageBox.Show(DataBase.ChickDB());
			}
		}

		private void Window_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
            if (!(e.Data.GetData(DataFormats.FileDrop) is String[] files)) return;

            FileOpen(files[0]);
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			Properties.Settings.Default.Save();
			DataBase.GetUpdate(IsOpen);
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;
			IsOpen = true;
			FileOpen(dlg.FileName);
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			SaveData.Instance().Save();
		}

		private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;
			SaveData.Instance().SaveAs(dlg.FileName);
		}

		private void MenuItemFileExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new AboutWindow();
			dlg.ShowDialog();
		}
		private void MenuAddDB_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new AddDB();
			dlg.ShowDialog();
		}
		private void MenuFeedback_Click(object sender, RoutedEventArgs e)
        {
			System.Diagnostics.Process.Start("https://github.com/jim972329667/MonsterHunterStories2/issues");
		}

		private void ButtonChoiceItem_Click(object sender, RoutedEventArgs e)
		{
			bool Get = true;
			int num = 0;
			ChoiceWindow dlg = new ChoiceWindow
            {
                Type = ChoiceWindow.eType.TYPE_ITEM
            };
            dlg.ListBoxItem.SelectionMode = SelectionMode.Extended;
			dlg.ShowDialog();
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			if (dlg.DialogResult == false) return;
			foreach (DataBase.ConverList items in dlg.ListBoxItem.SelectedItems)
            {
                uint id = items.Key;
				
                for (int i = 0; i < viewmodel.Items.Count; i++)
				{
					if (viewmodel.Items[i].ID == id)
					{
						Get = false;
						SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + id / 8, id % 8, true);
						break;
					}
				}
				if (Get)
				{
					num++;
					Item item = new Item(Util.ItemIDAddress(id))
					{
						ID = id,
						Count = 1
					};
					viewmodel.Items.Add(item);
					SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + id / 8, id % 8, true);
				}
				else Get = true;
                
			}
			if (num == 0) MessageBox.Show(Properties.Resources.MessageFailAddItem);
			else MessageBox.Show(string.Format(Properties.Resources.MessageSuccessAddItem, num.ToString()));
		}
		private void ButtonCheckItem_Click(object sender, RoutedEventArgs e)
        {
			int num = 0;
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			uint[] ItemList = new uint[viewmodel.Items.Count];
			for (int i = 0; i < viewmodel.Items.Count; i++)
			{
				if (viewmodel.Items[i] == null) return;
				var dbitem = DataBase.GetConver(viewmodel.Items[i].ID, "Items");
				if (((IList)Item.EmptyItemlist).Contains(viewmodel.Items[i].ID) && dbitem == null)
				{
					ItemList[num] = viewmodel.Items[i].ID;
					num++;
				}
			}
			for (int x = 0; x < num; x++)
			{
				for (int i = 0; i < viewmodel.Items.Count; i++)
				{
					if (((IList)ItemList).Contains(viewmodel.Items[i].ID))
					{
						viewmodel.Items[i].ID = 0;
						viewmodel.Items[i].Count = 0;
						viewmodel.Items.Remove(viewmodel.Items[i]);
						break;
					}
				}
			}
			for (int i = 0; i < viewmodel.Items.Count; i++)
			{
				uint ID = viewmodel.Items[i].ID;
				SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + ID / 8, ID % 8, true);
			}
			if (num == 0) MessageBox.Show(Properties.Resources.MessageFailDel);
			else MessageBox.Show(string.Format(Properties.Resources.MessageSuccessDel, num.ToString()));
		}
		private void ButtonItemDel_Click(object sender, RoutedEventArgs e)
		{
			int num = 0;
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			if(ListBoxItem.SelectedItems.Count == 0)
            {
				MessageBox.Show(Properties.Resources.ErrorChoiceItem);
				return;
			}
			uint[] ItemList = new uint[ListBoxItem.SelectedItems.Count];
			foreach (Item items in ListBoxItem.SelectedItems)
			{
				ItemList[num] = items.ID;
				num++;
			}
			for (int x = 0; x < num; x++)
            {
				for (int i = 0; i < viewmodel.Items.Count; i++)
				{
					if (((IList)ItemList).Contains(viewmodel.Items[i].ID))
					{
						viewmodel.Items[i].ID = 0;
						viewmodel.Items[i].Count = 0;
						viewmodel.Items.Remove(viewmodel.Items[i]);
						break;
					}
				}
			}
			for (int i = 0; i < viewmodel.Items.Count; i++)
            {
				uint ID = viewmodel.Items[i].ID;
				SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + ID / 8, ID % 8, true);
			}
			MessageBox.Show(string.Format(Properties.Resources.MessageSuccessDel, num.ToString()));
		}
		private void ButtonChoiceMonster_Click(object sender, RoutedEventArgs e)
		{
            if (!(ListBoxMonster.SelectedItem is Monster monster)) return;

            monster.ID = ChoiceDialog(ChoiceWindow.eType.TYPE_MONSTER, monster.ID);
		}

		private void ButtonChoiceWeapon_Click(object sender, RoutedEventArgs e)
		{
            if (!((sender as Button)?.DataContext is Weapon weapon)) return;

            var dlg = new ChoiceWindow
            {
                ID = weapon.ID,
                Type = ChoiceWindow.eType.TYPE_WEAPON,
                WeaponType = weapon.Type
            };
            //if (dlg.WeaponType >= Info.Instance().Weapon.Count) dlg.WeaponType = 0;
			if (dlg.ShowDialog() == false) return;

			weapon.ID = dlg.ID;
			weapon.Type = dlg.WeaponType;
		}

		private void ButtonChoiceRaidAction1_Click(object sender, RoutedEventArgs e)
		{
            if (!(ListBoxMonster.SelectedItem is Monster monster)) return;

            monster.RideAction1 = ChoiceDialog(ChoiceWindow.eType.TYPE_RAIDACTION, monster.RideAction1);
		}

		private void ButtonChoiceRaidAction2_Click(object sender, RoutedEventArgs e)
		{
            if (!(ListBoxMonster.SelectedItem is Monster monster)) return;

            monster.RideAction2 = ChoiceDialog(ChoiceWindow.eType.TYPE_RAIDACTION, monster.RideAction2);
		}

		private void ButtonAppendEgg_Click(object sender, RoutedEventArgs e)
		{
            if (!(DataContext is ViewModel viewmodel)) return;

            uint count = (uint)viewmodel.Eggs.Count;
			if (count >= Util.EGG_COUNT) return;

			Egg egg = new Egg(Util.EGG_ADDRESS + count * Util.EGG_SIZE);
			viewmodel.Eggs.Add(egg);
			SaveData.Instance().WriteNumber(Util.EGG_COUNT_ADDRESS, 1, count + 1);
		}

		private void ButtonEggFileOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = ".mhs2egg",
                Filter = "蛋文件|*.mhs2egg"
            };
            if (dlg.ShowDialog() != false)
            {
                int index = ListBoxEgg.SelectedIndex;
                foreach (string file in dlg.FileNames)
                {
                    if (File.Exists(dlg.FileName))
                    {
                        if (index < 0)
                        {
                            _ = MessageBox.Show(Properties.Resources.ErrorForEggChoice);
                            return;
                        }
                        if (index < ListBoxEgg.Items.Count + 1)
                        {
                            byte[] mBuffer = File.ReadAllBytes(file);
                            var hexText = new System.Text.StringBuilder();
                            for (int i = 0; i < mBuffer.Length - EggLength; i++)
                            {
                                if (i % 2 == 0 && i != 0) hexText.Append(" ");
                                hexText.Append(mBuffer[i + EggLength].ToString("X2"));
                            }
                            AddEggFromFile(hexText.ToString());
                            Array.Clear(mBuffer, 0, mBuffer.Length);
                            index++;
                            ListBoxEgg.SelectedIndex = index;
                        }
                    }
                }
				MessageBox.Show(Properties.Resources.MessageSuccess);
			}
        }

        private void ButtonEggFileSave(object sender, RoutedEventArgs e)
		{
            
            SaveFileDialog dlg = new SaveFileDialog
            {
                DefaultExt = ".mhs2egg",
                Filter = "蛋文件|*.mhs2egg"
            };
            if (dlg.ShowDialog() != false)
            {
				var egg = ListBoxEgg.SelectedItem as Egg;
				if (egg != null)
				{
					byte[] mBuffer = SaveData.Instance().ReadValue(egg.Address, Util.EGG_SIZE);
					byte[] byteAll = new byte[EggLength + mBuffer.Length];
					Array.Copy(eggx, 0, byteAll, 0, eggx.Length);
					Array.Copy(mBuffer, 0, byteAll, EggLength, mBuffer.Length);
					File.WriteAllBytes(dlg.FileName, byteAll);
				}
			}
		}

		private void PasteEgg(object sender, RoutedEventArgs e)
        {
			string text = Clipboard.GetText();
			AddEggFromFile(text);
		}

		private void AddEggFromFile(string info)
		{
			int index = ListBoxEgg.SelectedIndex;
			if (index < 0) return;

            if (!(ListBoxEgg.SelectedItem is Egg egg)) return;

            if (info.Replace(" ", "").Length != Util.EGG_SIZE * 2)
			{
				MessageBox.Show("Wrong Egg");
				return;
			}
            if (!(DataContext is ViewModel viewmodel)) return;
            try
            {
				Util.WriteHex(egg.Address, info);
			}
            catch
            {
				MessageBox.Show("Wrong Egg");
				return;
			}
			viewmodel.Eggs.RemoveAt(index);
			viewmodel.Eggs.Insert(index, new Egg(egg.Address));
			ListBoxEgg.SelectedIndex = index;
		}

		private void ButtonChoiceGenes_Click(object sender, RoutedEventArgs e)
		{
            if (!((sender as Button)?.DataContext is Gene gene)) return;

            gene.ID = ChoiceDialog(ChoiceWindow.eType.TYPE_GENE, gene.ID);
		}

		private void ButtonMonsterGeneStackMax_Click(object sender, RoutedEventArgs e)
		{
            if (!(ListBoxMonster.SelectedItem is Monster monster)) return;

            foreach (var gene in monster.Genes)
			{
				gene.Stack = 2;
			}
        }

		private void ButtonMonsterGeneUnlock_Click(object sender, RoutedEventArgs e)
		{
            if (!(ListBoxMonster.SelectedItem is Monster monster)) return;

            foreach (var gene in monster.Genes)
			{
				gene.Lock = false;
			}
		}

		private void ButtonEggGeneStackMax_Click(object sender, RoutedEventArgs e)
		{
            if (!(ListBoxEgg.SelectedItem is Egg egg)) return;

            foreach (var gene in egg.Genes)
			{
				gene.Stack = 2;
			}
		}

		private void ButtonEggGeneUnlock_Click(object sender, RoutedEventArgs e)
		{
            if (!(ListBoxEgg.SelectedItem is Egg egg)) return;

            foreach (var gene in egg.Genes)
			{
				gene.Lock = false;
			}
		}

		private void ButtonChoiceArmor_Click(object sender, RoutedEventArgs e)
		{
            if (!((sender as Button)?.DataContext is Armor armor)) return;

            armor.ID = ChoiceDialog(ChoiceWindow.eType.TYPE_ARMOR, armor.ID);
		}

		private void ButtonChoiceTalisman_Click(object sender, RoutedEventArgs e)
		{
            if (!((sender as Button)?.DataContext is Talisman talisman)) return;

            talisman.ID = ChoiceDialog(ChoiceWindow.eType.TYPE_TALISMAN, talisman.ID);
		}

		private void ButtonChoiceTalismanSkill1_Click(object sender, RoutedEventArgs e)
		{
            if (!((sender as Button)?.DataContext is Talisman talisman)) return;

            talisman.Skill1 = ChoiceDialog(ChoiceWindow.eType.TYPE_TALISMAN_SKILL, talisman.Skill1);
		}

		private void ButtonChoiceTalismanSkill2_Click(object sender, RoutedEventArgs e)
		{
            if (!((sender as Button)?.DataContext is Talisman talisman)) return;

            talisman.Skill2 = ChoiceDialog(ChoiceWindow.eType.TYPE_TALISMAN_SKILL, talisman.Skill2);
		}

		private uint ChoiceDialog(ChoiceWindow.eType type, uint id)
		{
            var dlg = new ChoiceWindow
            {
                ID = id,
                Type = type
            };
            dlg.ShowDialog();
			return dlg.ID;
		}

		private void FileOpen(String filename)
		{
			byte[] ns = new byte[] {0x0B, 0xA6, 0x25, 0x8C, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x24, 0xE0, 0x91};
			var mBuffer = File.ReadAllBytes(filename);
            byte[] Check1 = mBuffer.Skip(40).Take(20).ToArray();
            byte[] Check2 = mBuffer.Skip(88).Take(20).ToArray();
            bool PCconfirm;
            if (Check1.SequenceEqual(ns))
            {
                PCconfirm = false;
            }
            else if (Check2.SequenceEqual(ns))
            {
                PCconfirm = true;
            }
            else
            {
                MessageBox.Show(Properties.Resources.ErrorWrongSave);
                return;
            }
            SaveData.Instance().Adventure = PCconfirm ? Util.PC_ADDRESS : 0;
			SaveData.Instance().Open(filename);
			DataContext = new ViewModel();
		}

		//private void ButtonBaseGuide_Click(object sender, RoutedEventArgs e)
		//{
		//	foreach(int i in Util.GuideMonsterList)
  //          {
		//		uint x = Util.Guide_Monster + (uint)(i - 1) * 2;
		//		SaveData.Instance().WriteNumber(x, 2, 1);
		//	}
		//	MessageBox.Show(Properties.Resources.MessageSuccess);
		//}

		private void ButtonBaseAllKinship(object sender, RoutedEventArgs e)
		{
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			foreach (Monster x in viewmodel.Monsters)
			{
				if (x == null) return;
				if(x.ID != 0)
                {
					x.LifetimeBattles = 300;
					//x.BattlesWon = 100;
                }
			}
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		private void ButtonItemAllMax(object sender, RoutedEventArgs e)
		{
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			foreach (Item x in viewmodel.Items)
			{
				if (x == null) return;
				if (!((IList)Item.Itemlist).Contains(x.ID))
				{
					if(x.ID <= 1750) x.Count = 900;
				}
			}
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		public void GetMaxLanguage()
        {
			MaxLanguage = LanguageBox.Items.Count;
        }
		private void ButtonBaseAllDenQuality(object sender, RoutedEventArgs e)
        {
			int num = 0;
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			uint story = SaveData.Instance().ReadNumber(0x162C8, 4); //0x006c5660
			foreach (Den x in viewmodel.Dens)
            {
                if (!x.Isget)
                {
					if(story == 0x006c5660)
                    {
						if (x.Type == "0101 0001")
						{
							x.Rank = 1;
							x.Rarity = 2;
							x.Isget = true;
						}
						else if (x.Type == "0102 0001")
						{
							x.Rank = 1;
							x.Isget = true;
						}
					}
                    else
                    {
						if (x.Type == "0101 0001")
						{
							if (x.Rank == 0)
							{
								x.Rank = 1;
								x.Rarity = 1;
								x.Isget = true;
							}
							else
							{
								x.Rank = 1;
								x.Rarity = 2;
								x.Isget = true;
							}
						}
						else if (x.Type == "0102 0001")
						{
							x.Rank = 1;
							x.Isget = true;
						}
					}
					num++;
				}
			}
			if(num != 0)
				MessageBox.Show(Properties.Resources.MessageSuccess);
		}

        private void ComboBox_AllWeaponLv(object sender, SelectionChangedEventArgs e)
        {
			uint thislv = 1;
			int num = 0;
			int i = ComboBoxAllLv.SelectedIndex;
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null || i == -1) return;
			if (i == 0) thislv = 3;
			else if (i == 1) thislv = 5;
			else thislv = 10;
			foreach (var x in viewmodel.Weapons)
			{
				if (x.ID != 0)
				{
					num++;
					x.Lv = thislv;
				}
			}
			foreach (var x in viewmodel.Armors)
			{
				if (x.ID != 0)
				{
					num++;
					x.Lv = thislv;
				}
			}
			if (num != 0)
				MessageBox.Show(string.Format(Properties.Resources.MessageUpgradedEquipment, num.ToString()));
		}

        //private void Button111(object sender, RoutedEventArgs e)
        //      {
        //	const string Path = "C:/Users/jim97/Desktop/info/new2.txt";
        //	OpenFileDialog dlg = new OpenFileDialog();
        //	if (dlg.ShowDialog() != false)
        //	{
        //		if (System.IO.File.Exists(dlg.FileName))
        //		{
        //			string[] lines = System.IO.File.ReadAllLines(dlg.FileName);
        //                  for (int i = 0; i < lines.Length - 2; i++)
        //                  {
        //                      string x = lines[i+2];
        //				string y = lines[i];
        //				if(i % 3 == 0 && i <= 3138)
        //                      {
        //					//if (i <= 3138)
        //					//{
        //					//	System.IO.File.AppendAllText(Path, y + ";");
        //					//}
        //					System.IO.File.AppendAllText(Path, x + "\n");
        //				}
        //                  }
        //		}
        //		MessageBox.Show("Success");
        //	}
        //}
    }
}
