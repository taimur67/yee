using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200021F RID: 543
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class RevealVendettaEvent : GameEvent
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x0002E8BE File Offset: 0x0002CABE
		[JsonConstructor]
		private RevealVendettaEvent()
		{
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002E8C6 File Offset: 0x0002CAC6
		public RevealVendettaEvent(int triggeringPlayerID, PlayerPair players, VendettaState vendetta) : base(triggeringPlayerID)
		{
			this.PlayerPair = players;
			this.Vendetta = vendetta;
			base.AddAffectedPlayerId(players.First);
			base.AddAffectedPlayerId(players.Second);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002E8F5 File Offset: 0x0002CAF5
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("{0} revealed the vendetta of {1} and {2}", this.TriggeringPlayerID, this.PlayerPair.First, this.PlayerPair.Second);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x0002E92C File Offset: 0x0002CB2C
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID == this.TriggeringPlayerID)
			{
				return TurnLogEntryType.VendettaRevealed;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x0002E940 File Offset: 0x0002CB40
		public override void DeepClone(out GameEvent clone)
		{
			RevealVendettaEvent revealVendettaEvent = new RevealVendettaEvent
			{
				PlayerPair = this.PlayerPair,
				Vendetta = this.Vendetta.DeepClone(CloneFunction.FastClone)
			};
			base.DeepCloneGameEventParts<RevealVendettaEvent>(revealVendettaEvent);
			clone = revealVendettaEvent;
		}

		// Token: 0x040004E4 RID: 1252
		[JsonProperty]
		public PlayerPair PlayerPair;

		// Token: 0x040004E5 RID: 1253
		[JsonProperty]
		public VendettaState Vendetta;
	}
}
