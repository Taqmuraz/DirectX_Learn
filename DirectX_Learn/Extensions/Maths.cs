using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace DirectX_Learn
{
	public static class Maths
	{
		public static Quaternion Mul (this Quaternion quaternion, Vector3 vector)
		{
			return Quaternion.Multiply (quaternion, new Quaternion (vector.X, vector.Y, vector.Z, 0f));
		}
		public static Vector3 TransformVector (this Quaternion rotation, Vector3 vector)
		{
			Quaternion t = rotation.Mul (vector);
			t = Quaternion.Multiply (t, Quaternion.Invert(rotation));

			return new Vector3 (t.X, t.Y, t.Z);
		}
		public static Vector3 Clamp (this Vector3 vector, float maxLength = 1f)
		{
			float vectorLength = vector.Length();
			return vectorLength > maxLength ? Vector3.Normalize(vector) : vector;
		}
		public static Vector2 Clamp (this Vector2 vector, float maxLength = 1f)
		{
			float vectorLength = vector.Length();
			return vectorLength > maxLength ? Vector2.Normalize(vector) : vector;
		}
	}
}

