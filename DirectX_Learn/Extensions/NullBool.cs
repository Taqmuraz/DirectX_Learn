using System;

namespace DirectX_Learn
{
	public abstract class NullBool
	{
		public static implicit operator bool (NullBool nb)
		{
			return nb != null;
		}
	}
}

