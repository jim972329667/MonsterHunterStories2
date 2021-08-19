using System.ComponentModel;


namespace MonsterHunterStories2
{
	class Battle : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public uint SoloWins
		{
			get
			{
				uint value = SaveData.Instance().ReadNumber(Util.TOTAL_WINS_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_ADDRESS, 4);
				return value;
			}
			set
			{
				uint newtotal = value + PairWins + PairLose + SoloLose;
				uint newtotalwins = value + PairWins;

				Util.WriteNumber(Util.TOTAL_BATTLES_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_WINS_ADDRESS, 4, newtotalwins, 0, 0xFFFFFFFF);

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBattleWins)));
			}
		}
		public uint SoloLose
		{
			get
			{
				uint value = SaveData.Instance().ReadNumber(Util.TOTAL_BATTLES_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_BATTLES_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_WINS_ADDRESS, 4) + SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_ADDRESS, 4);
				return value;
			}
			set
			{
				uint newtotal = SoloWins + value + PairWins + PairLose;

				Util.WriteNumber(Util.TOTAL_BATTLES_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBattleLose)));
			}
		}
		public uint PairWins
		{
			get
			{
				return SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_ADDRESS, 4);
			}
			set
			{
				uint newtotalwins = value + SoloWins;
				uint newpairtotal = value + PairLose;
				uint newtotal = newpairtotal + SoloWins + SoloLose;

				Util.WriteNumber(Util.TOTAL_PAIR_WINS_ADDRESS, 4, value, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_BATTLES_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_WINS_ADDRESS, 4, newtotalwins, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_PAIR_BATTLES_ADDRESS, 4, newpairtotal, 0, 0xFFFFFFFF);

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBattleWins)));
			}
		}
		public uint PairLose
		{
			get
			{
				uint value = SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_BATTLES_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_ADDRESS, 4);
				return value;
			}
			set
			{
				uint newpairtotal = value + PairWins;
				uint newtotal = newpairtotal + SoloWins + SoloLose;

				Util.WriteNumber(Util.TOTAL_PAIR_BATTLES_ADDRESS, 4, newpairtotal, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_BATTLES_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllBattleLose)));
			}
		}
		public string AllBattleWins
		{
			get
			{
				string value = (SoloWins + PairWins).ToString();
				return value;
			}
		}
		public string AllBattleLose
		{
			get
			{
				string value = (SoloLose + PairLose).ToString();
				return value;
			}
		}
		public uint SoloWinsFrineds
		{
			get
			{
				uint value = SaveData.Instance().ReadNumber(Util.TOTAL_WINS_FRIENDS_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_FRIENDS_ADDRESS, 4);
				return value;
			}
			set
			{
				uint newtotal = value + PairWinsFrineds + PairLoseFrineds + SoloLoseFrineds;
				uint newtotalwins = value + PairWinsFrineds;

				Util.WriteNumber(Util.TOTAL_BATTLES_FRIENDS_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_WINS_FRIENDS_ADDRESS, 4, newtotalwins, 0, 0xFFFFFFFF);
			}
		}
		public uint SoloLoseFrineds
		{
			get
			{
				uint value = SaveData.Instance().ReadNumber(Util.TOTAL_BATTLES_FRIENDS_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_BATTLES_FRIENDS_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_WINS_FRIENDS_ADDRESS, 4) + SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_FRIENDS_ADDRESS, 4);
				return value;
			}
			set
			{
				uint newtotal = SoloWinsFrineds + value + PairWinsFrineds + PairLoseFrineds;

				Util.WriteNumber(Util.TOTAL_BATTLES_FRIENDS_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);
			}
		}
		public uint PairWinsFrineds
		{
			get
			{
				return SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_FRIENDS_ADDRESS, 4);
			}
			set
			{
				uint newtotalwins = value + SoloWinsFrineds;
				uint newpairtotal = value + PairLoseFrineds;
				uint newtotal = newpairtotal + SoloWinsFrineds + SoloLoseFrineds;

				Util.WriteNumber(Util.TOTAL_PAIR_WINS_FRIENDS_ADDRESS, 4, value, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_BATTLES_FRIENDS_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_WINS_FRIENDS_ADDRESS, 4, newtotalwins, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_PAIR_BATTLES_FRIENDS_ADDRESS, 4, newpairtotal, 0, 0xFFFFFFFF);
			}
		}
		public uint PairLoseFrineds
		{
			get
			{
				uint value = SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_BATTLES_FRIENDS_ADDRESS, 4) - SaveData.Instance().ReadNumber(Util.TOTAL_PAIR_WINS_FRIENDS_ADDRESS, 4);
				return value;
			}
			set
			{
				uint newpairtotal = value + PairWinsFrineds;
				uint newtotal = newpairtotal + SoloWinsFrineds + SoloLoseFrineds;

				Util.WriteNumber(Util.TOTAL_PAIR_BATTLES_FRIENDS_ADDRESS, 4, newpairtotal, 0, 0xFFFFFFFF);
				Util.WriteNumber(Util.TOTAL_BATTLES_FRIENDS_ADDRESS, 4, newtotal, 0, 0xFFFFFFFF);
			}
		}
	}
}
