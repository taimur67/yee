using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000280 RID: 640
	[Serializable]
	public class InformationContext : IDeepClone<InformationContext>
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0003169C File Offset: 0x0002F89C
		[JsonIgnore]
		public RevealedDataFlags Flags
		{
			get
			{
				RevealedDataFlags revealedDataFlags = (RevealedDataFlags)0;
				if (this.Powers)
				{
					revealedDataFlags |= RevealedDataFlags.Powers;
				}
				if (this.Vault)
				{
					revealedDataFlags |= RevealedDataFlags.Vault;
				}
				if (this.Events)
				{
					revealedDataFlags |= RevealedDataFlags.Events;
				}
				if (this.Rituals)
				{
					revealedDataFlags |= RevealedDataFlags.Rituals;
				}
				if (this.Schemes)
				{
					revealedDataFlags |= RevealedDataFlags.Schemes;
				}
				if (this.Relics)
				{
					revealedDataFlags |= RevealedDataFlags.Relics;
				}
				return revealedDataFlags;
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x000316F8 File Offset: 0x0002F8F8
		public void DeepClone(out InformationContext clone)
		{
			clone = new InformationContext
			{
				Powers = this.Powers,
				Vault = this.Vault,
				Events = this.Events,
				Rituals = this.Rituals,
				Schemes = this.Schemes,
				Relics = this.Relics
			};
		}

		// Token: 0x04000570 RID: 1392
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public bool Powers;

		// Token: 0x04000571 RID: 1393
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public bool Vault;

		// Token: 0x04000572 RID: 1394
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public bool Events;

		// Token: 0x04000573 RID: 1395
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public bool Rituals;

		// Token: 0x04000574 RID: 1396
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public bool Schemes;

		// Token: 0x04000575 RID: 1397
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public bool Relics;
	}
}
