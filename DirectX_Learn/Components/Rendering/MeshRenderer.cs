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
    public class MeshRenderer : GameComponent, IRenderObject
    {
        private Mesh mesh;
        private ExtendedMaterial[] materials;

        public MeshRenderer ()
        {
            RenderQueue.AddRenderObject(this);
        }
        public void SetMesh (string file)
        {
            mesh = Mesh.FromStream(ResourcesLoader.LoadStream(file), 0x0, MainForm.device, out materials);
        }

        void IRenderObject.PreRender(Device device)
        {
            device.Transform.World = transform.CreateMatrix();
        }
        void IRenderObject.Render(Device device)
        {
            if (mesh == null)
                return;

            for (int i = 0; i < mesh.NumberAttributes; i++)
            {
                device.Material = materials[i].Material3D;
                mesh.DrawSubset(i);
            }
        }
    }
}
