﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class MonsterID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			string name = DataBase.GetConver(id, "Monsters");
			if (id == 0) name = Properties.Resources.MainNoneType;
			else if (string.IsNullOrEmpty(name)) name = Properties.Resources.ErrorUnknowMonster + ": " + id.ToString(); ;
			//return Info.Instance().Search(Info.Instance().Monster, id)?.Value;
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
