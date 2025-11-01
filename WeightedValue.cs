using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200042C RID: 1068
	[Serializable]
	public struct WeightedValue<T>
	{
		// Token: 0x04000A12 RID: 2578
		[JsonProperty]
		public T Value;

		// Token: 0x04000A13 RID: 2579
		[JsonProperty]
		public float Weight;
	}
}
