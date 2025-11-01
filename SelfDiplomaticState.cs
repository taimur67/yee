using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000514 RID: 1300
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SelfDiplomaticState : DiplomaticState
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600192C RID: 6444 RVA: 0x00059139 File Offset: 0x00057339
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.Neutral;
			}
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0005913C File Offset: 0x0005733C
		public override bool AllowMovementIntoTerritory(DiplomaticTurnState diplomacy, int requestingPlayerId, int targetPlayer)
		{
			return true;
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x0600192E RID: 6446 RVA: 0x0005913F File Offset: 0x0005733F
		[JsonIgnore]
		public override bool AllowSupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x00059142 File Offset: 0x00057342
		public override bool AllowNearbyHealingProvidedBy(int providingPlayerId)
		{
			return true;
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x00059145 File Offset: 0x00057345
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new SelfDiplomaticState();
		}
	}
}
