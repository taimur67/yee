using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000251 RID: 593
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ExcommunicatedPlayerReinstatedEvent : GameEvent
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x000300C5 File Offset: 0x0002E2C5
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x000300C8 File Offset: 0x0002E2C8
		[JsonConstructor]
		public ExcommunicatedPlayerReinstatedEvent()
		{
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x000300D0 File Offset: 0x0002E2D0
		public ExcommunicatedPlayerReinstatedEvent(int playerId) : base(playerId)
		{
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000300D9 File Offset: 0x0002E2D9
		public override void DeepClone(out GameEvent clone)
		{
			clone = base.DeepCloneGameEventParts<ExcommunicatedPlayerReinstatedEvent>(new ExcommunicatedPlayerReinstatedEvent());
		}
	}
}
