using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
    public class RenderQueue
    {
        private static readonly List<IRenderObject> renderObjects = new List<IRenderObject>();
        

        public static void AddRenderObject (IRenderObject renderObject)
        {
            renderObjects.Add(renderObject);
        }
        public static void RemoveRenderObject (IRenderObject renderObject)
        {
            renderObjects.Remove(renderObject);
        }

        public static void RenderCall (Device device)
        {
            if (device == null)
                throw new ArgumentException("Device is null!");
            foreach (var r in renderObjects)
            {
                r.PreRender(device);
                r.Render(device);
            }
        }
    }
}
