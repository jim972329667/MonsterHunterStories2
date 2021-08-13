using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;

namespace MonsterHunterStories2
{
	class ViewModel
	{
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Weapon> Weapons { get; set; } = new ObservableCollection<Weapon>();
		public ObservableCollection<Armor> Armors { get; set; } = new ObservableCollection<Armor>();
		public ObservableCollection<Talisman> Talismans { get; set; } = new ObservableCollection<Talisman>();
		public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();
		public ObservableCollection<Monster> Monsters { get; set; } = new ObservableCollection<Monster>();
		public ObservableCollection<Egg> Eggs { get; set; } = new ObservableCollection<Egg>();
		public ObservableCollection<Den> Dens { get; set; } = new ObservableCollection<Den>();
		public ViewModel()
		{
            for (uint i = 1; i < 2000; i++)
			{
				uint address = Util.ItemIDAddress(i);
				Item item = new Item(address);
				if (item.ID == 0) continue;
				if (item.Count == 0) continue;
				item.Type = ItemType(item.ID);
				Items.Add(item);
			}

			for (uint i = 0; i < Util.CHARACTER_COUNT; i++)
			{
				Character chara = new Character(Util.CHARACTER_ADDRESS + Util.CHARACTER_SIZE * i);
				if (String.IsNullOrEmpty(chara.Name)) continue;

				Characters.Add(chara);
			}

			for (uint i = 0; i < Util.MONSTER_COUNT; i++)
			{
				Monster monster = new Monster(Util.MONSTER_ADDRESS + Util.MONSTER_SIZE * i);
				if (String.IsNullOrEmpty(monster.Name)) continue;

				Monsters.Add(monster);
			}

			uint count = SaveData.Instance().ReadNumber(Util.EGG_COUNT_ADDRESS, 1);
			for (uint i = 0; i < count; i++)
			{
				Egg egg = new Egg(Util.EGG_ADDRESS + Util.EGG_SIZE * i);
				Eggs.Add(egg);
			}

			for (uint i = 0; i < Util.WEAPON_COUNT; i++)
			{
				Weapon weapon = new Weapon(Util.WEAPON_ADDRESS + Util.WEAPON_SIZE * i);
				Weapons.Add(weapon);
			}

			for (uint i = 0; i < Util.ARMOR_COUNT; i++)
			{
				Armor armor = new Armor(Util.ARMOR_ADDRESS + Util.ARMOR_SIZE * i);
				Armors.Add(armor);
			}

			for (uint i = 0; i < Util.TALISMAN_COUNT; i++)
			{
				Talisman Talisman = new Talisman(Util.TALISMAN_ADDRESS + Util.TALISMAN_SIZE * i);
				Talismans.Add(Talisman);
			}

			for (uint i = 0; i < Util.DAN_COUNT; i++)
			{
				Den Den = new Den(Util.DAN_ADDRESS + Util.DAN_SIZE * i);
				if (Den.LocationID != 0xFFFFFFFF && Den.LocationID != 0)
					if (Den.Type == "0101 0001" || Den.Type == "0102 0001")
                    {
						Den.Isget = false;
						Dens.Add(Den);
					}
			}
		}

		public uint Money
		{
			get {return SaveData.Instance().ReadNumber(Util.MONEY_ADDRESS, 4);}
			set {Util.WriteNumber(Util.MONEY_ADDRESS, 4, value, 0, 9999999);}
		}
		
		public uint PlayTimeHour
		{
			get {return SaveData.Instance().ReadNumber(64, 4) / 3600;}
			set
			{
				uint time = SaveData.Instance().ReadNumber(64, 4) % 3600 + value * 3600;
				SaveData.Instance().WriteNumber(64, 4, time);
			}
		}
		public uint SaveConvert
        {
			get { return SaveData.Instance().ReadNumber(0, 1); }
			set { Util.WriteNumber(0, 1, value, 0, 1); }
		}
		public uint PlayTimeMinute
		{
			get {return SaveData.Instance().ReadNumber(64, 4) / 60 % 60;}
			set
			{
				if (value < 0) value = 0;
				if (value > 59) value = 59;
				uint time = SaveData.Instance().ReadNumber(64, 4) / 3600 * 3600 + value * 60;
				SaveData.Instance().WriteNumber(64, 4, time);
			}
		}
		public uint AllCommonSmellEgg
		{
			get { return SaveData.Instance().ReadNumber(Util.EGG_AllCOUNT_CommonSmell_ADDRESS, 4); }
			set { Util.WriteNumber(Util.EGG_AllCOUNT_CommonSmell_ADDRESS, 4, value, 0, 0xFFFF); }
		}
		public uint AllHighSmellEgg
		{
			get { return SaveData.Instance().ReadNumber(Util.EGG_AllCOUNT_HighSmell_ADDRESS, 4); }
			set { Util.WriteNumber(Util.EGG_AllCOUNT_HighSmell_ADDRESS, 4, value, 0, 0xFFFF); }
		}
		public uint AllCoOpEgg
		{
			get { return SaveData.Instance().ReadNumber(Util.EGG_AllCOUNT_CoOp_ADDRESS, 4); }
			set { Util.WriteNumber(Util.EGG_AllCOUNT_CoOp_ADDRESS, 4, value, 0, 0xFFFF); }
		}
		public uint MonstersSlain
		{
			get { return SaveData.Instance().ReadNumber(Util.MONSTER_SLAIN_ADDRESS, 4); }
			set { Util.WriteNumber(Util.MONSTER_SLAIN_ADDRESS, 4, value, 0, 0xFFFF); }
		}
		private uint ItemType(uint ID)
        {
			uint type;
            if (((IList)Item.HealingItemlist).Contains(ID))
			{
				type = 1;
            }
            else if (((IList)Item.SupportItemlist).Contains(ID))
            {
				type = 2;
            }
			else if (((IList)Item.MaterialsItemlist).Contains(ID))
			{
				type = 3;
			}
			else if (((IList)Item.FacilitiesItemlist).Contains(ID))
			{
				type = 4;
			}
			else if (((IList)Item.GrowthItemlist).Contains(ID))
			{
				type = 5;
			}
			else if (((IList)Item.KeyItemlist).Contains(ID))
			{
				type = 6;
			}
			else if (((IList)Item.EmptyItemlist).Contains(ID) && (DataBase.GetConver(ID,"Items") == "" || DataBase.GetConver(ID, "Items") == null))
			{
				type = 8;
			}
            else
            {
				type = 7;
            }
			return type;
		}
	}
}
