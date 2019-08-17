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
    public class MeshContainerDerived : MeshContainer
    {
        public Texture[] meshTextures { get; set; }
        public int numberAttributes { get; set; }
        public int numberInfluences { get; set; }
        public BoneCombination[] bones { get; set; }
        public FrameDerived[] frameMatrices { get; set; }
        public Matrix[] offsetMatrices { get; set; }

        public static void GenerateSkinnedMesh (MeshContainerDerived mesh)
        {
            if (mesh.SkinInformation == null) throw new ArgumentException();

            int numInfl = 0;
            BoneCombination[] bones;

            MeshData m = mesh.MeshData;
            m.Mesh = mesh.SkinInformation.ConvertToBlendedMesh(m.Mesh, MeshFlags.Managed | MeshFlags.OptimizeVertexCache, mesh.GetAdjacencyStream(), out numInfl, out bones);

            mesh.numberInfluences = numInfl;
            mesh.bones = bones;
            mesh.numberAttributes = bones.Length;
            mesh.MeshData = m;
        }
    }
}
