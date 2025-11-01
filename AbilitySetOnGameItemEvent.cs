using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000676 RID: 1654
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class AbilitySetOnGameItemEvent : GameEvent
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x00069132 File Offset: 0x00067332
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x00069135 File Offset: 0x00067335
		[JsonConstructor]
		protected AbilitySetOnGameItemEvent()
		{
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x00069144 File Offset: 0x00067344
		public AbilitySetOnGameItemEvent(int triggeringPlayerID, Identifier targetId, int ownerId, ConfigRef<AbilityStaticData> abilityData, bool wasAbilityAdded = true) : base(triggeringPlayerID)
		{
			this.Target = targetId;
			base.AddAffectedPlayerId(ownerId);
			this.AbilityData = abilityData;
			this.WasAbilityAdded = wasAbilityAdded;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00069172 File Offset: 0x00067372
		public AbilitySetOnGameItemEvent(int triggeringPlayerID, GamePiece targetGamePiece, ConfigRef<AbilityStaticData> abilityData, bool wasAbilityAdded = true) : base(triggeringPlayerID)
		{
			this.Target = targetGamePiece.Id;
			base.AddAffectedPlayerId(targetGamePiece.ControllingPlayerId);
			this.AbilityData = abilityData;
			this.WasAbilityAdded = wasAbilityAdded;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000691AC File Offset: 0x000673AC
		public override string GetDebugName(TurnContext context)
		{
			if (!this.WasAbilityAdded)
			{
				return string.Format("GamePiece {0} had {1} added", this.Target, this.AbilityData);
			}
			return string.Format("GamePiece {0} had {1} removed", this.Target, this.AbilityData);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000691F8 File Offset: 0x000673F8
		public override void DeepClone(out GameEvent clone)
		{
			AbilitySetOnGameItemEvent abilitySetOnGameItemEvent = new AbilitySetOnGameItemEvent
			{
				Target = this.Target,
				AbilityData = this.AbilityData.DeepClone<AbilityStaticData>(),
				WasAbilityAdded = this.WasAbilityAdded
			};
			base.DeepCloneGameEventParts<AbilitySetOnGameItemEvent>(abilitySetOnGameItemEvent);
			clone = abilitySetOnGameItemEvent;
		}

		// Token: 0x04000CD9 RID: 3289
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public Identifier Target = Identifier.Invalid;

		// Token: 0x04000CDA RID: 3290
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public ConfigRef<AbilityStaticData> AbilityData;

		// Token: 0x04000CDB RID: 3291
		[JsonProperty]
		public bool WasAbilityAdded;
	}
}
