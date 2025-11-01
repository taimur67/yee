using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D6 RID: 726
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceAbilityActiveRitual : ActiveRitual
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00037ECA File Offset: 0x000360CA
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x00037ED2 File Offset: 0x000360D2
		[JsonProperty]
		public ConfigRef<AbilityStaticData> AbilityDataConfigRef { get; set; }

		// Token: 0x06000E28 RID: 3624 RVA: 0x00037EDC File Offset: 0x000360DC
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			GamePiece targetGamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			ritualCastEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(player.Id, targetGamePiece, this.AbilityDataConfigRef, true));
			ItemAbilityStaticData itemAbilityStaticData = context.Database.Fetch<ItemAbilityStaticData>(this.AbilityDataConfigRef.Id);
			Ability item = new Ability(itemAbilityStaticData)
			{
				Name = this.AbilityDataConfigRef.Id,
				SourceId = this.AbilityDataConfigRef.Id
			};
			GameItemTargetGroup gameItemTargetGroup = new GameItemTargetGroup();
			gameItemTargetGroup.Targets.Add(base.TargetContext.ItemId);
			gameItemTargetGroup.Abilities.Add(item);
			foreach (ModifierStaticData modifierStaticData in itemAbilityStaticData.GetModifiers())
			{
				GamePieceModifierStaticData gamePieceModifierStaticData = modifierStaticData as GamePieceModifierStaticData;
				if (gamePieceModifierStaticData != null)
				{
					gameItemTargetGroup.Modifiers.Add(new GamePieceModifier(gamePieceModifierStaticData)
					{
						Source = new RitualContext(player.Id, player.ArchfiendId, ritualCastEvent.RitualId)
					});
				}
			}
			this._globalModifierId = context.CurrentTurn.PushGlobalModifier(gameItemTargetGroup);
			this.RecalculateTargetModifiers(context);
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00038014 File Offset: 0x00036214
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (gamePiece != null)
			{
				banishedEvent.AddChildEvent<AbilitySetOnGameItemEvent>(new AbilitySetOnGameItemEvent(player.Id, gamePiece, this.AbilityDataConfigRef, false));
			}
			context.CurrentTurn.PopGlobalModifier(this._globalModifierId);
			this.RecalculateTargetModifiers(context);
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00038078 File Offset: 0x00036278
		private void RecalculateTargetModifiers(TurnProcessContext context)
		{
			PlayerState playerState = context.CurrentTurn.FindControllingPlayer(base.TargetContext.ItemId);
			if (playerState == null)
			{
				return;
			}
			context.RecalculateAllModifiersFor(playerState);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x000380A7 File Offset: 0x000362A7
		public void DeepCloneGamePieceAbilityActiveRitualParts(GamePieceAbilityActiveRitual clone)
		{
			clone._globalModifierId = this._globalModifierId;
			clone.AbilityDataConfigRef = this.AbilityDataConfigRef.DeepClone<AbilityStaticData>();
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000380C8 File Offset: 0x000362C8
		public override void DeepClone(out GameItem gameItem)
		{
			GamePieceAbilityActiveRitual gamePieceAbilityActiveRitual = new GamePieceAbilityActiveRitual();
			base.DeepCloneActiveRitualParts(gamePieceAbilityActiveRitual);
			this.DeepCloneGamePieceAbilityActiveRitualParts(gamePieceAbilityActiveRitual);
			gameItem = gamePieceAbilityActiveRitual;
		}

		// Token: 0x04000645 RID: 1605
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
