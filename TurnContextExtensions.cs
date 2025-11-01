using System;
using System.Collections.Generic;
using System.Linq;
using Core.StaticData;
using Game.Simulation.StaticData;
using Game.StaticData;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x0200058A RID: 1418
	public static class TurnContextExtensions
	{
		// Token: 0x06001AF5 RID: 6901 RVA: 0x0005E064 File Offset: 0x0005C264
		public static bool IsMultiplayer(this TurnContext context)
		{
			return context.CurrentTurn.EnumeratePlayerStates(false, true).Count(delegate(PlayerState t)
			{
				string playFabId = t.PlayFabId;
				return playFabId != null && playFabId.Length > 0;
			}) > 1;
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0005E09A File Offset: 0x0005C29A
		public static IEnumerable<GameEvent> InstallModifierAsPermanentAdjustment(this TurnProcessContext context, PlayerState instigator, PlayerState target, ArchfiendModifierStaticData modifier)
		{
			foreach (StatModificationBinding<ArchfiendStat> statModificationBinding in modifier.AllBindings)
			{
				ModifiableValue modifiableValue;
				if (target.TryGet(statModificationBinding.StatKey, out modifiableValue))
				{
					int clampedAdjustment = (int)modifier.CalculatePowerChange(target, statModificationBinding.StatKey);
					GameEvent gameEvent;
					context.TryPermanentlyAdjustArchfiendStat(instigator, target, statModificationBinding.StatKey, (int)statModificationBinding.Value, clampedAdjustment, out gameEvent);
					if (gameEvent != null)
					{
						yield return gameEvent;
					}
				}
			}
			IEnumerator<StatModificationBinding<ArchfiendStat>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0005E0BF File Offset: 0x0005C2BF
		public static bool TryPermanentlyAdjustArchfiendStat(this TurnProcessContext context, PlayerState instigator, PlayerState player, PowerType powerType, int attemptedAdjustment, int clampedAdjustment, out GameEvent ev)
		{
			return context.TryPermanentlyAdjustArchfiendStat(instigator, player, powerType.ToArchfiendStat(), attemptedAdjustment, clampedAdjustment, out ev);
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0005E0D8 File Offset: 0x0005C2D8
		public static bool TryGetProtectingGameItem(this TurnProcessContext context, PlayerState target, ArchfiendStat statType, out GameItem protectingItem)
		{
			protectingItem = null;
			Func<ConfigRef<ItemAbilityStaticData>, ItemAbilityStaticData> <>9__2;
			Func<EntityTag_ImmuneToNegativeStatChanges, bool> <>9__5;
			foreach (GameItem gameItem in from itemId in context.CurrentTurn.GetGameIdentifiersControlledBy(target.Id, true)
			select context.CurrentTurn.FetchGameItem(itemId) into item
			where item != null
			select item)
			{
				if (!gameItem.CanBePlacedInVault || context.CurrentTurn.HasControllingGamePiece(gameItem.Id))
				{
					GameItemStaticData gameItemStaticData = context.Database.Fetch<GameItemStaticData>(gameItem.StaticDataId);
					if (gameItemStaticData != null && gameItemStaticData._providedAbilities != null)
					{
						IEnumerable<ConfigRef<ItemAbilityStaticData>> providedAbilities = gameItemStaticData._providedAbilities;
						Func<ConfigRef<ItemAbilityStaticData>, ItemAbilityStaticData> selector;
						if ((selector = <>9__2) == null)
						{
							selector = (<>9__2 = ((ConfigRef<ItemAbilityStaticData> abilityRef) => context.Database.Fetch(abilityRef)));
						}
						IEnumerable<EntityTag_ImmuneToNegativeStatChanges> source = providedAbilities.Select(selector).SelectMany((ItemAbilityStaticData abilityStaticData) => ((abilityStaticData != null) ? abilityStaticData.GetModifiers() : null) ?? Enumerable.Empty<ModifierStaticData>()).SelectMany(delegate(ModifierStaticData modifierStaticData)
						{
							IEnumerable<EntityTag> enumerable = (modifierStaticData != null) ? modifierStaticData.Tags : null;
							return enumerable ?? Enumerable.Empty<EntityTag>();
						}).OfType<EntityTag_ImmuneToNegativeStatChanges>();
						Func<EntityTag_ImmuneToNegativeStatChanges, bool> predicate;
						if ((predicate = <>9__5) == null)
						{
							predicate = (<>9__5 = ((EntityTag_ImmuneToNegativeStatChanges t) => t.ProtectsAgainstStat(statType)));
						}
						if (source.Any(predicate))
						{
							protectingItem = gameItem;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0005E288 File Offset: 0x0005C488
		public static bool TryPermanentlyAdjustArchfiendStat(this TurnProcessContext context, PlayerState instigator, PlayerState target, ArchfiendStat statType, int attemptedAdjustment, int clampedAdjustment, out GameEvent ev)
		{
			ev = null;
			GameItem gameItem;
			if (attemptedAdjustment < 0 && context.TryGetProtectingGameItem(target, statType, out gameItem))
			{
				ev = new ProtectedFromArchfiendModificationEvent(instigator.Id, target, statType, gameItem.Id);
				return false;
			}
			if (clampedAdjustment == 0)
			{
				ev = new ModifyArchfiendEvent((instigator != null) ? instigator.Id : int.MinValue, target, statType, attemptedAdjustment, 0, false);
				return false;
			}
			ev = context.PermanentlyAdjustArchfiendStat(instigator, target, statType, attemptedAdjustment, clampedAdjustment);
			return ev != null;
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0005E2FC File Offset: 0x0005C4FC
		public static GameEvent PermanentlyAdjustArchfiendStat(this TurnProcessContext context, PlayerState instigator, PlayerState target, ArchfiendStat statType, int attemptedAdjustment, int clampedAdjustment)
		{
			ModifiableValue modifiableValue;
			if (!target.TryGet(statType, out modifiableValue))
			{
				return null;
			}
			if (clampedAdjustment < 0)
			{
				EntityTag_PreventArchfiendStatBaseAdjustmentsBelowValue entityTag_PreventArchfiendStatBaseAdjustmentsBelowValue = (from t in target.EnumerateTags<EntityTag_PreventArchfiendStatBaseAdjustmentsBelowValue>()
				where t.Stat == statType
				select t).SelectMaxOrDefault((EntityTag_PreventArchfiendStatBaseAdjustmentsBelowValue t) => t.Threshold, null);
				if (entityTag_PreventArchfiendStatBaseAdjustmentsBelowValue != null && modifiableValue.BaseValue + clampedAdjustment < entityTag_PreventArchfiendStatBaseAdjustmentsBelowValue.Threshold)
				{
					clampedAdjustment = Math.Min(0, modifiableValue.BaseValue - entityTag_PreventArchfiendStatBaseAdjustmentsBelowValue.Threshold);
				}
			}
			if (clampedAdjustment == 0)
			{
				return new ModifyArchfiendEvent((instigator != null) ? instigator.Id : int.MinValue, target, statType, attemptedAdjustment, 0, false);
			}
			GameEvent result = new ModifyArchfiendEvent((instigator != null) ? instigator.Id : int.MinValue, target, statType, attemptedAdjustment, clampedAdjustment, false);
			modifiableValue.AdjustBase((float)clampedAdjustment);
			return result;
		}
	}
}
