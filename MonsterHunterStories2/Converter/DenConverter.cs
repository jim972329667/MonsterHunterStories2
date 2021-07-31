using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class DenVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string type = (string)value;
			return type == "0101 0001" ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	class DenType2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string type = (string)value;
			string returnstr;
			if(type == "0101 0001")
            {
				returnstr = Properties.Resources.MainDenRandomDen;
			}
            else
            {
				returnstr = Properties.Resources.MainDenRetreatDen;
            }
			return returnstr;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
