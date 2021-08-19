using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class WeaponID2NameConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)values[0];
			uint type = (uint)values[1];
			String name;
			if (id == 0 && type == 0) name = "None";
			else name = DataBase.GetWeaponConver(id, type);
			if (String.IsNullOrEmpty(name))
			{
				if (type == 0x7fff)
					name = Properties.Resources.MainNoneType;
				else
					name = Properties.Resources.ErrorUnknowWeapon;
			}
			return name;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
