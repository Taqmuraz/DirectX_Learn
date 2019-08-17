using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
	public class MainForm : Form
	{
		public static Device device { get; private set; }
		
		private Brush textBrush = new SolidBrush(Color.White);

		double timeLast;
		int maxFPS;
		int minFPS;

		public static MainForm activeMainForm { get; private set; }

		public delegate void GraphicLoopUpdate ();

		public static event GraphicLoopUpdate OnGraphicLoopUpdate;

		public MainForm () : base ()
		{
			activeMainForm = this;

			WindowState = FormWindowState.Maximized;
			//FormBorderStyle = FormBorderStyle.None;

			this.SetStyle (ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

			Input.SubsctibeToForm (this);
		}

		public void InitializeGraphics ()
		{
			try {

				PresentParameters prms = new PresentParameters();
				prms.Windowed = true;
				prms.SwapEffect = SwapEffect.Discard;
				prms.EnableAutoDepthStencil = true;
				prms.AutoDepthStencilFormat = DepthFormat.D16;

				device = new Device (0, DeviceType.Hardware, this, CreateFlags.HardwareVertexProcessing, prms);

				device.DeviceResizing += Device_DeviceResizing;

                GameObject gObj = new GameObject("HumanDraw");
                MeshRenderer rend = gObj.AddComponent<MeshRenderer>();
                rend.SetMesh("HumanAnim");
                rend.transform.position = new Vector3(-2f, 0f, 0f);

                gObj = new GameObject("HumanAnim");

                AnimationClip clip = gObj.AddComponent<AnimationClip>();

                clip.CreateAnimation("HumanAnim", device);
                clip.playSpeed = 1f;
                clip.Play();
                clip.transform.position = new Vector3(2f, 0f, 0f);

			} catch (Exception ex) {
				Program.Log (ex.ToString());
			}
			InitializeCamera ();
		}

		void Device_DeviceResizing (object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
		}
		void InitializeCamera ()
		{
			Camera.CreateOrGetCamera<FreeMoveCamera> (this);
			Camera.activeCamera.position = new Vector3 (0, 2f, -5.0f);
		}
		void UpdateCamera ()
		{
			Camera camera = Camera.activeCamera;
			device.Transform.Projection = camera.CreateProjectionMatrix ();
			device.Transform.View = camera.CreateViewMatrix ();
		}
		void UpdateLighting ()
		{
			device.Lights[0].Type = LightType.Directional;
			device.Lights[0].Diffuse = Color.White;
			device.Lights[0].Range = 1000f;
			device.Lights[0].Direction = new Vector3(-1f, -1f, 1f);
			device.Lights [0].Attenuation0 = 0.3f;

			device.Lights [0].Update ();
			device.Lights[0].Enabled = true;
		}
		protected override void OnPaint (PaintEventArgs e)
		{
            
			device.Clear (ClearFlags.Target | ClearFlags.ZBuffer, Color.CornflowerBlue, 1.0f, 0);

			if (OnGraphicLoopUpdate != null)
				OnGraphicLoopUpdate ();

			UpdateCamera ();
			UpdateLighting ();

			device.BeginScene ();

			device.RenderState.Lighting = true;
			device.RenderState.CullMode = Cull.None;

            RenderQueue.RenderCall(device);

			device.EndScene ();

			device.Present ();

            CalcFps(e.Graphics);

			this.Invalidate ();
		}
        private void CalcFps(Graphics graphics)
        {
            double timeNow = DateTime.Now.TimeOfDay.TotalSeconds;
            double delta = timeNow - timeLast;
            int fps = (int)(1d / delta);
            timeLast = timeNow;

            if (maxFPS < fps)
                maxFPS = fps;
            if (minFPS > fps)
                minFPS = fps;
            if (Input.IsKeyHolding(Keys.Space))
                maxFPS = minFPS = 0;

            graphics.DrawString(string.Format("FPS : {0}; MAX : {1}; MIN : {2}", fps, maxFPS, minFPS), Font, textBrush, 0, 0);
        }
    }
}
