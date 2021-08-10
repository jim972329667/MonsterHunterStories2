using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace MonsterHunterStories2
{
	class Gene : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly uint mAddress;

		public Gene(uint address)
		{
			mAddress = address;
		}
		
		public uint ID
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GeneImage)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Legit)));
			}
		}

		public bool Lock
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 2, 1) == 1; }
			set
			{
				SaveData.Instance().WriteNumber(mAddress + 2, 1, value == true ? 1U : 0);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lock)));
			}
		}

		public uint Stack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 3, 1); }
			set
			{
				Util.WriteNumber(mAddress + 3, 1, value, 0, 2);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stack)));
			}
		}

		public BitmapSource GeneImage
        {
            get
			{
				return GenePNG.IDBitmap(ID);
			}
        }

		public bool Legit
		{
			get
			{
				bool returnbool = false;
				if(DataBase.GetPNGID(ID) != null)
					returnbool = DataBase.GetPNGID(ID).Legit;
				return returnbool;
			}
		}
	}
}
