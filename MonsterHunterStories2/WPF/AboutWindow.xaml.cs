using System.Windows;
using System.Windows.Input;

namespace MonsterHunterStories2
{
	/// <summary>
	/// AboutWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
		}
		private void LabelHP_MouseDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/turtle-insect/MonsterHunterStories2");
		}
		private void LabelGH_MouseDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/jim972329667/MonsterHunterStories2/");
		}
		private void LabelLT_MouseDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://bbs.naxgen.cn/thread-253594-1-1.html");
		}
		private void LabelUp_MouseDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/jim972329667/MonsterHunterStories2/releases");
		}
	}
}
