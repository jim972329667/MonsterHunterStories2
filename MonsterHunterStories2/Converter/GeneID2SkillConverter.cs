using System;
using System.Globalization;
using System.Windows.Data;

namespace MonsterHunterStories2
{
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
}