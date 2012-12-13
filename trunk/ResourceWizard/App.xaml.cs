using System.Windows;
using GameCore;
using ResourceWizard.VMs;
using ResourceWizard.Views;

namespace ResourceWizard
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Constants.GAME_MODE = false;

			XClient.UiDispatcher = Dispatcher;
			Manager.Instance.Application = this;
			Manager.Instance.Dispatcher = Dispatcher;
			var mw = new MainWindow(){DataContext = new MainVM()};
			mw.Show();
		}
	}
}
