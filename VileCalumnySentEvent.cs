using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200023A RID: 570
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VileCalumnySentEvent : DiplomaticEvent
	{
		// Token: 0x06000B23 RID: 2851 RVA: 0x0002F625 File Offset: 0x0002D825
		private VileCalumnySentEvent()
		{
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0002F62D File Offset: 0x0002D82D
		public VileCalumnySentEvent(int triggeringPlayerId, int scapegoatId, int targetPlayerId) : base(triggeringPlayerId, targetPlayerId)
		{
			base.AddAffectedPlayerId(scapegoatId);
			this.ScapegoatId = scapegoatId;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0002F645 File Offset: 0x0002D845
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Player {0} claims {1} has been speaking ill of {2}", this.TriggeringPlayerID, this.ScapegoatId, base.AffectedPlayerID);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0002F672 File Offset: 0x0002D872
		public override bool IsAssociatedWith(int playerId)
		{
			return playerId == this.ScapegoatId || base.IsAssociatedWith(playerId);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002F686 File Offset: 0x0002D886
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.VileCalumnySentInitiator;
			}
			if (forPlayerID == this.ScapegoatId)
			{
				return TurnLogEntryType.VileCalumnySentScapegoat;
			}
			if (forPlayerID == base.AffectedPlayerID)
			{
				return TurnLogEntryType.None;
			}
			return TurnLogEntryType.VileCalumnySentWitness;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0002F6B8 File Offset: 0x0002D8B8
		public override void DeepClone(out GameEvent clone)
		{
			VileCalumnySentEvent vileCalumnySentEvent = new VileCalumnySentEvent
			{
				ScapegoatId = this.ScapegoatId
			};
			base.DeepCloneDiplomaticEventParts(vileCalumnySentEvent);
			clone = vileCalumnySentEvent;
		}

		// Token: 0x0400050E RID: 1294
		[JsonProperty]
		[BindableValue("scapegoat_name", BindingOption.IntPlayerId)]
		public int ScapegoatId;
	}
}
