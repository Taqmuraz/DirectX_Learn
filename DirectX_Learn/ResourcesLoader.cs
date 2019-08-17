using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DirectX_Learn
{
    public class ResourcesLoader
    {
        public static Stream LoadStream (string resourceName)
        {
            return new MemoryStream(LoadObjectAs<byte[]>(resourceName));
        }
        public static Image LoadTexture (string textureName)
        {
            return LoadObjectAs<Bitmap>(textureName);
        }
        public static T LoadObjectAs<T> (string objectName)
        {
            return (T)EngineSources.ResourceManager.GetObject(objectName, EngineSources.Culture);
        }
    }
}
