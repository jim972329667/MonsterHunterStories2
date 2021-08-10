using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MonsterHunterStories2
{
	class Monster : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<Gene> Genes { get; set; } = new ObservableCollection<Gene>();

		private readonly uint[] LvExp = { 0, 6, 23, 58, 118, 208, 336, 508, 731, 1012, 1359, 1780, 2283, 2876, 3569, 4370, 5289, 6337, 7523, 8859, 10357, 12029, 13887, 15946, 18220, 20724, 23474, 26488, 29784, 33381, 37299, 41561, 46190, 51210, 56649, 62534, 68896, 75767, 83182, 91178, 99795, 109075, 119064, 129811, 141369, 153795, 201850, 253393, 308677, 367976, 431583, 499815, 573013, 651546, 735811, 826236, 923283, 1027449, 1139272, 1319362, 1531256, 1758835, 2003299, 2265943, 2548165, 2851474, 3231835, 3640748, 4080426, 4553259, 5106590, 5786916, 6518937, 7306699, 8154569, 9067262, 10172697, 11362972, 12644766, 14025290, 15512331, 17648286, 19949580, 22429287, 25101522, 27981530, 31603146, 35507107, 39715810, 44253441, 51817492, 64052899, 77248340, 91480172, 106830855, 123389441, 147206322, 172900449, 200621375, 230530610 };

		private readonly uint mAddress;

		public Monster(uint address)
		{
			mAddress = address;
			for (uint i = 0; i < 9; i++)
			{
				Genes.Add(new Gene(address + 332 + 4 * i));
			}
		}

		public String Name
		{
			get { return SaveData.Instance().ReadText(mAddress, 30); }
			set
			{
				SaveData.Instance().WriteText(mAddress, 30, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
			}
		}

		public uint ID
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 52, 4); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 52, 4, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
			}
		}

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 86, 1); }
			set
			{
				Util.WriteNumber(mAddress + 86, 1, value, 1, 99);
				if (value > 1 && value < 99)
					Util.WriteNumber(mAddress + 188, 4, LvExp[value] - 10, 0, 230530610);
				else if (value >= 99)
					Util.WriteNumber(mAddress + 188, 4, 200623925, 0, 230530610);
				else
					Util.WriteNumber(mAddress + 188, 4, 0, 0, 230530610);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exp)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxExp)));
			}
		}

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 188, 4); }
			set
			{
                Util.WriteNumber(mAddress + 188, 4, value, 0, 230530610);
                uint i;
                for (i = 0; i < LvExp.Length; i++)
                {
					int xx = (int)value - (int)LvExp[i];
					if (xx <= 0)
                    {
						break;
					}
				}
                if (i == 0) Util.WriteNumber(mAddress + 86, 1, 1, 1, 99); 
                else Util.WriteNumber(mAddress + 86, 1, i, 1, 99);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lv)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxExp)));
			}
		}
		public uint MaxExp
        {
            get
			{
				if (Lv >= LvExp.Length) return LvExp[LvExp.Length - 1];
				else return LvExp[Lv];
			}
        }

		public uint RideAction1
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 60, 1); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 60, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RideAction1)));
			}
		}

		public uint RideAction2
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 61, 1); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 61, 1, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RideAction2)));
			}
		}

		public uint CurrentHP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 72, 4); }
			set { Util.WriteNumber(mAddress + 72, 4, value, 0, 999999); }
		}

		public uint MaxHP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 76, 4); }
			set { Util.WriteNumber(mAddress + 76, 4, value, 0, 999999); }
		}

		public uint Speed
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 80, 4); }
			set { Util.WriteNumber(mAddress + 80, 4, value, 0, 999999); }
		}

		public uint NormalElementAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 132, 2); }
			set { Util.WriteNumber(mAddress + 132, 2, value, 0, 0xFFFF); }
		}

		public uint FireElementAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 134, 2); }
			set { Util.WriteNumber(mAddress + 134, 2, value, 0, 0xFFFF); }
		}

		public uint WaterElementAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 136, 2); }
			set { Util.WriteNumber(mAddress + 136, 2, value, 0, 0xFFFF); }
		}

		public uint ThunderElementAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 138, 2); }
			set { Util.WriteNumber(mAddress + 138, 2, value, 0, 0xFFFF); }
		}

		public uint IceElementAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 140, 2); }
			set { Util.WriteNumber(mAddress + 140, 2, value, 0, 0xFFFF); }
		}

		public uint DragonElementAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 142, 2); }
			set { Util.WriteNumber(mAddress + 142, 2, value, 0, 0xFFFF); }
		}

		public uint NormalElementDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 152, 2); }
			set { Util.WriteNumber(mAddress + 152, 2, value, 0, 0xFFFF); }
		}

		public uint FireElementDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 154, 2); }
			set { Util.WriteNumber(mAddress + 154, 2, value, 0, 0xFFFF); }
		}

		public uint WaterElementDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 156, 2); }
			set { Util.WriteNumber(mAddress + 156, 2, value, 0, 0xFFFF); }
		}

		public uint ThunderElementDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 158, 2); }
			set { Util.WriteNumber(mAddress + 158, 2, value, 0, 0xFFFF); }
		}

		public uint IceElementDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 160, 2); }
			set { Util.WriteNumber(mAddress + 160, 2, value, 0, 0xFFFF); }
		}

		public uint DragonElementDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 162, 2); }
			set { Util.WriteNumber(mAddress + 162, 2, value, 0, 0xFFFF); }
		}

		public uint HPBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 168, 1); }
			set { Util.WriteNumber(mAddress + 168, 1, value, 0, 3); }
		}

		public uint NormalElementAttackBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 169, 1); }
			set { Util.WriteNumber(mAddress + 169, 1, value, 0, 3); }
		}

		public uint FireElementAttackBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 170, 1); }
			set { Util.WriteNumber(mAddress + 170, 1, value, 0, 3); }
		}

		public uint WaterElementAttackBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 171, 1); }
			set { Util.WriteNumber(mAddress + 171, 1, value, 0, 3); }
		}

		public uint ThunderElementAttackBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 172, 1); }
			set { Util.WriteNumber(mAddress + 172, 1, value, 0, 3); }
		}

		public uint IceElementAttackBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 173, 1); }
			set { Util.WriteNumber(mAddress + 173, 1, value, 0, 3); }
		}

		public uint DragonElementAttackBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 174, 1); }
			set { Util.WriteNumber(mAddress + 174, 1, value, 0, 3); }
		}

		public uint NormalElementDefenseBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 175, 1); }
			set { Util.WriteNumber(mAddress + 175, 1, value, 0, 3); }
		}

		public uint FireElementDefenseBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 176, 1); }
			set { Util.WriteNumber(mAddress + 176, 1, value, 0, 3); }
		}

		public uint WaterElementDefenseBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 177, 1); }
			set { Util.WriteNumber(mAddress + 177, 1, value, 0, 3); }
		}

		public uint ThunderElementDefenseBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 178, 1); }
			set { Util.WriteNumber(mAddress + 178, 1, value, 0, 3); }
		}

		public uint IceElementDefenseBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 179, 1); }
			set { Util.WriteNumber(mAddress + 179, 1, value, 0, 3); }
		}

		public uint DragonElementDefenseBonus
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 180, 1); }
			set { Util.WriteNumber(mAddress + 180, 1, value, 0, 3); }
		}
		
		public uint LifetimeBattles
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 392, 4); }
			set { Util.WriteNumber(mAddress + 392, 4, value, 0, 999); }
		}
		public uint BattlesWon
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 400, 4); }
			set { Util.WriteNumber(mAddress + 400, 4, value, 0, SaveData.Instance().ReadNumber(mAddress + 392, 4)); }
		}
	}
}
