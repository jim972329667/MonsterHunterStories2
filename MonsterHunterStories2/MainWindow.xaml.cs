using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Collections;
using System.IO;

namespace MonsterHunterStories2
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly byte[] eggx = System.Text.Encoding.Default.GetBytes("MHS2_EGGx");
		private static readonly int EggLength = eggx.Length % 4 == 0 ? eggx.Length : (eggx.Length / 4 * 4) + 4;
        public MainWindow()
		{
			//初始化图片
			GenePNG.Bitmaps();
			InitializeComponent();
			if (!File.Exists(@".\info\text.db")) MessageBox.Show("丢失数据库");
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
			dlg.ShowDialog();
		}
		private void MenuAddDB_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new AddDB();
			dlg.ShowDialog();
		}

		private void ButtonChoiceItem_Click(object sender, RoutedEventArgs e)
		{
			bool Get = true;
			bool AllHave = false;
            ChoiceWindow dlg = new ChoiceWindow
            {
                Type = ChoiceWindow.eType.TYPE_ITEM
            };
            dlg.ListBoxItem.SelectionMode = SelectionMode.Extended;
			dlg.ShowDialog();
			ViewModel viewmodel = DataContext as ViewModel;
			if (viewmodel == null) return;
			foreach (KeyValuesInfo items in dlg.ListBoxItem.SelectedItems)
            {
                uint id = items.Key;
                for (int i = 0; i < viewmodel.Items.Count; i++)
				{
					if (viewmodel.Items[i].ID == id)
					{
						Get = false;
						AllHave = true;
						break;
					}
				}
				if (Get)
				{
					AllHave = false;
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
            if (AllHave) MessageBox.Show("已经全部拥有!");
			else MessageBox.Show("Success!");
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
            if (dlg.WeaponType >= Info.Instance().Weapon.Count) dlg.WeaponType = 0;
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
				MessageBox.Show("Success");
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
            Util.WriteHex(egg.Address, info);
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
			SaveData.Instance().Adventure = Properties.Settings.Default.PCConfirm ? Util.PC_ADDRESS : 0;
			SaveData.Instance().Open(filename);
			DataContext = new ViewModel();
		}

		private void ButtonBaseGuide_Click(object sender, RoutedEventArgs e)
		{
			foreach(int i in Util.GuideMonsterList)
            {
				uint x = Util.Guide_Monster + (uint)(i - 1) * 2;
				SaveData.Instance().WriteNumber(x, 2, 1);
			}
			MessageBox.Show("Success!");
		}

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
			MessageBox.Show("Success");
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
			MessageBox.Show("Success");
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
