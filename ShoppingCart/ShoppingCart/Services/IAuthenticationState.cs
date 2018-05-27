using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;

namespace ShoppingCart.Services
{
    public interface IAuthenticationState
    {
        void SetAuth(OAuth2Authenticator authenticator);
        OAuth2Authenticator GetAuth();
    }
}
