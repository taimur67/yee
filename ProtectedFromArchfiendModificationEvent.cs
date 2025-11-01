using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000270 RID: 624
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ProtectedFromArchfiendModificationEvent : GameEvent
	{
		// Token: 0x06000C41 RID: 3137 RVA: 0x0003106A File Offset: 0x0002F26A
		[JsonConstructor]
		protected ProtectedFromArchfiendModificationEvent()
		{
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00031072 File Offset: 0x0002F272
		public ProtectedFromArchfiendModificationEvent(int triggeringPlayerID, PlayerState targetPlayer, ArchfiendStat stat, Identifier protectingGameItemId) : base(triggeringPlayerID)
		{
			this.Stat = stat;
			this.ProtectingGameItemId = protectingGameItemId;
			base.AddAffectedPlayerId(targetPlayer.Id);
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00031096 File Offset: 0x0002F296
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("Archfiend {0} stat {1} was protected by item {2}", base.AffectedPlayerID, this.Stat, this.ProtectingGameItemId);
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000310C4 File Offset: 0x0002F2C4
		public override void DeepClone(out GameEvent clone)
		{
			ProtectedFromArchfiendModificationEvent protectedFromArchfiendModificationEvent = new ProtectedFromArchfiendModificationEvent
			{
				Stat = this.Stat,
				ProtectingGameItemId = this.ProtectingGameItemId
			};
			base.DeepCloneGameEventParts<ProtectedFromArchfiendModificationEvent>(protectedFromArchfiendModificationEvent);
			clone = protectedFromArchfiendModificationEvent;
		}

		// Token: 0x0400054F RID: 1359
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public ArchfiendStat Stat;

		// Token: 0x04000550 RID: 1360
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier ProtectingGameItemId;
	}
}
