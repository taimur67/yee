using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000204 RID: 516
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class AbilityGenerateTributeEvent : GameEvent
	{
		// Token: 0x06000A10 RID: 2576 RVA: 0x0002DAE7 File Offset: 0x0002BCE7
		[JsonConstructor]
		private AbilityGenerateTributeEvent()
		{
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0002DAEF File Offset: 0x0002BCEF
		public AbilityGenerateTributeEvent(int triggeringPlayerId, Identifier providerId, string abilityId)
		{
			this.TriggeringPlayerID = triggeringPlayerId;
			this.ProviderId = providerId;
			this.AbilityId = abilityId;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0002DB0C File Offset: 0x0002BD0C
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.AbilityGeneratedTribute;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0002DB14 File Offset: 0x0002BD14
		public override void DeepClone(out GameEvent clone)
		{
			AbilityGenerateTributeEvent abilityGenerateTributeEvent = new AbilityGenerateTributeEvent
			{
				ProviderId = this.ProviderId,
				AbilityId = this.AbilityId.DeepClone()
			};
			base.DeepCloneGameEventParts<AbilityGenerateTributeEvent>(abilityGenerateTributeEvent);
			clone = abilityGenerateTributeEvent;
		}

		// Token: 0x040004C1 RID: 1217
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier ProviderId;

		// Token: 0x040004C2 RID: 1218
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public string AbilityId;
	}
}
