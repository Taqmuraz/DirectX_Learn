using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
	public class TransformComponent : NullBool
	{
		public Vector3 position { get; set; }
		public Quaternion rotation { get; set; }
        public Vector3 size { get; set; }

		public Vector3 right
		{
			get { return TransformVector (new Vector3 (1f, 0f, 0f)); }
		}
		public Vector3 up
		{
			get { return TransformVector (new Vector3 (0f, 1f, 0f)); }
		}
		public Vector3 forward
		{
			get { return TransformVector (new Vector3 (0f, 0f, 1f)); }
		}

		public TransformComponent ()
		{
			rotation = Quaternion.Identity;
            size = new Vector3(1f, 1f, 1f);
		}

		public Vector3 TransformVector (Vector3 origin)
		{
			return rotation.TransformVector (origin);
		}
        public Matrix CreateMatrix ()
        {
            return Matrix.Scaling(size) * Matrix.RotationQuaternion(rotation) * Matrix.Translation(position);
        }
	}
	
}
