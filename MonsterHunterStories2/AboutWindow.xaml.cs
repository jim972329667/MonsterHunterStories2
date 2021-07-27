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
			System.Diagnostics.Process.Start("http://turtleinsect.php.xdomain.jp/");
		}
		private void LabelGH_MouseDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/jim972329667/MonsterHunterStories2/");
		}
		private void LabelLT_MouseDown(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.91wii.com/thread-253594-1-1.html");
		}
	}
}
