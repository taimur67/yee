using System;

namespace LoG
{
	// Token: 0x020000F5 RID: 245
	public class ActionCastHealGamePiece : ActionCastRitual<HealGamePieceRitualOrder>
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x0001181F File Offset: 0x0000FA1F
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Heal;
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00011823 File Offset: 0x0000FA23
		protected override string GetRitualId()
		{
			return "undying_vigor";
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001182A File Offset: 0x0000FA2A
		protected override PowerType GetPowerType()
		{
			return PowerType.Wrath;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001182D File Offset: 0x0000FA2D
		protected override Identifier GetTargetItemId()
		{
			return this._targetGamePieceID;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00011835 File Offset: 0x0000FA35
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00011838 File Offset: 0x0000FA38
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001183B File Offset: 0x0000FA3B
		public ActionCastHealGamePiece(Identifier targetGamePieceID)
		{
			this._targetGamePieceID = targetGamePieceID;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001184C File Offset: 0x0000FA4C
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			base.AddConstraint(new WPLegionBadlyDamaged(this._targetGamePieceID, 0.5f));
			base.AddEffect(new WPOpportunisticHeal());
			base.AddEffect(WPCombatAdvantage.FromHealing(this._targetGamePieceID, 1f));
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x000118A0 File Offset: 0x0000FAA0
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			ObjectiveCondition_CastHealingRituals objectiveCondition_CastHealingRituals = objectiveCondition as ObjectiveCondition_CastHealingRituals;
			if (objectiveCondition_CastHealingRituals != null)
			{
				if (objectiveCondition_CastHealingRituals.MinimumHealing > 0)
				{
					GamePiece gamePiece = this.OwningPlanner.TrueTurn.FetchGameItem<GamePiece>(this.GetTargetItemId());
					if (gamePiece != null && gamePiece.TotalHP - gamePiece.HP < objectiveCondition_CastHealingRituals.MinimumHealing)
					{
						return false;
					}
				}
				return true;
			}
			return base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x0400022D RID: 557
		public const string RitualId = "undying_vigor";

		// Token: 0x0400022E RID: 558
		private readonly Identifier _targetGamePieceID;
	}
}
