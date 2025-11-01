using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D2 RID: 722
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class BuffGamePieceHitPointsActiveRitual : ActiveRitual
	{
		// Token: 0x06000E11 RID: 3601 RVA: 0x00037934 File Offset: 0x00035B34
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameDatabase database = context.Database;
			BuffGamePieceHitPointsRitualData buffGamePieceHitPointsRitualData = database.Fetch<BuffGamePieceHitPointsRitualData>(base.StaticDataId);
			ItemAbilityStaticData itemAbilityStaticData = database.Fetch(buffGamePieceHitPointsRitualData.Ability);
			Ability ability = new Ability(itemAbilityStaticData);
			ability.Name = itemAbilityStaticData.Id;
			ability.SourceId = buffGamePieceHitPointsRitualData.Ability.Id;
			PlayerTargetGroup playerTargetGroup = new PlayerTargetGroup();
			playerTargetGroup.Targets.Add(base.TargetContext.PlayerId);
			playerTargetGroup.Abilities.Add(ability);
			IEnumerable<StatBoostPerLevelModifierStaticData> source = itemAbilityStaticData.GetModifiers().OfType<StatBoostPerLevelModifierStaticData>();
			playerTargetGroup.Modifiers = IEnumerableExtensions.ToList<IModifier>(from staticData in source
			select new StatBoostPerLevelModifier(staticData)
			{
				Source = ability
			});
			this._globalModifierId = currentTurn.PushGlobalModifier(playerTargetGroup);
			AbilitySetOnAllGamePiecesEvent ev = new AbilitySetOnAllGamePiecesEvent(player.Id, GamePieceCategory.PoP, buffGamePieceHitPointsRitualData.Ability, true);
			ritualCastEvent.AddChildEvent<AbilitySetOnAllGamePiecesEvent>(ev);
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(base.TargetContext.PlayerId, null));
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00037A4C File Offset: 0x00035C4C
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			currentTurn.PopGlobalModifier(this._globalModifierId);
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(base.TargetContext.PlayerId, null));
			BuffGamePieceHitPointsRitualData buffGamePieceHitPointsRitualData = context.Database.Fetch<BuffGamePieceHitPointsRitualData>(base.StaticDataId);
			AbilitySetOnAllGamePiecesEvent ev = new AbilitySetOnAllGamePiecesEvent(player.Id, GamePieceCategory.PoP, buffGamePieceHitPointsRitualData.Ability, false);
			banishedEvent.AddChildEvent<AbilitySetOnAllGamePiecesEvent>(ev);
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00037ABC File Offset: 0x00035CBC
		public sealed override void DeepClone(out GameItem gameItem)
		{
			BuffGamePieceHitPointsActiveRitual buffGamePieceHitPointsActiveRitual = new BuffGamePieceHitPointsActiveRitual
			{
				_globalModifierId = this._globalModifierId
			};
			base.DeepCloneActiveRitualParts(buffGamePieceHitPointsActiveRitual);
			gameItem = buffGamePieceHitPointsActiveRitual;
		}

		// Token: 0x0400063E RID: 1598
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
