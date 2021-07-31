using System.ComponentModel;


namespace MonsterHunterStories2
{
    class Den
	{
		private readonly uint mAddress;

		public Den(uint address)
		{
			mAddress = address;
		}

		public uint LocationID
		{
			get { return SaveData.Instance().ReadNumber(mAddress,4);}
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
			}
		}
		public uint Rarity
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 45, 1); }
			set
			{
				Util.WriteNumber(mAddress + 45, 1, value, 0, 2);
			}
		}

	}
}
