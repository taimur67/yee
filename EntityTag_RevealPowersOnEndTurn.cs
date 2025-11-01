using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002AC RID: 684
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_RevealPowersOnEndTurn : EntityTag
	{
		// Token: 0x06000D22 RID: 3362 RVA: 0x000349B0 File Offset: 0x00032BB0
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_RevealPowersOnEndTurn entityTag_RevealPowersOnEndTurn = new EntityTag_RevealPowersOnEndTurn
			{
				WhatToReveal = this.WhatToReveal
			};
			base.DeepCloneEntityTagParts(entityTag_RevealPowersOnEndTurn);
			clone = entityTag_RevealPowersOnEndTurn;
		}

		// Token: 0x040005D3 RID: 1491
		[JsonProperty]
		public PowerRevelations WhatToReveal;
	}
}
