using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShoppingCart.Services
{
    interface ILocalUserFolderLocator
    {
        // also copy the image to the program-accessible folder in the target platform behind the scene
        string GetPathToLocalImageDir(string filename, Stream stream);
    }
}
