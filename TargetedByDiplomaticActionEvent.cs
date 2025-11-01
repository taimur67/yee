using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000272 RID: 626
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TargetedByDiplomaticActionEvent : GameEvent
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x00031165 File Offset: 0x0002F365
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Secret;
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00031168 File Offset: 0x0002F368
		[JsonConstructor]
		public TargetedByDiplomaticActionEvent()
		{
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00031170 File Offset: 0x0002F370
		public TargetedByDiplomaticActionEvent(PlayerIndex triggeringPlayer, PlayerIndex targetPlayer) : base((int)triggeringPlayer)
		{
			base.AddAffectedPlayerId((int)targetPlayer);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00031180 File Offset: 0x0002F380
		public override void DeepClone(out GameEvent clone)
		{
			clone = new TargetedByDiplomaticActionEvent();
			base.DeepCloneGameEventParts<GameEvent>(clone);
		}
	}
}
