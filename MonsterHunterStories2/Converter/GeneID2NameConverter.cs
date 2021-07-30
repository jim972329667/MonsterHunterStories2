﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
	class GeneID2NameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint id = (uint)value;
			//String name = Info.Instance().Search(Info.Instance().Gene, id)?.Value;
			String name = DataBase.GetConver(id, "Genes");
			if (String.IsNullOrEmpty(name)) name = Properties.Resources.ErrorUnknowGene + ": " + id.ToString();
			return name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}