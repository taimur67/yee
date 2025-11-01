using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002E2 RID: 738
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TrueKnowledgeActiveRitual : ActiveRitual
	{
		// Token: 0x06000E6A RID: 3690 RVA: 0x00039674 File Offset: 0x00037874
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			EntityTag_RitualVulnerability entityTag_RitualVulnerability = new EntityTag_RitualVulnerability();
			entityTag_RitualVulnerability.VulnerableToPlayers.Set(player.Id);
			entityTag_RitualVulnerability.Value = this.VulnerabilityValue;
			GameItemTargetGroup modifiableTargetGroup = new GameItemTargetGroup(EntityTagUtils.CreateModifierWithTag<EntityTag_RitualVulnerability>(entityTag_RitualVulnerability), new Identifier[]
			{
				base.TargetContext.ItemId
			});
			TurnState currentTurn = context.CurrentTurn;
			this.VulnerabilityModifierId = currentTurn.PushGlobalModifier(modifiableTargetGroup);
			GamePieceTruthKnowledgeModifier gamePieceTruthKnowledgeModifier = new GamePieceTruthKnowledgeModifier(base.TargetContext.ItemId);
			currentTurn.AddKnowledgeModifier(player, gamePieceTruthKnowledgeModifier, false);
			this.KnowledgeModifierId = gamePieceTruthKnowledgeModifier.Guid;
			this.RecalculateTargetModifiers(context);
			GamePiece gamePiece = currentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (gamePiece == null)
			{
				return Result.Failure;
			}
			ritualCastEvent.AddChildEvent<GamePieceStratagemVisibilityEvent>(new GamePieceStratagemVisibilityEvent(player.Id, gamePiece, true));
			ritualCastEvent.AddChildEvent<ModifyGameItemRitualResistanceEvent>(new ModifyGameItemRitualResistanceEvent(player.Id, gamePiece.ControllingPlayerId, gamePiece, this.VulnerabilityValue));
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00039760 File Offset: 0x00037960
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			TurnState currentTurn = context.CurrentTurn;
			context.RemoveKnowledgeFromPlayers(this.KnowledgeModifierId);
			currentTurn.PopGlobalModifier(this.VulnerabilityModifierId);
			this.RecalculateTargetModifiers(context);
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (gamePiece == null)
			{
				return Result.Failure;
			}
			banishedEvent.AddChildEvent<GamePieceStratagemVisibilityEvent>(new GamePieceStratagemVisibilityEvent(player.Id, gamePiece, false));
			banishedEvent.AddChildEvent<ModifyGameItemRitualResistanceEvent>(new ModifyGameItemRitualResistanceEvent(player.Id, gamePiece.ControllingPlayerId, gamePiece, 0));
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x000397E8 File Offset: 0x000379E8
		private void RecalculateTargetModifiers(TurnProcessContext context)
		{
			PlayerState player;
			if (context.CurrentTurn.TryFindControllingPlayer(base.TargetContext.ItemId, out player))
			{
				context.RecalculateAllModifiersFor(player);
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00039818 File Offset: 0x00037A18
		public sealed override void DeepClone(out GameItem gameItem)
		{
			TrueKnowledgeActiveRitual trueKnowledgeActiveRitual = new TrueKnowledgeActiveRitual();
			base.DeepCloneActiveRitualParts(trueKnowledgeActiveRitual);
			trueKnowledgeActiveRitual.KnowledgeModifierId = this.KnowledgeModifierId;
			trueKnowledgeActiveRitual.VulnerabilityValue = this.VulnerabilityValue;
			trueKnowledgeActiveRitual.VulnerabilityModifierId = this.VulnerabilityModifierId;
			gameItem = trueKnowledgeActiveRitual;
		}

		// Token: 0x0400065C RID: 1628
		[JsonProperty]
		public Guid KnowledgeModifierId;

		// Token: 0x0400065D RID: 1629
		[JsonProperty]
		public int VulnerabilityValue;

		// Token: 0x0400065E RID: 1630
		[JsonProperty]
		private Guid VulnerabilityModifierId;
	}
}
