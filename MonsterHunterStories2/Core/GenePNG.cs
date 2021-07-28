using System;
using System.Collections.Generic;
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
            //string filename = "info\\GenePNGID.txt";
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
            //Fileopen(filename);
        }

        //public static Dictionary<uint, uint> GeneID = new Dictionary<uint, uint>();
        
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
            try
            {
                //GeneID.TryGetValue(ID, out x);
                x = DataBase.GetPNGID(ID);
                if (x > 25) x = 0;
            }
            catch
            {
                x = 0;
            }
            BitmapSource returnsource = ConvertBitmap(bparry[x]);
            return returnsource;
        }
        //private static void Fileopen(string FileName)
        //{
        //    if (!System.IO.File.Exists(FileName)) return;
        //    string[] lines = System.IO.File.ReadAllLines(FileName);
        //    foreach (string line in lines)
        //    {
        //        if (line.Length < 3) continue;
        //        if (line[0] == '#') continue;
        //        string[] values = line.Split('\t');
        //        if (values.Length < 2) continue;
        //        if (string.IsNullOrEmpty(values[0]) || string.IsNullOrEmpty(values[1])) continue;
        //        _ = uint.TryParse(values[0], out uint result1);
        //        _ = uint.TryParse(values[1], out uint result2);
        //        GeneID.Add(result1, result2);
        //    }
        //}
    }
}
