using System;
using Game.Simulation.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D9 RID: 729
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HellsMawActiveRitual : ActiveRitual
	{
		// Token: 0x06000E3C RID: 3644 RVA: 0x0003852C File Offset: 0x0003672C
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			GameDatabase database = context.Database;
			HellsMawRitualData hellsMawRitualData = database.Fetch<HellsMawRitualData>(base.StaticDataId);
			ItemAbilityStaticData itemAbilityStaticData = database.Fetch(hellsMawRitualData.Ability);
			Ability ability = new Ability(itemAbilityStaticData);
			ability.Name = itemAbilityStaticData.Id;
			ability.SourceId = hellsMawRitualData.Ability.Id;
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
			AbilitySetOnAllGamePiecesEvent ev = new AbilitySetOnAllGamePiecesEvent(player.Id, GamePieceCategory.Legion, hellsMawRitualData.Ability, true);
			ritualCastEvent.AddChildEvent<AbilitySetOnAllGamePiecesEvent>(ev);
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(base.TargetContext.PlayerId, null));
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x000385FC File Offset: 0x000367FC
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			currentTurn.PopGlobalModifier(this._globalModifierId);
			context.RecalculateAllModifiersFor(currentTurn.FindPlayerState(base.TargetContext.PlayerId, null));
			HellsMawRitualData hellsMawRitualData = context.Database.Fetch<HellsMawRitualData>(base.StaticDataId);
			banishedEvent.AddChildEvent<AbilitySetOnAllGamePiecesEvent>(new AbilitySetOnAllGamePiecesEvent(player.Id, GamePieceCategory.Legion, hellsMawRitualData.Ability, false));
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0003866C File Offset: 0x0003686C
		public override void DeepClone(out GameItem gameItem)
		{
			HellsMawActiveRitual hellsMawActiveRitual = new HellsMawActiveRitual
			{
				_globalModifierId = this._globalModifierId
			};
			base.DeepCloneActiveRitualParts(hellsMawActiveRitual);
			gameItem = hellsMawActiveRitual;
		}

		// Token: 0x0400064D RID: 1613
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
