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
    public class AllocatedHierarchyDerived : AllocateHierarchy
    {
        public AllocatedHierarchyDerived ()
        {
        }

        public override Frame CreateFrame(string name)
        {
            FrameDerived frame = new FrameDerived();
            frame.Name = name;
            frame.TransformationMatrix = Matrix.Identity;
            frame.combinedTransformationMatrix = Matrix.Identity;

            return frame;
        }

        public override MeshContainer CreateMeshContainer(string name, MeshData meshData, ExtendedMaterial[] materials, EffectInstance[] effectInstances, GraphicsStream adjacency, SkinInformation skinInfo)
        {
            if (meshData.Mesh == null) throw new ArgumentException();
            if (meshData.Mesh.VertexFormat == VertexFormats.None) throw new ArgumentException();
            MeshContainerDerived mesh = new MeshContainerDerived();
            mesh.Name = name;
            int numFaces = meshData.Mesh.NumberFaces;
            Device dev = meshData.Mesh.Device;

            if ((meshData.Mesh.VertexFormat & VertexFormats.Normal) == 0)
            {
                Mesh tempMesh = meshData.Mesh.Clone(meshData.Mesh.Options.Value, meshData.Mesh.VertexFormat | VertexFormats.Normal, dev);
                meshData.Mesh = tempMesh;
                meshData.Mesh.ComputeNormals();
            }
            mesh.SetMaterials(materials);
            mesh.SetAdjacency(adjacency);
            Texture[] meshTextures = new Texture[materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                if (!string.IsNullOrEmpty(materials[i].TextureFilename))
                    meshTextures[i] = TextureLoader.FromStream(dev, ResourcesLoader.LoadStream(materials[i].TextureFilename));
            }
            mesh.meshTextures = meshTextures;
            mesh.MeshData = meshData;
            
            if (skinInfo != null)
            {
                mesh.SkinInformation = skinInfo;
                int numBones = skinInfo.NumberBones;
                Matrix[] offsetMatrices = new Matrix[numBones];

                for (int i = 0; i < numBones; i++)
                {
                    offsetMatrices[i] = skinInfo.GetBoneOffsetMatrix(i);
                }
                mesh.offsetMatrices = offsetMatrices;
                MeshContainerDerived.GenerateSkinnedMesh(mesh);
            }
            return mesh;
        }
    }
}
