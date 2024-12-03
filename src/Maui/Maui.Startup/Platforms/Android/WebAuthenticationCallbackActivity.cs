using Android.App;
using Android.Content.PM;

namespace Maui.Startup;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Android.Content.Intent.ActionView],
							Categories = [
								Android.Content.Intent.CategoryDefault,
								Android.Content.Intent.CategoryBrowsable
							],
							DataScheme = _callBackScheme)]
public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
{
	const string _callBackScheme = "fadilabs";
}
