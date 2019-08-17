using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
	public class FreeMoveCamera : Camera
	{
		public FreeMoveCamera () : base ()
		{
			MainForm.OnGraphicLoopUpdate += FreeCameraControl;
		}
		void FreeCameraControl ()
		{
			position += TransformVector (Input.GetMoveInput3D () * 0.1f);
		}
	}
	
}
