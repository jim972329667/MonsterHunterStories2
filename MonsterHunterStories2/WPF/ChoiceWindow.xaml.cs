﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using System.Collections.Generic;

namespace MonsterHunterStories2
{
	/// <summary>
	/// ChoiceWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class ChoiceWindow : Window
	{
		public enum eType
		{
			TYPE_ITEM,
			TYPE_MONSTER,
			TYPE_RAIDACTION,
			TYPE_GENE,
			TYPE_GENESKILL,
			TYPE_WEAPON,
			TYPE_ARMOR,
			TYPE_TALISMAN,
			TYPE_TALISMAN_SKILL,
			TYPE_ITEM_DESCRIPTION,
		}

		public uint ID { get; set; }
		public eType Type { get; set; } = eType.TYPE_ITEM;
		public uint WeaponType { get; set; }

		private List<DataBase.ConverList> LoveGene = new List<DataBase.ConverList>();
		public ChoiceWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			CreateItemList("");
			TextBoxFilter.Focus();
			WeaponTypeArea.Visibility = Type == eType.TYPE_WEAPON ? Visibility.Visible : Visibility.Collapsed;
			//GeneArea.Visibility = Type == eType.TYPE_GENE ? Visibility.Visible : Visibility.Collapsed;
			GeneArea.Visibility = Visibility.Collapsed;
			ComboBoxWeaponType.SelectedIndex = (int)WeaponType;
		}

		private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CreateItemList(TextBoxFilter.Text);
		}

		private void ListBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ButtonDecision.IsEnabled = ListBoxItem.SelectedIndex >= 0;
		}

		private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ButtonDecision_Click(sender, null);
		}

		private void ComboBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			WeaponType = (uint)ComboBoxWeaponType.SelectedIndex;
			CreateItemList(TextBoxFilter.Text);
			TextBoxFilter.Focus();
		}

		private void ButtonDecision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
				var item = (KeyValuesInfo)ListBoxItem.SelectedItem;
				ID = item.Key;
			}
            catch
            {
				var item = (DataBase.ConverList)ListBoxItem.SelectedItem;
				ID = item.Key;
			}
			
			DialogResult = true;
			Close();
		}

		private void CreateItemList(String filter)
		{
			ListBoxItem.Items.Clear();
            var dbinfos = DataBase.GetConverList("Items");
			//var infos = Info.Instance().Item;
			//System.Collections.Generic.List<KeyValuesInfo> infos = null;

			//else if (Type == eType.TYPE_MONSTER) infos = Info.Instance().Monster;
			//else if (Type == eType.TYPE_RAIDACTION) infos = Info.Instance().RideAction;
			//else if (Type == eType.TYPE_GENE) infos = Info.Instance().Gene;
			//else if (Type == eType.TYPE_TALISMAN_SKILL) infos = Info.Instance().TalismanSkill;
			//else if (Type == eType.TYPE_GENESKILL) infos = Info.Instance().GeneSkill;
			//else if (Type == eType.TYPE_ITEM_DESCRIPTION) infos = Info.Instance().ItemDescription;

			if (Type == eType.TYPE_ARMOR) dbinfos = DataBase.GetConverList("Armors");
			else if (Type == eType.TYPE_TALISMAN) dbinfos = DataBase.GetConverList("Talismans");
			else if (Type == eType.TYPE_WEAPON) dbinfos = DataBase.GetWeaponConverList("Weapons", WeaponType);
			else if (Type == eType.TYPE_RAIDACTION) dbinfos = DataBase.GetConverList("Rides");
			else if (Type == eType.TYPE_MONSTER) dbinfos = DataBase.GetConverList("Monsters");
			else if (Type == eType.TYPE_GENE)
			{
				if(ComboBoxGenes.SelectedIndex == 1)
                {
					dbinfos = LoveGene;
                }
				else
					dbinfos = DataBase.GetConverList("Genes");
			}
			else if (Type == eType.TYPE_TALISMAN_SKILL) dbinfos = DataBase.GetConverList("Talisman_Skills");

			foreach (DataBase.ConverList info in dbinfos)
			{
				String value = info.Value;
				if (String.IsNullOrEmpty(value)) continue;
				if (String.IsNullOrEmpty(filter) || value.IndexOf(filter,StringComparison.OrdinalIgnoreCase) >= 0)
				{
					ListBoxItem.Items.Add(info);
				}
			}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			
			var x = ListBoxItem.SelectedItems;
			foreach(DataBase.ConverList xx in x)
            {
				LoveGene.Add(xx);
            }
		}
    }
}
