using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Collections;
using System.IO;
using LiteDB;
using System.Linq;
using System.ComponentModel;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly byte[] eggx = System.Text.Encoding.Default.GetBytes("MHS2_EGGx");
		private static readonly int EggLength = eggx.Length % 4 == 0 ? eggx.Length : (eggx.Length / 4 * 4) + 4;
		private static readonly byte[] CoOp = new byte[]
		{
			0xFF,0x00,0x00,0x00,0x10,0x00,0x00,0x00,0x02,0x00,0x00,0x00,0xBB,0x2C,0xB2,0xC9,0xC5,0x14,0xC8,0x91,0x00,0x00,0x00,0x00,0x00,
			0x01,0x00,0x00,0xFE,0xD1,0xC8,0x47,0xB0,0x5E,0xC8,0x66,0x00,0x00,0x00,0x00,0xF0,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00
		};
		private static readonly byte[] smell = new byte[] { 0x02, 0x00, 0x00, 0x00 };

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
			string connectionString = AppDomain.CurrentDomain.BaseDirectory + "info\\MHS2.db";
			//初始化图片
			GenePNG.Bitmaps();
			//检查数据库
			if (!File.Exists(connectionString))
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
		public void GetMaxLanguage()
		{
			MaxLanguage = LanguageBox.Items.Count;
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
			dlg.support.Visibility = Visibility.Collapsed;
			dlg.ShowDialog();
		}
		private void MenuSupport_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new AboutWindow();
			dlg.information.Visibility = Visibility.Collapsed;
			dlg.Height = 500;
			dlg.Width = 500;
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
		private void FileOpen(String filename)
		{
			byte[] ns = new byte[] { 0x0B, 0xA6, 0x25, 0x8C, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0F, 0x24, 0xE0, 0x91 };
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
			IsOpen = true;
			SaveData.Instance().Adventure = PCconfirm ? Util.PC_ADDRESS : 0;
			SaveData.Instance().Open(filename);
			DataContext = new ViewModel();

			ICollectionView cv = CollectionViewSource.GetDefaultView(ListBoxItem.ItemsSource);
			cv.GroupDescriptions.Clear();
			cv.GroupDescriptions.Add(new PropertyGroupDescription("Type"));
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
			ListBoxItem.SelectedItems.Clear();
			foreach (DataBase.ConverList items in dlg.ListBoxItem.SelectedItems)
            {
                uint id = items.Key;
				
                for (int i = 0; i < viewmodel.Items.Count; i++)
				{
					if (viewmodel.Items[i].ID == id)
					{
						Get = false;
						SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + id / 8, id % 8, true);
						ListBoxItem.SelectedItems.Add(viewmodel.Items[i]);
						ListBoxItem.Focus();
						ListBoxItem.ScrollIntoView(viewmodel.Items[i]);
						break;
					}
				}
				if (Get)
				{
					num++;
					Item item = new Item(Util.ItemIDAddress(id))
					{
						ID = id,
						Count = 1,
						Type = 0
					};
					viewmodel.Items.Add(item);
					SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + id / 8, id % 8, true);
					//ListBoxItem.SelectedItems.Add(item);
				}
				else Get = true;
			}
			if (num != 0) MessageBox.Show(string.Format(Properties.Resources.MessageSuccessAddItem, num.ToString()));
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
		private void ButtonChoiceGenes_Click(object sender, RoutedEventArgs e)
		{
			if (!((sender as Button)?.DataContext is Gene gene)) return;

			gene.ID = ChoiceDialog(ChoiceWindow.eType.TYPE_GENE, gene.ID);
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
		private void ButtonMonsterAllKinship(object sender, RoutedEventArgs e)
		{
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			if (!IsOpen) return;
			foreach (Monster x in viewmodel.Monsters)
			{
				if (x == null) return;
				if (x.ID != 0)
				{
					x.LifetimeBattles = 300;
					//x.BattlesWon = 100;
				}
			}
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		private void ButtonMonsterAllLv(object sender, RoutedEventArgs e)
		{
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			if (!IsOpen) return;
			foreach (Monster x in viewmodel.Monsters)
			{
				if (x == null) return;
				if (x.ID != 0)
				{
					x.Lv = 98;
				}
			}
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		private void ButtonMonsterFileSave(object sender, RoutedEventArgs e)
		{
			if (!IsOpen) return;
			int index = ListBoxMonster.SelectedIndex;
			if (index < 0)
			{
				_ = MessageBox.Show(Properties.Resources.ErrorForEggChoice);
				return;
			}
			SaveFileDialog dlg = new SaveFileDialog
			{
				DefaultExt = ".mhs2egg",
				Filter = "蛋文件|*.mhs2egg"
			};
			if (dlg.ShowDialog() != false)
			{
				var monster = ListBoxMonster.SelectedItem as Monster;
				if (monster != null)
				{
					byte[] id = SaveData.Instance().ReadValue(monster.mAddress + 52, 4);
					byte[] gene = SaveData.Instance().ReadValue(monster.mAddress + 332, 36);
                    byte[] byteAll = new byte[EggLength + id.Length + smell.Length + gene.Length + CoOp.Length];
					int length = EggLength;
					Array.Copy(eggx, 0, byteAll, 0, eggx.Length);
                    Array.Copy(id, 0, byteAll, length, id.Length);
					length += id.Length;
					Array.Copy(smell, 0, byteAll, length, smell.Length);
					length += smell.Length;
					Array.Copy(gene, 0, byteAll, length, gene.Length);
					length += gene.Length;
					Array.Copy(CoOp, 0, byteAll, length, CoOp.Length);

                    File.WriteAllBytes(dlg.FileName, byteAll);
				}
			}
		}
		private void ButtonAppendEgg_Click(object sender, RoutedEventArgs e)
		{
			if (!(DataContext is ViewModel viewmodel)) return;
			if (!IsOpen) return;

			uint count = (uint)viewmodel.Eggs.Count;
			if (count >= Util.EGG_COUNT) return;

			Egg egg = new Egg(Util.EGG_ADDRESS + count * Util.EGG_SIZE);
			viewmodel.Eggs.Add(egg);
			SaveData.Instance().WriteNumber(Util.EGG_COUNT_ADDRESS, 1, count + 1);
		}
		private void ButtonEggFileOpen(object sender, RoutedEventArgs e)
		{
			if (!IsOpen) return;
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
			if (!IsOpen) return;
			int index = ListBoxEgg.SelectedIndex;
			if (index < 0)
			{
				_ = MessageBox.Show(Properties.Resources.ErrorForEggChoice);
				return;
			}
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
        private void ButtonPasteEgg(object sender, RoutedEventArgs e)
        {
            string text = Clipboard.GetText();
            AddEggFromFile(text);
        }
        private void AddEggFromFile(string info)
		{
			int index = ListBoxEgg.SelectedIndex;
			if (index < 0) return;
			if (!IsOpen) return;

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
		private void ComboBox_AllWeaponLv(object sender, SelectionChangedEventArgs e)
		{
			uint thislv = 1;
			int num = 0;
			int i = ComboBoxAllLv.SelectedIndex;
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null || i == -1) return;
			if (!IsOpen) return;
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
		private void ButtonCheckItem_Click(object sender, RoutedEventArgs e)
        {
			int num = 0;
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			if (!IsOpen) return;
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
			if (!IsOpen) return;
			if (ListBoxItem.SelectedItems.Count == 0)
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
		private void ButtonItemAllMax(object sender, RoutedEventArgs e)
		{
			ViewModel viewmodel = DataContext as ViewModel;
			if (!IsOpen) return;
			if (viewmodel == null) return;
			foreach (Item x in viewmodel.Items)
			{
				if (x == null) return;
				if (!((IList)Item.KeyItemlist).Contains(x.ID))
				{
					if (x.ID <= 1750) x.Count = 900;
				}
			}
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		private void ButtonBaseAllDenQuality(object sender, RoutedEventArgs e)
        {
			int num = 0;
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			if (!IsOpen) return;
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
		private void ButtonAllGuide(object sender, RoutedEventArgs e)
		{
			if (!IsOpen) return;
			if (Guide1.IsChecked == true || Guide2.IsChecked == true || Guide3.IsChecked == true)
			{
				if (Guide1.IsChecked == true)
                {
					SaveData.Instance().WriteValue(Util.GUIDE_Monsterpedia_ADDRESS_1, Guide.Monsterpedia1);
					SaveData.Instance().WriteValue(Util.GUIDE_Monsterpedia_ADDRESS_2, Guide.Monsterpedia2);
				}
				if (Guide2.IsChecked == true)
                {
					SaveData.Instance().WriteValue(Util.GUIDE_Monstipedia_ADDRESS, Guide.Monstipedia);
				}
                if (Guide3.IsChecked == true)
                {
					SaveData.Instance().WriteValue(Util.GUIDE_BookofGenes_ADDRESS, Guide.BookofGenes);
				}
			}
			else
				return;
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		private void ButtonAllMedals(object sender, RoutedEventArgs e)
		{
			if (!IsOpen) return;
			SaveData.Instance().WriteValue(Util.MEDALS_ADDRESS, Guide.Medals);
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		private void ButtonRefreshMelynx(object sender, RoutedEventArgs e)
		{
			if (!IsOpen) return;
			for(uint i = 0; i < Util.MELYNXLNC_COUNT; i++)
            {
				SaveData.Instance().WriteNumber(Util.MELYNXLNC_ADDRESS + i, 1, 0);
            }
			MessageBox.Show(Properties.Resources.MessageSuccess);
		}
		private void ButtonAllItems(object sender, RoutedEventArgs e)
		{
			bool Get = true;
			int num = 0;
			uint[] itemids = new uint[0];
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			if (!IsOpen) return;

			if (!uint.TryParse(ItemCount.Text, out uint count)) count = 1;
			else if (count > 999) count = 999;
			else if (count == 0) count = 1;

			if (ItemHealing.IsChecked == true || ItemSupport.IsChecked == true || ItemMaterials.IsChecked == true || ItemFacilities.IsChecked == true || ItemGrowth.IsChecked == true)
            {
				if (ItemHealing.IsChecked == true)
                {
					itemids = itemids.Concat(Item.HealingItemlist).ToArray();
				}
				if (ItemSupport.IsChecked == true)
				{
					itemids = itemids.Concat(Item.SupportItemlist).ToArray();
				}
				if (ItemMaterials.IsChecked == true)
				{
					itemids = itemids.Concat(Item.MaterialsItemlist).ToArray();
				}
				if (ItemFacilities.IsChecked == true)
				{
					itemids = itemids.Concat(Item.FacilitiesItemlist).ToArray();
				}
				if (ItemGrowth.IsChecked == true)
				{
					itemids = itemids.Concat(Item.GrowthItemlist).ToArray();
				}

				foreach (var id in itemids)
				{

					for (int i = 0; i < viewmodel.Items.Count; i++)
					{
						if (viewmodel.Items[i].ID == id)
						{
							viewmodel.Items[i].Count = count;
							SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + id / 8, id % 8, true);
							Get = false;
							break;
						}
					}
					if (Get)
					{
						num++;
						Item item = new Item(Util.ItemIDAddress(id))
						{
							ID = id,
							Count = count,
							Type = 0
						};
						viewmodel.Items.Add(item);
						SaveData.Instance().WriteBit(Util.ITEMSETTING_ADDRESS + id / 8, id % 8, true);
					}
					else Get = true;
				}
				if (num != 0) MessageBox.Show(string.Format(Properties.Resources.MessageSuccessAddItem, num.ToString()));
				else MessageBox.Show(Properties.Resources.MessageSuccess);
			}
		}

		//private void ButtonDIY(object sender, RoutedEventArgs e)
  //      {
		//	string s = "11111001";
		//	string xx = string.Format("{0:X}", Convert.ToInt32(s, 2));
		//	if (DIY.IsChecked == true) DIY_Grid.Visibility = Visibility.Visible;
		//	else DIY_Grid.Visibility = Visibility.Collapsed;
		//}

		//private void ButtonBaseGuide_Click(object sender, RoutedEventArgs e)
		//{
		//	foreach(int i in Util.GuideMonsterList)
		//          {
		//		uint x = Util.Guide_Monster + (uint)(i - 1) * 2;
		//		SaveData.Instance().WriteNumber(x, 2, 1);
		//	}
		//	MessageBox.Show(Properties.Resources.MessageSuccess);
		//}
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
