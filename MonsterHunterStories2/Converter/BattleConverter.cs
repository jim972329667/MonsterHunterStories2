using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class WinPercentageConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			_ = double.TryParse((string)values[0], out double Wins);
			_ = double.TryParse((string)values[1], out double Lose);
			double pr = 0;
			if (Lose + Wins == 0) pr = 0;
			else pr = Wins / (Lose + Wins);
			String value = string.Format("{0:P}", pr);
			return value;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}