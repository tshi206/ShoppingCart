using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Services
{
    public class GoogleAccount
    {
        /**
         *  "id": "",
           "name": "",
           "given_name": "",
           "family_name": "",
           "link": "",
           "picture": "",
           "gender": "",
           "locale": ""
         */

        public string id { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string gender { get; set; }
        public string locale { get; set; }

    }
}
