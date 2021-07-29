using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class RaidActionID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			return DataBase.GetConver(id, "Rides");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
