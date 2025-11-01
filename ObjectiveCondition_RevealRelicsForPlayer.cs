using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005A2 RID: 1442
	[Serializable]
	public class ObjectiveCondition_RevealRelicsForPlayer : ObjectiveCondition_Reveal
	{
		// Token: 0x06001B2B RID: 6955 RVA: 0x0005E94A File Offset: 0x0005CB4A
		[JsonConstructor]
		public ObjectiveCondition_RevealRelicsForPlayer()
		{
			this.Reveal = RevealedDataFlags.Relics;
		}
	}
}
