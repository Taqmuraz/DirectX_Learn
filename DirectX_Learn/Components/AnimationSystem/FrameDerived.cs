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
    public class FrameDerived : Frame
    {
        private Matrix combined = Matrix.Identity;

        public Matrix combinedTransformationMatrix { get; set; }
    }
}
