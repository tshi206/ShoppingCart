using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}
