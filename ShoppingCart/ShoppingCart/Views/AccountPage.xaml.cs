using System;
using System.Diagnostics;
using Acr.UserDialogs;
using ShoppingCart.Models;
using ShoppingCart.Services;
using ShoppingCart.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountPage : ContentPage
	{

	    private Button Login { get; set; }
	    private Label Welcome { get; set; }

        public AccountPage ()
		{
			InitializeComponent ();

		    MessagingCenter.Subscribe<AccountViewModel, string>(this, "AuthSuccess", async (obj, msg) =>
		    {
		        Debug.WriteLine(msg.ToString());

		        Stack.Children.Remove(Login);

		        Image img = new Image {Source = AccountViewModel.UserProfileImg};
		        Stack.Children.Add(img);

		        Welcome = new Label {Text = "Hi! " + AccountViewModel.Username, FontSize = 30, FontFamily = "Arial", HorizontalOptions = LayoutOptions.Center};
		        Stack.Children.Add(Welcome);

                await DisplayAlert("Done!", "Authentication succeeded.", "nice");
		    });

		    MessagingCenter.Subscribe<AccountViewModel, string>(this, "AuthFailed", async (obj, msg) =>
		    {
		        Debug.WriteLine(msg.ToString());

		        Login.IsEnabled = true;

                await DisplayAlert("Oops!", "Something went wrong. Try again later...", "ok");
            });

		    MessagingCenter.Subscribe<AccountViewModel, string>(this, "AuthCanceled", async (obj, msg) =>
		    {
		        Debug.WriteLine(msg.ToString());

		        Login.IsEnabled = true;

                await DisplayAlert("Canceled", "Your authentication process has been canceled", "ok");
            });

            /**
             * <!-- <Button Margin="0,10,0,0" Text="Learn more" Command="{Binding OpenWebCommand}" BackgroundColor="{StaticResource Primary}" TextColor="White" /> -->
             */
            Login = new Button();
		    Login.Margin = new Thickness(0, 10, 0, 0);
		    Login.Text = "Login with Google+";
		    Login.Clicked += LoginOnClicked;

            Stack.Children.Add(Login);

		}

	    private void LoginOnClicked(object sender, EventArgs e)
	    {
	        using (UserDialogs.Instance.Loading("Authenticating...\n" +
	                                            "Please wait...",
	            null, null, true, MaskType.Black))
	        {
	            Login.IsEnabled = false;

	            Debug.WriteLine(AccountViewModel.Auth);
	            Debug.WriteLine(AccountViewModel.Auth.GetAuthenticator());

	            DependencyService.Get<IAuthenticationState>().SetAuth(AccountViewModel.Auth.GetAuthenticator());

	            Debug.WriteLine(DependencyService.Get<IAuthenticationState>());
	            Debug.WriteLine(DependencyService.Get<IAuthenticationState>().GetAuth());

	            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
	            presenter.Login(AccountViewModel.Auth.GetAuthenticator());
                
            }
        }
	}
}