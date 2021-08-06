using System;
using System.ComponentModel;

namespace MonsterHunterStories2
{
	class Character : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly uint mAddress;

		private readonly uint[] LvExp = { 0, 6, 23, 58, 118, 208, 336, 508, 731, 1012, 1359, 1780, 2283, 2876, 3569, 4370, 5289, 6337, 7523, 8859, 10357, 12029, 13887, 15946, 18220, 20724, 23474, 26488, 29784, 33381, 37299, 41561, 46190, 51210, 56649, 62534, 68896, 75767, 83182, 91178, 99795, 109075, 119064, 129811, 141369, 153795, 201850, 253393, 308677, 367976, 431583, 499815, 573013, 651546, 735811, 826236, 923283, 1027449, 1139272, 1319362, 1531256, 1758835, 2003299, 2265943, 2548165, 2851474, 3231835, 3640748, 4080426, 4553259, 5106590, 5786916, 6518937, 7306699, 8154569, 9067262, 10172697, 11362972, 12644766, 14025290, 15512331, 17648286, 19949580, 22429287, 25101522, 27981530, 31603146, 35507107, 39715810, 44253441, 51817492, 64052899, 77248340, 91480172, 106830855, 123389441, 147206322, 172900449, 200621375, 230530610, 262800979 };
		public Character(uint address)
		{
			mAddress = address;
		}

		public String Name
		{
			get { return SaveData.Instance().ReadText(mAddress, 18); }
			set
			{
				SaveData.Instance().WriteText(mAddress, 18, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
			}
		}

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 234, 1); }
			set
			{
				Util.WriteNumber(mAddress + 234, 1, value, 1, 100);
				if (value > 0 && value <= 100)
					Util.WriteNumber(mAddress + 336, 4, LvExp[value] - 10, 0, 262800979);
				else
					Util.WriteNumber(mAddress + 336, 4, value, 0, 262800979);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Exp)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxExp)));
			}
		}

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 336, 4); }
			set
			{
                Util.WriteNumber(mAddress + 336, 4, value, 0, 262800979);
                uint i;
                for (i = 0; i < LvExp.Length; i++)
                {
                    if (value - LvExp[i] <= 0)
                        break;
                }
                Lv = i;
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
		public uint SkinR
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 64, 1); }
			set
			{
				Util.WriteNumber(mAddress + 64, 1, value, 0, 255);
				Util.WriteNumber(mAddress + 188, 1, value, 0, 255);
			}
		}

		public uint SkinG
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 65, 1); }
			set
			{
				Util.WriteNumber(mAddress + 65, 1, value, 0, 255);
				Util.WriteNumber(mAddress + 189, 1, value, 0, 255);
			}
		}

		public uint SkinB
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 66, 1); }
			set
			{
				Util.WriteNumber(mAddress + 66, 1, value, 0, 255);
				Util.WriteNumber(mAddress + 190, 1, value, 0, 255);
			}
		}

		public uint HairStyle
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 140, 1); }
			set { SaveData.Instance().WriteNumber(mAddress + 140, 1, value); }
		}

		public uint Eyes
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 168, 1); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 168, 1, value);
				SaveData.Instance().WriteNumber(mAddress + 174, 1, value);
			}
		}

		public uint Mouth
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 169, 1); }
			set { SaveData.Instance().WriteNumber(mAddress + 169, 1, value); }
		}

		public uint Makeup
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 170, 1); }
			set { SaveData.Instance().WriteNumber(mAddress + 170, 1, value); }
		}

		public uint BodyType
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 180, 1); }
			set { Util.WriteNumber(mAddress + 180, 1, value, 0, 1); }
		}

		public uint FaceShape
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 182, 1); }
			set { SaveData.Instance().WriteNumber(mAddress + 182, 1, value); }
		}

		public uint Voice
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 183, 1); }
			set { SaveData.Instance().WriteNumber(mAddress + 183, 1, value); }
		}


	}
}
