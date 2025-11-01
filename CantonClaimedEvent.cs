using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200024A RID: 586
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class CantonClaimedEvent : GameEvent
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0002FE06 File Offset: 0x0002E006
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0002FE09 File Offset: 0x0002E009
		[JsonIgnore]
		public int PreviousOwner
		{
			get
			{
				return base.AffectedPlayerID;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0002FE11 File Offset: 0x0002E011
		[JsonIgnore]
		public int NewOwner
		{
			get
			{
				return this.TriggeringPlayerID;
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0002FE19 File Offset: 0x0002E019
		[JsonConstructor]
		private CantonClaimedEvent()
		{
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002FE21 File Offset: 0x0002E021
		public CantonClaimedEvent(HexCoord hex, int claimingPlayerId, int previousOwnerPlayerId) : base(claimingPlayerId)
		{
			this.Hex = hex;
			base.AddAffectedPlayerId(previousOwnerPlayerId);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002FE38 File Offset: 0x0002E038
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Canton Claimed {0},{1}", this.Hex.column, this.Hex.row);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002FE64 File Offset: 0x0002E064
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.None;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0002FE68 File Offset: 0x0002E068
		public override void DeepClone(out GameEvent clone)
		{
			CantonClaimedEvent cantonClaimedEvent = new CantonClaimedEvent
			{
				Hex = this.Hex
			};
			base.DeepCloneGameEventParts<CantonClaimedEvent>(cantonClaimedEvent);
			clone = cantonClaimedEvent;
		}

		// Token: 0x04000518 RID: 1304
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public HexCoord Hex;
	}
}
