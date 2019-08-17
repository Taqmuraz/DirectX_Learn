using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
	public class Camera : TransformComponent
	{
		public static Camera activeCamera { get; private set; }

		private static Form mainForm;

		private float fov = 60f;
		private float zNear = 0.03f;
		private float zFar = 100f;

		public static Camera CreateOrGetCamera<T> (Form form) where T : Camera, new()
		{
			mainForm = form;
			if (!activeCamera || !(activeCamera is T))
				return activeCamera = new T ();
			return activeCamera;
		}

		protected Camera () : base ()
		{
			
		}

		public Matrix CreateProjectionMatrix ()
		{
			float aspectRatio = (float)mainForm.Width / (float)mainForm.Height;

			return Matrix.PerspectiveFovLH (fov * ((float)Math.PI / 180f), aspectRatio, zNear, zFar);
		}
		public Matrix CreateViewMatrix ()
		{
			Camera camera = Camera.activeCamera;
			return Matrix.LookAtLH (camera.position, camera.position + camera.forward, camera.up);
		}
	}
}
