using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
	public static class Program
	{
		public static void Main ()
		{
			using (MainForm form = new MainForm())
			{
				form.Show ();
				form.InitializeGraphics ();

				Application.Run (form);
			}
		}
		public static void Log (string message)
		{
			Console.WriteLine (message);
		}
	}
}

