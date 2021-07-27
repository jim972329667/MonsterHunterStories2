using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class ItemID2DescriptionConcerter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			String Description = Info.Instance().Search(Info.Instance().ItemDescription, id)?.Value;
			if (String.IsNullOrEmpty(Description)) Description = Properties.Resources.MainNoneType;
			return Description;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}