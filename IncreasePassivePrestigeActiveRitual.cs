using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002DA RID: 730
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class IncreasePassivePrestigeActiveRitual : ActiveRitual
	{
		// Token: 0x06000E40 RID: 3648 RVA: 0x000386A0 File Offset: 0x000368A0
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameDatabase database = context.Database;
			IncreasePassivePrestigeRitualData increasePassivePrestigeRitualData = database.Fetch<IncreasePassivePrestigeRitualData>(base.StaticDataId);
			ItemAbilityStaticData itemAbilityStaticData = database.Fetch(increasePassivePrestigeRitualData.Ability);
			Ability ability = new Ability(itemAbilityStaticData);
			ability.Name = itemAbilityStaticData.Id;
			ability.SourceId = increasePassivePrestigeRitualData.Ability.Id;
			this._globalModifierId = currentTurn.PushGlobalModifier(new PlayerTargetGroup
			{
				Targets = 
				{
					base.TargetContext.PlayerId
				},
				Abilities = 
				{
					ability
				}
			});
			AbilitySetOnAllGamePiecesEvent ev = new AbilitySetOnAllGamePiecesEvent(player.Id, GamePieceCategory.PoP, increasePassivePrestigeRitualData.Ability, true);
			ritualCastEvent.AddChildEvent<AbilitySetOnAllGamePiecesEvent>(ev);
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00038758 File Offset: 0x00036958
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			currentTurn.PopGlobalModifier(this._globalModifierId);
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(base.TargetContext.PlayerId, null));
			IncreasePassivePrestigeRitualData increasePassivePrestigeRitualData = context.Database.Fetch<IncreasePassivePrestigeRitualData>(base.StaticDataId);
			AbilitySetOnAllGamePiecesEvent ev = new AbilitySetOnAllGamePiecesEvent(player.Id, GamePieceCategory.PoP, increasePassivePrestigeRitualData.Ability, false);
			banishedEvent.AddChildEvent<AbilitySetOnAllGamePiecesEvent>(ev);
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x000387C8 File Offset: 0x000369C8
		public override Result OnProcessPrestige(TurnProcessContext context, PlayerState player)
		{
			IncreasePassivePrestigeRitualData increasePassivePrestigeRitualData = context.Database.Fetch<IncreasePassivePrestigeRitualData>(base.StaticDataId);
			using (IEnumerator<GamePiece> enumerator = (from gamePiece in context.CurrentTurn.GetActiveGamePiecesForPlayer(player.Id)
			where gamePiece.IsFixture()
			select gamePiece).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.HasTag<EntityTag_CannotGeneratePrestige>())
					{
						int randomRoll = context.CurrentTurn.GetRandomRoll(increasePassivePrestigeRitualData.MinPrestigeGeneration, increasePassivePrestigeRitualData.MaxPrestigeGeneration, player.HasTag<EntityTag_CheatLuckyRitualEffectRolls>());
						PaymentReceivedEvent gameEvent = context.GivePrestige(player, randomRoll);
						context.CurrentTurn.AddGameEvent<PaymentReceivedEvent>(gameEvent);
					}
				}
			}
			return Result.Success;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x00038890 File Offset: 0x00036A90
		public sealed override void DeepClone(out GameItem gameItem)
		{
			IncreasePassivePrestigeActiveRitual increasePassivePrestigeActiveRitual = new IncreasePassivePrestigeActiveRitual
			{
				_globalModifierId = this._globalModifierId
			};
			base.DeepCloneActiveRitualParts(increasePassivePrestigeActiveRitual);
			gameItem = increasePassivePrestigeActiveRitual;
		}

		// Token: 0x0400064E RID: 1614
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
