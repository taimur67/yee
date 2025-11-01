using System;

namespace LoG
{
	// Token: 0x020000EF RID: 239
	public class ActionCastDamageGamePieceRitual : ActionCastRitual<DamageGamePieceRitualOrder>
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00010FE8 File Offset: 0x0000F1E8
		public override ActionID ID
		{
			get
			{
				if (!this._permanentDamage)
				{
					return ActionID.Cast_Damage_Gamepiece_Destruction;
				}
				return ActionID.Cast_Damage_Gamepiece_Destruction_Permanent;
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00010FF6 File Offset: 0x0000F1F6
		protected override string GetRitualId()
		{
			if (!this._permanentDamage)
			{
				return "infernal_affliction";
			}
			return "dire_dissipation";
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001100B File Offset: 0x0000F20B
		protected override PowerType GetPowerType()
		{
			return PowerType.Destruction;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0001100E File Offset: 0x0000F20E
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00011011 File Offset: 0x0000F211
		protected override int CooldownDuration
		{
			get
			{
				if (!this._permanentDamage)
				{
					return 3;
				}
				return 2;
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001101E File Offset: 0x0000F21E
		protected override Identifier GetTargetItemId()
		{
			return this._targetGamePieceID;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00011026 File Offset: 0x0000F226
		public ActionCastDamageGamePieceRitual(Identifier targetGamePieceId, bool permanentDamage = false)
		{
			this._targetGamePieceID = targetGamePieceId;
			this._permanentDamage = permanentDamage;
			base.AddScalarCostModifier(this._permanentDamage ? -0.5f : 0.1f, PFCostModifier.Heuristic_Bonus);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00011058 File Offset: 0x0000F258
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			int targetPlayerId = this.GetTargetPlayerId();
			if (!base.IsMasked)
			{
				base.AddPrecondition(new WPCanAttack(targetPlayerId, false));
			}
			PlayerState playerState = this.OwningPlanner.PlayerState;
			if (!base.IsAnyVariantAlreadyOngoing("lilith_baleful_gaze") && ActionBalefulGaze.CanBeUsedByArchfiend(playerState) && this.OwningPlanner.AIPreviewPlayerState.RitualState.AvailableSlots >= 2)
			{
				base.AddPrecondition(new WPTrueKnowledgeGamePiece(this._targetGamePieceID));
			}
			Identifier targetItemId = this.GetTargetItemId();
			GamePiece pandaemonium = this.OwningPlanner.TrueTurn.GetPandaemonium();
			if (targetItemId == pandaemonium.Id && !this.OwningPlanner.PlayerState.Excommunicated)
			{
				base.AddPrecondition(WPForcesFlanking.RequiredTargetToBeFlanked(this._targetGamePieceID));
			}
			GamePiece gamePiece;
			if (!this.OwningPlanner.AIPreviewTurn.TryFetchGameItem<GamePiece>(targetItemId, out gamePiece))
			{
				base.Disable(string.Format("Invalid target item {0}", targetItemId));
				return;
			}
			this.AddEffectsIfAttackingDarkPylon(gamePiece);
			if (!gamePiece.IsDestructible())
			{
				PlayerState playerState2 = this.OwningPlanner.AIPreviewTurn.FindPlayerState(targetPlayerId, null);
				Identifier identifier = targetItemId;
				Identifier? identifier2 = (playerState2 != null) ? new Identifier?(playerState2.StrongholdId) : null;
				if (identifier == identifier2.GetValueOrDefault() & identifier2 != null)
				{
					base.AddPrecondition(new WPCanEliminate(targetPlayerId));
				}
				if (this._permanentDamage)
				{
					if (gamePiece.TotalHP <= gamePiece.TotalHP.LowerBound)
					{
						base.Disable("Target already has minimum HP");
						return;
					}
				}
				else
				{
					if (gamePiece.HP <= 1)
					{
						base.Disable("Target already has minimum HP");
						return;
					}
					base.AddPrecondition(new WPHasForcesNearby(gamePiece, 2));
				}
			}
			base.AddEffect(new WPCombatVsGamepiece(this._targetGamePieceID));
			base.AddEffect(new WPCombatVsPlayer(targetPlayerId));
			base.AddEffect(new WPUndermineArchfiend(targetPlayerId));
		}

		// Token: 0x04000220 RID: 544
		public const string RitualId = "infernal_affliction";

		// Token: 0x04000221 RID: 545
		public const string RitualIdPermanent = "dire_dissipation";

		// Token: 0x04000222 RID: 546
		private readonly bool _permanentDamage;

		// Token: 0x04000223 RID: 547
		private readonly Identifier _targetGamePieceID;
	}
}
