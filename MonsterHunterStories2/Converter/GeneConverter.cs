﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class GeneID2NameConverter : IValueConverter
	{
		public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)values;
			String name = DataBase.GetConver(id, "Genes");
			if (String.IsNullOrEmpty(name)) name = Properties.Resources.ErrorUnknowGene + ": " + id.ToString();
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
	class GeneID2SkillConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			String Skill = DataBase.GetConver(id, "GeneSkills");
			//String Name = Info.Instance().Search(Info.Instance().Gene, id)?.Value;
			String Name = DataBase.GetConver(id, "Genes");
			if (String.IsNullOrEmpty(Skill)) Skill = Properties.Resources.MainNoneType;
			if (String.IsNullOrEmpty(Name)) Name = Properties.Resources.ErrorUnknowGene + ": " + id.ToString();
			String ReturnStr = Name + "\n" + Skill;

			return ReturnStr;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	class GeneLegitVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool legit = (bool)value;
			return legit ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}