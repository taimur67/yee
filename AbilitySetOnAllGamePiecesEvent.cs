using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200067B RID: 1659
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class AbilitySetOnAllGamePiecesEvent : GameEvent
	{
		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x00069408 File Offset: 0x00067608
		[JsonIgnore]
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Public;
			}
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0006940B File Offset: 0x0006760B
		[JsonConstructor]
		protected AbilitySetOnAllGamePiecesEvent()
		{
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x00069413 File Offset: 0x00067613
		public AbilitySetOnAllGamePiecesEvent(int triggeringPlayerID, GamePieceCategory affectedCategory, ConfigRef<ItemAbilityStaticData> itemAbility, bool added = true) : base(triggeringPlayerID)
		{
			this.AffectedCategory = affectedCategory;
			this.ItemAbility = itemAbility;
			this.WasAbilityAdded = added;
			base.AddAffectedPlayerId(triggeringPlayerID);
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x00069439 File Offset: 0x00067639
		public override string GetDebugName(TurnContext context)
		{
			return string.Format("GamePieces of category {0} have received ability {1}", this.AffectedCategory, this.ItemAbility);
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x00069458 File Offset: 0x00067658
		public override void DeepClone(out GameEvent clone)
		{
			AbilitySetOnAllGamePiecesEvent abilitySetOnAllGamePiecesEvent = new AbilitySetOnAllGamePiecesEvent
			{
				ItemAbility = this.ItemAbility.DeepClone<ItemAbilityStaticData>(),
				AffectedCategory = this.AffectedCategory,
				WasAbilityAdded = this.WasAbilityAdded
			};
			base.DeepCloneGameEventParts<AbilitySetOnAllGamePiecesEvent>(abilitySetOnAllGamePiecesEvent);
			clone = abilitySetOnAllGamePiecesEvent;
		}

		// Token: 0x04000CDC RID: 3292
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public ConfigRef<ItemAbilityStaticData> ItemAbility;

		// Token: 0x04000CDD RID: 3293
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public GamePieceCategory AffectedCategory;

		// Token: 0x04000CDE RID: 3294
		[JsonProperty]
		public bool WasAbilityAdded;
	}
}
