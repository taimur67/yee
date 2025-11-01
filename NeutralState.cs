using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000513 RID: 1299
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class NeutralState : DiplomaticState
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001925 RID: 6437 RVA: 0x00059119 File Offset: 0x00057319
		[JsonIgnore]
		public override DiplomaticStateValue Type
		{
			get
			{
				return DiplomaticStateValue.Neutral;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001926 RID: 6438 RVA: 0x0005911C File Offset: 0x0005731C
		[JsonIgnore]
		public override bool AllowHostileDiplomacy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001927 RID: 6439 RVA: 0x0005911F File Offset: 0x0005731F
		[JsonIgnore]
		public override bool AllowFriendlyDiplomacy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001928 RID: 6440 RVA: 0x00059122 File Offset: 0x00057322
		[JsonIgnore]
		public override bool AllowVassalRelationshipRequest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x00059125 File Offset: 0x00057325
		[JsonConstructor]
		public NeutralState()
		{
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0005912D File Offset: 0x0005732D
		public override CantonCaptureRule GetCantonCaptureRules(DiplomaticTurnState diplomacy, int requestingPlayer, int targetPlayer)
		{
			return CantonCaptureRule.CannotCapture;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x00059130 File Offset: 0x00057330
		public override void DeepClone(out DiplomaticState clone)
		{
			clone = new NeutralState();
		}
	}
}
