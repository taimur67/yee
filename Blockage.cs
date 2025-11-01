using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001C0 RID: 448
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public struct Blockage : IDeepClone<Blockage>
	{
		// Token: 0x0600084F RID: 2127 RVA: 0x00027588 File Offset: 0x00025788
		public Blockage(Identifier legion, HexCoord location)
		{
			this.Legion = legion;
			this.Location = location;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00027598 File Offset: 0x00025798
		public void DeepClone(out Blockage clone)
		{
			clone = new Blockage(this.Legion, this.Location);
		}

		// Token: 0x04000406 RID: 1030
		[JsonProperty]
		public Identifier Legion;

		// Token: 0x04000407 RID: 1031
		[JsonProperty]
		public HexCoord Location;
	}
}
