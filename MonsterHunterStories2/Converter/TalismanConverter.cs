using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class TalismanID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			String name = DataBase.GetConver(id, "Talismans");
			if (id == 0) name = Properties.Resources.MainNoneType;
			else if (String.IsNullOrEmpty(name)) name = Properties.Resources.ErrorUnknowTalisman + ": " + id.ToString();
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	class TalismanSkillID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			//String name = Info.Instance().Search(Info.Instance().TalismanSkill, id)?.Value;
			String name = DataBase.GetConver(id, "Talisman_Skills");
			if (id == 0) name = Properties.Resources.MainNoneType;
			if (String.IsNullOrEmpty(name)) name = Properties.Resources.ErrorUnknowTalismanSkill + ": " + id.ToString();
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
