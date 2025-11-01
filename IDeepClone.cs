using System;

namespace LoG
{
	// Token: 0x020006DF RID: 1759
	public interface IDeepClone<T>
	{
		// Token: 0x0600213A RID: 8506
		void DeepClone(out T clone);
	}
}
