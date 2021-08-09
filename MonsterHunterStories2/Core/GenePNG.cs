using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MonsterHunterStories2
{
    public class GenePNG
    {
        public static Bitmap[] bparry = new Bitmap[26];

        private static readonly int Width = 320;

        private static readonly int Height = 300;
        public static void Bitmaps()
        {
            int count = 0;
            for (int j = 0; j < 4; j++)   //height
            {
                for (int i = 0; i < 8; i++)   //Width
                {
                    if (j == 3 && i == 2)
                    {
                        break;
                    }
                    Bitmap newbitmap = new Bitmap(Width, Height);
                    Graphics g = Graphics.FromImage(newbitmap);
                    Rectangle rect = new Rectangle(0, 0, Width, Height);
                    Rectangle rect1 = new Rectangle(Width * i, Height * j, Width, Height);
                    g.DrawImage(Properties.Resources.Gene_Table, rect, rect1, GraphicsUnit.Pixel);
                    g.Dispose();
                    bparry[count] = newbitmap;
                    count++;
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
        public static BitmapSource IDBitmap(uint ID)
        {
            uint x = 0;
            //GeneID.TryGetValue(ID, out x);
            if (DataBase.GetPNGID(ID) != null)
                x = DataBase.GetPNGID(ID).PNGID;
            if (x > 25) x = 0;
            BitmapSource returnsource = ConvertBitmap(bparry[x]);
            return returnsource;
        }
    }
}
