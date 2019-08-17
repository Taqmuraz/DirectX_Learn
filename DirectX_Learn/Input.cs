using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
	public class Input
	{
		private class KeyInputAxisCollection : IEnumerable, IEnumerable<KeyInputAxis>
		{
			private readonly List<KeyInputAxis> inputAxes = new List<KeyInputAxis> ();

			public void Add (KeyInputAxis keyAxis)
			{
				inputAxes.Add (keyAxis);
			}

			public KeyInputAxis this [Keys key]
			{
				get {
					KeyInputAxis keyAxis = inputAxes.FirstOrDefault (k => k.key == key);
					if (!keyAxis)
						throw new Exception ("There are no key with code : " + key.ToString());
					return keyAxis;
				}
			}
			IEnumerator IEnumerable.GetEnumerator ()
			{
				return inputAxes.GetEnumerator ();
			}
			IEnumerator<KeyInputAxis> IEnumerable<KeyInputAxis>.GetEnumerator ()
			{
				return inputAxes.GetEnumerator ();
			}
		}
		private class KeyInputAxis : NullBool
		{
			public readonly Keys key = Keys.None;
			public bool isHolding { get; set; }

			public KeyInputAxis (Keys key)
			{
				this.key = key;
			}
		}
		private static readonly KeyInputAxisCollection inputAxes = new KeyInputAxisCollection ();

		static Input ()
		{
			AddAxis (Keys.W);
			AddAxis (Keys.A);
			AddAxis (Keys.S);
			AddAxis (Keys.D);
			AddAxis (Keys.Q);
			AddAxis (Keys.E);
			AddAxis (Keys.Space);
		}

		public static void AddAxis (Keys key)
		{
			if (inputAxes.FirstOrDefault(k => k.key == key))
				return;
			inputAxes.Add (new KeyInputAxis (key));
		}

		public static void SubsctibeToForm (Form form)
		{
			form.KeyDown += (sender, e) => KeyDownListener (e.KeyData);
			form.KeyUp += (sender, e) => KeyUpListener (e.KeyData);
		}

		public static bool IsKeyHolding (Keys key)
		{
			return inputAxes[key].isHolding;
		}

		private static void KeyDownListener (Keys key)
		{
			foreach (var axis in inputAxes) {
				if (axis.key == key) 
				{
					axis.isHolding = true;
				}
			}
		}
		private static void KeyUpListener (Keys key)
		{
			foreach (var axis in inputAxes) {
				if (axis.key == key)
				{
					axis.isHolding = false;
				}
			}
		}
		public static Vector2 GetMoveInputUnclamped ()
		{
			Vector2 move = new Vector2 ();
			if (IsKeyHolding (Keys.A))
				move.X -= 1f;
			if (IsKeyHolding (Keys.D))
				move.X += 1f;
			if (IsKeyHolding (Keys.W))
				move.Y += 1f;
			if (IsKeyHolding (Keys.S))
				move.Y -= 1f;
			return move;
		}
		public static Vector2 GetMoveInput ()
		{
			return GetMoveInputUnclamped ().Clamp ();
		}
		public static Vector3 GetMoveInput3D ()
		{
			return GetMoveInput3DUnclamped ().Clamp ();
		}
		public static Vector3 GetMoveInput3DUnclamped ()
		{
			Vector2 move_2d = GetMoveInputUnclamped ();
			Vector3 move = new Vector3 (move_2d.X, 0f, move_2d.Y);

			if (IsKeyHolding (Keys.Q))
				move.Y -= 1f;
			if (IsKeyHolding (Keys.E))
				move.Y += 1f;
			return move;
		}
		public static Vector3 GetMoveInput3D_Flat ()
		{
			Vector2 move = GetMoveInput ();
			return new Vector3 (move.X, 0f, move.Y);
		}
	}
}
