using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000256 RID: 598
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class AbyssVictoryProgresses : GameEvent
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00030252 File Offset: 0x0002E452
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00030255 File Offset: 0x0002E455
		public AbyssVictoryProgresses() : base(-1)
		{
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0003025E File Offset: 0x0002E45E
		public override string GetDebugName(TurnContext context)
		{
			return "Abyss Victory Progresses";
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00030265 File Offset: 0x0002E465
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.AbyssProgresses;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0003026C File Offset: 0x0002E46C
		public override void DeepClone(out GameEvent clone)
		{
			AbyssVictoryProgresses abyssVictoryProgresses = new AbyssVictoryProgresses();
			base.DeepCloneGameEventParts<AbyssVictoryProgresses>(abyssVictoryProgresses);
			clone = abyssVictoryProgresses;
		}
	}
}
