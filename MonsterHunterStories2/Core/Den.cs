using System.ComponentModel;


namespace MonsterHunterStories2
{
    class Den : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private readonly uint mAddress;

		public Den(uint address)
		{
			mAddress = address;
		}

		public uint LocationID //Den number
		{
			get { return SaveData.Instance().ReadNumber(mAddress,2);}
		}
		public uint LocationID2
		{
			get { return SaveData.Instance().ReadNumber(mAddress+2, 2); }
		}
		public uint LocationID3  //Location code
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 12, 4); }
		}

		public string Type
        {
			get { return Util.ReadHex(mAddress + 20, 4); }
		}
		public uint Rank
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 42, 1) - 1; }
			set
			{
				Util.WriteNumber(mAddress + 42, 1, value + 1, 1, 2);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rank)));
			}
		}
		public uint Rarity
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 46, 1); }
			set
			{
				Util.WriteNumber(mAddress + 46, 1, value, 0, 2);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rarity)));
			}
		}
		public bool Isget { set; get; }

	}
}
