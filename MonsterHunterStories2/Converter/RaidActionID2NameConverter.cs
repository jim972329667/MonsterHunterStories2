﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class RaidActionID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			return Info.Instance().Search(Info.Instance().RideAction, id)?.Value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
