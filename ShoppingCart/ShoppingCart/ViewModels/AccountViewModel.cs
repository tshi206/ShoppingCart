using System;
using System.Diagnostics;
using System.Windows.Input;
using ShoppingCart.Services;
using Xamarin.Auth;
using Xamarin.Forms;

namespace ShoppingCart.ViewModels
{
    public class AccountViewModel : CartViewModel, IGoogleAuthenticationDelegate
    {

        public static GoogleAuthenticator Auth { get; set; }

        private string clientId = "430029783273-bvuv4f3g7a7jsdd8olog0jj9sqeg2181.apps.googleusercontent.com";
        private string scope = "https://www.googleapis.com/auth/plus.login";
        private string redirectUrl = "com.googleusercontent.apps.430029783273-bvuv4f3g7a7jsdd8olog0jj9sqeg2181:/oauth2redirect";

        public AccountViewModel()
        {
            Title = "Account";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));

            Auth = new GoogleAuthenticator(clientId, scope, redirectUrl, this);
        }

        public ICommand OpenWebCommand { get; }

        public void OnAuthenticationCompleted(GoogleOAuthToken token)
        {
            Debug.WriteLine("ACCESS TOKEN : " + token.AccessToken);
            MessagingCenter.Send(this, "AuthSuccess", token.AccessToken);
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            Debug.WriteLine("AUTH FAILED : " + message);
            MessagingCenter.Send(this, "AuthFailed", message);
        }

        public void OnAuthenticationCanceled()
        {
            Debug.WriteLine("AUTH CANCELED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            MessagingCenter.Send(this, "AuthCanceled", "canceled");
        }
    }

}