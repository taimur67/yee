using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002DD RID: 733
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class MartyrGamePieceActiveRitual : ActiveRitual
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x00038E06 File Offset: 0x00037006
		// (set) Token: 0x06000E52 RID: 3666 RVA: 0x00038E0E File Offset: 0x0003700E
		[JsonProperty]
		public GamePieceModifierStaticData Modifier { get; set; }

		// Token: 0x06000E53 RID: 3667 RVA: 0x00038E18 File Offset: 0x00037018
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.Modifier.EffectiveModifications(gamePiece))
			{
				if (gamePieceStatModification.AttemptedOffset != 0)
				{
					ritualCastEvent.AddChildEvent<ModifyGamePieceEvent>(new ModifyGamePieceEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, false));
				}
			}
			GameItemTargetGroup modifiableTargetGroup = new GameItemTargetGroup(new GamePieceModifier(this.Modifier)
			{
				Source = new RitualContext(player.Id, player.ArchfiendId, base.StaticDataId)
			}, new Identifier[]
			{
				base.TargetContext.ItemId
			});
			this._globalModifierId = context.CurrentTurn.PushGlobalModifier(modifiableTargetGroup);
			this.RecalculateTargetModifiers(context);
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00038F10 File Offset: 0x00037110
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			if (player == null)
			{
				return Result.Failure;
			}
			context.CurrentTurn.PopGlobalModifier(this._globalModifierId);
			this.RecalculateTargetModifiers(context);
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (gamePiece != null)
			{
				foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.Modifier.EffectiveModifications(gamePiece))
				{
					banishedEvent.AddChildEvent<ModifyGamePieceEvent>(new ModifyGamePieceEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, true));
				}
			}
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00038FC8 File Offset: 0x000371C8
		private void RecalculateTargetModifiers(TurnContext context)
		{
			PlayerState player = context.CurrentTurn.FindControllingPlayer(base.TargetContext.ItemId);
			context.RecalculateAllModifiersFor(player);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00038FF4 File Offset: 0x000371F4
		public sealed override void DeepClone(out GameItem gameItem)
		{
			MartyrGamePieceActiveRitual martyrGamePieceActiveRitual = new MartyrGamePieceActiveRitual();
			base.DeepCloneActiveRitualParts(martyrGamePieceActiveRitual);
			martyrGamePieceActiveRitual._globalModifierId = this._globalModifierId;
			martyrGamePieceActiveRitual.Modifier = this.Modifier.DeepClone(CloneFunction.FastClone);
			gameItem = martyrGamePieceActiveRitual;
		}

		// Token: 0x04000655 RID: 1621
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
