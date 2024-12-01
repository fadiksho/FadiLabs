using Maui.Startup.Services;

namespace Maui.Startup;

public partial class MainPage : ContentPage
{
  private readonly MauiUserService _userService;

  public MainPage(MauiUserService userService)
  {
    InitializeComponent();
    _userService = userService;
  }

  private async void BlazorWebView_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
  {
    await _userService.LoadUserAuthenticationAsync();
  }
}
