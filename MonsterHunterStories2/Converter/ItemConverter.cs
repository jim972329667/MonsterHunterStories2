using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class ItemID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			//String name = Info.Instance().Search(Info.Instance().Item, id)?.Value;
			String name = DataBase.GetConver(id, "Items");
			if (String.IsNullOrEmpty(name)) name = Properties.Resources.ErrorUnknowItem + ": " + id.ToString();
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	class ItemID2DescriptionConcerter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			String Description = DataBase.GetConver(id, "ItemDescriptions");
			if (String.IsNullOrEmpty(Description)) Description = Properties.Resources.MainNoneType;
			return Description;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	class ItemType2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint type = (uint)value;
			string name;
            switch (type)
            {
                case 0:
					name = Properties.Resources.MainItemType0;
					break;
				case 1:
					name = Properties.Resources.MainItemType1;
					break;
				case 2:
					name = Properties.Resources.MainItemType2;
					break;
				case 3:
					name = Properties.Resources.MainItemType3;
					break;
				case 4:
					name = Properties.Resources.MainItemType4;
					break;
				case 5:
					name = Properties.Resources.MainItemType5;
					break;
				case 6:
					name = Properties.Resources.MainItemType6;
					break;
				case 8:
					name = Properties.Resources.MainItemType8;
					break;
				default:
					name = Properties.Resources.MainItemType7;
					break;
			}
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
