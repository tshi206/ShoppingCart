using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShoppingCart.Droid;
using ShoppingCart.Services;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationState))]
namespace ShoppingCart.Droid
{
    public class AuthenticationState : IAuthenticationState
    {

        public static OAuth2Authenticator Authenticator { get; set; }

        public void SetAuth(OAuth2Authenticator authenticator)
        {
            Authenticator = authenticator;
        }

        public OAuth2Authenticator GetAuth()
        {
            return Authenticator;
        }
    }
}