using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class ArmorID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			String name = DataBase.GetConver(id, "Armors");
			if (id == 0) name = Properties.Resources.MainNoneType;
			if (String.IsNullOrEmpty(name)) name = Properties.Resources.ErrorUnknowArmor + ": " + id.ToString();
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
