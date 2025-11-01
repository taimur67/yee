using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002D8 RID: 728
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GamePieceModifierActiveRitual : ActiveRitual
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00038317 File Offset: 0x00036517
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x0003831F File Offset: 0x0003651F
		[JsonProperty]
		public GamePieceModifier Modifier { get; set; }

		// Token: 0x06000E36 RID: 3638 RVA: 0x00038328 File Offset: 0x00036528
		public override Result StartRitual(TurnProcessContext context, PlayerState player, RitualCastEvent ritualCastEvent)
		{
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.Modifier.Data.EffectiveModifications(gamePiece))
			{
				ritualCastEvent.AddChildEvent<ModifyGamePieceEvent>(new ModifyGamePieceEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, false));
			}
			GameItemTargetGroup modifiableTargetGroup = new GameItemTargetGroup(this.Modifier, new Identifier[]
			{
				base.TargetContext.ItemId
			});
			this._globalModifierId = context.CurrentTurn.PushGlobalModifier(modifiableTargetGroup);
			this.RecalculateTargetModifiers(context);
			return base.StartRitual(context, player, ritualCastEvent);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x000383F8 File Offset: 0x000365F8
		public override Result EndRitual(TurnProcessContext context, PlayerState player, ItemBanishedEvent banishedEvent)
		{
			context.CurrentTurn.PopGlobalModifier(this._globalModifierId);
			this.RecalculateTargetModifiers(context);
			GamePiece gamePiece = context.CurrentTurn.FetchGameItem<GamePiece>(base.TargetContext.ItemId);
			if (gamePiece != null)
			{
				foreach (ModifierStaticDataExtensions.GamePieceStatModification gamePieceStatModification in this.Modifier.Data.EffectiveModifications(gamePiece))
				{
					banishedEvent.AddChildEvent<ModifyGamePieceEvent>(new ModifyGamePieceEvent(player.Id, gamePiece, gamePieceStatModification.Stat, gamePieceStatModification.AttemptedOffset, gamePieceStatModification.EffectiveOffset, true));
				}
			}
			return base.EndRitual(context, player, banishedEvent);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000384AC File Offset: 0x000366AC
		private void RecalculateTargetModifiers(TurnProcessContext context)
		{
			PlayerState playerState = context.CurrentTurn.FindControllingPlayer(base.TargetContext.ItemId);
			if (playerState == null)
			{
				return;
			}
			context.RecalculateAllModifiersFor(playerState);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000384DB File Offset: 0x000366DB
		protected void DeepCloneGamePieceModifierActiveRitualParts(GamePieceModifierActiveRitual clone)
		{
			clone._globalModifierId = this._globalModifierId;
			clone.Modifier = this.Modifier.DeepClone(CloneFunction.FastClone);
			base.DeepCloneActiveRitualParts(clone);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00038504 File Offset: 0x00036704
		public override void DeepClone(out GameItem gameItem)
		{
			GamePieceModifierActiveRitual gamePieceModifierActiveRitual = new GamePieceModifierActiveRitual();
			this.DeepCloneGamePieceModifierActiveRitualParts(gamePieceModifierActiveRitual);
			gameItem = gamePieceModifierActiveRitual;
		}

		// Token: 0x0400064B RID: 1611
		[JsonProperty]
		private Guid _globalModifierId;
	}
}
