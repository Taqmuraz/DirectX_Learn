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
    public class AnimationClipData
    {
        public readonly AnimationRootFrame rootFrame;
        private readonly AllocatedHierarchyDerived alloc = new AllocatedHierarchyDerived();

        public readonly string fileName;

        public AnimationClipData (string fileName)
        {
            this.fileName = fileName;

        }

        public void SetupBoneMatrices(FrameDerived frame)
        {
            if (frame.MeshContainer != null)
                SetupBoneMatrices((MeshContainerDerived)frame.MeshContainer);
            if (frame.FrameSibling != null)
                SetupBoneMatrices((FrameDerived)frame.FrameSibling);
            if (frame.FrameFirstChild != null)
                SetupBoneMatrices((FrameDerived)frame.FrameFirstChild);
        }
        public void SetupBoneMatrices(MeshContainerDerived mesh)
        {
            if (mesh.SkinInformation != null)
            {
                int numBones = mesh.SkinInformation.NumberBones;
                FrameDerived[] frameMatrices = new FrameDerived[numBones];

                for (int i = 0; i < numBones; i++)
                {
                    FrameDerived frame = (FrameDerived)Frame.Find(rootFrame.FrameHierarchy, mesh.SkinInformation.GetBoneName(i));
                    if (frame == null) throw new ArgumentException();
                    frameMatrices[i] = frame;
                }
                mesh.frameMatrices = frameMatrices;
            }
        }
    }
}
