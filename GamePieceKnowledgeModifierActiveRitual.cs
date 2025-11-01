using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D7 RID: 727
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceKnowledgeModifierActiveRitual : ActiveRitual
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x000380F4 File Offset: 0x000362F4
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x000380FC File Offset: 0x000362FC
		[JsonProperty]
		public GamePieceModifier Modifier { get; set; }

		// Token: 0x06000E30 RID: 3632 RVA: 0x00038108 File Offset: 0x00036308
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			GamePieceModifierStaticData data = new GamePieceModifierStaticData().SetValue(this.TargetStat.ToGamePieceStat(), (float)this.ModifierValue, ModifierTarget.ValueOffset);
			this.Modifier = new GamePieceModifier(data);
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.Modifier.Data.EffectiveModifications(gamePiece))
			{
				ritualCastEvent.AddChildEvent<ModifyGamePieceKnowledgeEvent>(new ModifyGamePieceKnowledgeEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, false));
			}
			TurnState currentTurn = context.CurrentTurn;
			GamePieceBaseKnowledgeModifier gamePieceBaseKnowledgeModifier = new GamePieceBaseKnowledgeModifier(base.TargetContext.ItemId, this.Modifier);
			currentTurn.AddKnowledgeModifier(BitMask.AllBut(new int[]
			{
				player.Id
			}), gamePieceBaseKnowledgeModifier, false);
			this.KnowledgeModifierId = gamePieceBaseKnowledgeModifier.Guid;
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00038214 File Offset: 0x00036414
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			context.RemoveKnowledgeFromPlayers(this.KnowledgeModifierId);
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (gamePiece != null)
			{
				foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.Modifier.Data.EffectiveModifications(gamePiece))
				{
					banishedEvent.AddChildEvent<ModifyGamePieceKnowledgeEvent>(new ModifyGamePieceKnowledgeEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, true));
				}
			}
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000382BC File Offset: 0x000364BC
		public sealed override void DeepClone(out GameItem gameItem)
		{
			GamePieceKnowledgeModifierActiveRitual gamePieceKnowledgeModifierActiveRitual = new GamePieceKnowledgeModifierActiveRitual();
			base.DeepCloneActiveRitualParts(gamePieceKnowledgeModifierActiveRitual);
			gamePieceKnowledgeModifierActiveRitual.KnowledgeModifierId = this.KnowledgeModifierId;
			gamePieceKnowledgeModifierActiveRitual.TargetStat = this.TargetStat;
			gamePieceKnowledgeModifierActiveRitual.ModifierValue = this.ModifierValue;
			gamePieceKnowledgeModifierActiveRitual.Modifier = this.Modifier.DeepClone(CloneFunction.FastClone);
			gameItem = gamePieceKnowledgeModifierActiveRitual;
		}

		// Token: 0x04000647 RID: 1607
		[JsonProperty]
		public Guid KnowledgeModifierId;

		// Token: 0x04000648 RID: 1608
		[JsonProperty]
		public CombatStatType TargetStat;

		// Token: 0x04000649 RID: 1609
		[JsonProperty]
		public int ModifierValue;
	}
}
