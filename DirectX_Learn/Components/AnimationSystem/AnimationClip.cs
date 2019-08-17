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
    public class AnimationClip : GameComponent, IRenderObject
    {
        private AnimationRootFrame rootFrame;

        private float lastTime;
        private float elapsedTime;
        private bool playing = false;
        
        public float playSpeed { get; set; }

        public AnimationClip()
        {
            playSpeed = 1f;
            RenderQueue.AddRenderObject(this);
        }

        public void Play()
        {
            elapsedTime = 0;
            lastTime = Time.time;
            playing = true;
        }
        public void Stop()
        {
            playing = false;
        }
        void UpdateTime()
        {
            if (!playing)
                return;
            elapsedTime = (Time.time - lastTime) * playSpeed;
            lastTime = Time.time;
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

        void IRenderObject.PreRender(Device device)
        {
            UpdateTime();
            ProcessNextFrame();
        }
        void IRenderObject.Render(Device device)
        {
            DrawFrame((FrameDerived)rootFrame.FrameHierarchy, device);
        }

        private void DrawFrame (FrameDerived frame, Device device)
        {
            MeshContainerDerived mesh = (MeshContainerDerived)frame.MeshContainer;

            while (mesh != null)
            {
                DrawMeshContainer(mesh, frame, device);
                mesh = (MeshContainerDerived)mesh.NextContainer;
            }
            if (frame.FrameSibling != null)
                DrawFrame((FrameDerived)frame.FrameSibling, device);
            if (frame.FrameFirstChild != null)
                DrawFrame((FrameDerived)frame.FrameFirstChild, device);
        }

        private void DrawMeshContainer(MeshContainerDerived mesh, FrameDerived frame, Device device)
        {
            if (mesh.SkinInformation != null)
            {
                int attribldPrev = -1;
                for (int iattrib = 0; iattrib < mesh.numberAttributes; iattrib++)
                {
                    int numBlend = 0;
                    BoneCombination[] bones = mesh.bones;
                    for (int i = 0; i < mesh.numberInfluences; i++)
                    {
                        if (bones[iattrib].BoneId[i] != -1)
                        {
                            numBlend = i;
                        }
                    }
                    if (device.DeviceCaps.MaxVertexBlendMatrices >= numBlend + 1)
                    {
                        Matrix[] offsetMatrices = mesh.offsetMatrices;
                        FrameDerived[] frameMatrices = mesh.frameMatrices;
                        for (int i = 0; i < mesh.numberInfluences; i++)
                        {
                            int matrixlndex = bones[iattrib].BoneId[i];
                            if (matrixlndex != -1)
                            {
                                Matrix tempMatrix = offsetMatrices[matrixlndex] *
                                frameMatrices[matrixlndex].combinedTransformationMatrix;
                                device.Transform.SetWorldMatrixByIndex(i, tempMatrix);
                            }
                        }
                        device.RenderState.VertexBlend = (VertexBlend)numBlend;
                        if ((attribldPrev != bones[iattrib].AttributeId) ||
                        (attribldPrev == -1))
                        {
                            device.Material = mesh.GetMaterials()[
                            bones[iattrib].AttributeId].Material3D;
                            device.SetTexture(0, mesh.meshTextures[
                            bones[iattrib].AttributeId]);
                            attribldPrev = bones[iattrib].AttributeId;
                        }
                        mesh.MeshData.Mesh.DrawSubset(iattrib);
                    }
                }
            }
            else
            {
                device.Transform.World = frame.combinedTransformationMatrix;
                ExtendedMaterial[] mtrl = mesh.GetMaterials();
                for (int iMaterial = 0; iMaterial < mtrl.Length; iMaterial++)
                {
                    device.Material = mtrl[iMaterial].Material3D;
                    device.SetTexture(0, mesh.meshTextures[iMaterial]);
                    mesh.MeshData.Mesh.DrawSubset(iMaterial);
                }
            }
        }


        private void ProcessNextFrame ()
        {
            if (rootFrame.AnimationController != null)
                rootFrame.AnimationController.AdvanceTime(elapsedTime, null);

            Matrix worldMatrix = transform.CreateMatrix();
            UpdateFrameMatrices((FrameDerived)rootFrame.FrameHierarchy, worldMatrix);
        }
        public void CreateAnimation (string fileName, Device device)
        {
            AllocatedHierarchyDerived alloc = new AllocatedHierarchyDerived();
            rootFrame = Mesh.LoadHierarchy(ResourcesLoader.LoadStream(fileName), MeshFlags.Managed, device, alloc, null);

            SetupBoneMatrices((FrameDerived)rootFrame.FrameHierarchy);
        }
        private void UpdateFrameMatrices (FrameDerived frame, Matrix parentMatrix)
        {
            frame.combinedTransformationMatrix = frame.TransformationMatrix * parentMatrix;
            if (frame.FrameSibling != null)
                UpdateFrameMatrices((FrameDerived)frame.FrameSibling, parentMatrix);
            if (frame.FrameFirstChild != null)
                UpdateFrameMatrices((FrameDerived)frame.FrameFirstChild, frame.combinedTransformationMatrix);
        }
    }
}
