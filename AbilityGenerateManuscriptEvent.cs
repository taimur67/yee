using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000203 RID: 515
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class AbilityGenerateManuscriptEvent : GameEvent
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x0002DA6A File Offset: 0x0002BC6A
		[JsonConstructor]
		private AbilityGenerateManuscriptEvent()
		{
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0002DA72 File Offset: 0x0002BC72
		public AbilityGenerateManuscriptEvent(int triggeringPlayerId, int countGenerated, Identifier providerId, string abilityId)
		{
			this.NumGenerated = countGenerated;
			this.TriggeringPlayerID = triggeringPlayerId;
			this.ProviderId = providerId;
			this.AbilityId = abilityId;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0002DA97 File Offset: 0x0002BC97
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.AbilityGenerateManuscript;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
		public override void DeepClone(out GameEvent clone)
		{
			AbilityGenerateManuscriptEvent abilityGenerateManuscriptEvent = new AbilityGenerateManuscriptEvent
			{
				ProviderId = this.ProviderId,
				AbilityId = this.AbilityId.DeepClone(),
				NumGenerated = this.NumGenerated
			};
			base.DeepCloneGameEventParts<AbilityGenerateManuscriptEvent>(abilityGenerateManuscriptEvent);
			clone = abilityGenerateManuscriptEvent;
		}

		// Token: 0x040004BE RID: 1214
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier ProviderId;

		// Token: 0x040004BF RID: 1215
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public int NumGenerated;

		// Token: 0x040004C0 RID: 1216
		[BindableValue(null, BindingOption.StaticDataId)]
		[JsonProperty]
		public string AbilityId;
	}
}
