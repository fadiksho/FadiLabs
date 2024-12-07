using Maui.Startup.Services;

namespace Maui.Startup;

public partial class App : Application
{
	private readonly MauiUserService _userService;
	public App(MauiUserService userService)
	{
		InitializeComponent();

		_userService = userService;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage(_userService)) { Title = "Fadi Labs" };
	}
}
