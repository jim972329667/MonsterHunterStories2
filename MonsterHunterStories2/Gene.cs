using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MonsterHunterStories2
{
	class Gene : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly uint mAddress;

		public static Bitmap[] bparry = new Bitmap[187];

		private static int width = 68;

		private static int height = 56;

		public Gene(uint address)
		{
			mAddress = address;
		}
		public static void Bitmaps()
        {
			int count = 0;
			for (int j = 0; j < 11; j++)   //height
			{
				for (int i = 0; i < 17; i++)   //weighr
				{
					Bitmap newbitmap = new Bitmap(width, height);
					Graphics g = Graphics.FromImage(newbitmap);
					Rectangle rect = new Rectangle(0, 0, width, height);
					Rectangle rect1 = new Rectangle(width * i, height * j, width, height);
					g.DrawImage(Properties.Resources.icons_letsgo, rect, rect1, GraphicsUnit.Pixel);
					g.Dispose();
					bparry[count] = newbitmap;
					count++;
				}
			}
		}
		public uint ID
		{
			get { return SaveData.Instance().ReadNumber(mAddress, 2); }
			set
			{
				SaveData.Instance().WriteNumber(mAddress, 2, value);
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GeneImage)));
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
                try
                {
					BitmapSource returnsource = ConvertBitmap(bparry[ID]);
					return returnsource;
				}
                catch
                {
					BitmapSource returnsource = ConvertBitmap(bparry[0]);
					return returnsource;
				}
				
			}
        }
		public static BitmapSource ConvertBitmap(Bitmap source)

		{

			return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(

						  source.GetHbitmap(),

						  IntPtr.Zero,

						  Int32Rect.Empty,

						  BitmapSizeOptions.FromEmptyOptions());

		}

	}
}
