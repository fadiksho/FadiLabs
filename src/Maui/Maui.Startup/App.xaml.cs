using Maui.Startup.Services;

namespace Maui.Startup;

public partial class App : Application
{
  public App(MauiUserService userService)
  {
    InitializeComponent();

    MainPage = new MainPage(userService);
  }
}
