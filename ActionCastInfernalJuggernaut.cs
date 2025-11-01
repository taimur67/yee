using System;

namespace LoG
{
	// Token: 0x020000F7 RID: 247
	public class ActionCastInfernalJuggernaut : ActionCastRitual<InfernalJuggernautRitualOrder>
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0001198D File Offset: 0x0000FB8D
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Buff_Legion_Wrath;
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00011991 File Offset: 0x0000FB91
		protected override string GetRitualId()
		{
			return "infernal_juggernaut";
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00011998 File Offset: 0x0000FB98
		protected override PowerType GetPowerType()
		{
			return PowerType.Wrath;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001199B File Offset: 0x0000FB9B
		protected override Identifier GetTargetItemId()
		{
			return this._targetGamePieceID;
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000119A3 File Offset: 0x0000FBA3
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x000119A6 File Offset: 0x0000FBA6
		protected override int CooldownDuration
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000119A9 File Offset: 0x0000FBA9
		public ActionCastInfernalJuggernaut(Identifier targetGamePieceId)
		{
			this._targetGamePieceID = targetGamePieceId;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000119B8 File Offset: 0x0000FBB8
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			GamePiece gamePiece = this.OwningPlanner.TrueTurn.FetchGameItem<GamePiece>(this._targetGamePieceID);
			if (gamePiece == null)
			{
				base.Disable("Not a game piece");
				return;
			}
			if (gamePiece.GroundMoveDistance <= 0)
			{
				base.AddScalarCostReduction(1f, PFCostModifier.Heuristic_Bonus);
			}
			else
			{
				base.AddScalarCostReduction(0.9f, PFCostModifier.Heuristic_Bonus);
			}
			CombatStats combatStats = new CombatStats();
			combatStats.Ranged = 10;
			combatStats.Melee = 10;
			combatStats.Infernal = 10;
			base.AddEffect(WPCombatAdvantage.BonusFor(this._targetGamePieceID, combatStats));
		}

		// Token: 0x04000230 RID: 560
		public const string RitualId = "infernal_juggernaut";

		// Token: 0x04000231 RID: 561
		private readonly Identifier _targetGamePieceID;
	}
}
