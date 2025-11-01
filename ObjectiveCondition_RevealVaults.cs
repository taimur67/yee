using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A3 RID: 1443
	[Serializable]
	public class ObjectiveCondition_RevealVaults : ObjectiveCondition_Reveal
	{
		// Token: 0x06001B2C RID: 6956 RVA: 0x0005E95A File Offset: 0x0005CB5A
		[JsonConstructor]
		public ObjectiveCondition_RevealVaults()
		{
			this.Reveal = RevealedDataFlags.Vault;
		}
	}
}
