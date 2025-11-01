using System;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002E0 RID: 736
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class ShadowBindingActiveRitual : ActiveRitual
	{
		// Token: 0x06000E62 RID: 3682 RVA: 0x000393D4 File Offset: 0x000375D4
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			if (currentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId) == null)
			{
				return Result.Failure;
			}
			GameDatabase database = context.Database;
			ShadowBindingRitualData shadowBindingRitualData = database.Fetch<ShadowBindingRitualData>(base.StaticDataId);
			ItemAbilityStaticData itemAbilityStaticData = database.Fetch<ItemAbilityStaticData>(shadowBindingRitualData.ShadowBindingDebuff.Id);
			Ability ability = new Ability(itemAbilityStaticData)
			{
				Name = shadowBindingRitualData.ShadowBindingDebuff.Id,
				SourceId = shadowBindingRitualData.ShadowBindingDebuff.Id
			};
			GameItemTargetGroup gameItemTargetGroup = new GameItemTargetGroup();
			gameItemTargetGroup.Targets.Add(base.TargetContext.ItemId);
			gameItemTargetGroup.Abilities.Add(ability);
			foreach (ModifierStaticData modifierStaticData in itemAbilityStaticData.GetModifiers())
			{
				GamePieceModifierStaticData gamePieceModifierStaticData = modifierStaticData as GamePieceModifierStaticData;
				if (gamePieceModifierStaticData != null)
				{
					gameItemTargetGroup.Modifiers.Add(new GamePieceModifier(gamePieceModifierStaticData)
					{
						Source = new RitualContext(player.Id, player.ArchfiendId, shadowBindingRitualData.Id)
					});
				}
			}
			this._globalModifierId = (ability.ModifierGroupId = currentTurn.PushGlobalModifier(gameItemTargetGroup));
			this.RecalculateTargetModifiers(context);
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00039514 File Offset: 0x00037714
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			ShadowBindingRitualData shadowBindingRitualData = context.Database.Fetch<ShadowBindingRitualData>(base.StaticDataId);
			GamePiece targetGamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			banishedEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(player.Id, targetGamePiece, shadowBindingRitualData.ShadowBindingDebuff, true));
			currentTurn.PopGlobalModifier(this._globalModifierId);
			this.RecalculateTargetModifiers(context);
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00039588 File Offset: 0x00037788
		private void RecalculateTargetModifiers(TurnProcessContext context)
		{
			PlayerState player = context.CurrentTurn.FindControllingPlayer(base.TargetContext.ItemId);
			context.RecalculateAllModifiersFor(player);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x000395B4 File Offset: 0x000377B4
		public sealed override void DeepClone(out GameItem gameItem)
		{
			ShadowBindingActiveRitual shadowBindingActiveRitual = new ShadowBindingActiveRitual();
			base.DeepCloneActiveRitualParts(shadowBindingActiveRitual);
			shadowBindingActiveRitual._globalModifierId = this._globalModifierId;
			gameItem = shadowBindingActiveRitual;
		}

		// Token: 0x0400065B RID: 1627
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
