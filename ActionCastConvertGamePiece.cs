using System;

namespace LoG
{
	// Token: 0x020000EE RID: 238
	public class ActionCastConvertGamePiece : ActionCastRitual<ConvertGamePieceRitualOrder>
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00010E62 File Offset: 0x0000F062
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Steal_Legion;
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00010E66 File Offset: 0x0000F066
		protected override string GetRitualId()
		{
			return "convert_legion";
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00010E6D File Offset: 0x0000F06D
		protected override PowerType GetPowerType()
		{
			return PowerType.Deceit;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00010E70 File Offset: 0x0000F070
		protected override Identifier GetTargetItemId()
		{
			return this._targetItemId;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00010E78 File Offset: 0x0000F078
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00010E7B File Offset: 0x0000F07B
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00010E7E File Offset: 0x0000F07E
		public ActionCastConvertGamePiece(Identifier targetItemId)
		{
			this._targetItemId = targetItemId;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00010E90 File Offset: 0x0000F090
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			int targetPlayerId = this.GetTargetPlayerId();
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindPlayerState(targetPlayerId, null);
			if (playerState == null || playerState.Id == -2147483648 || playerState.Id == -1)
			{
				base.Disable(string.Format("Invalid target player {0}", targetPlayerId));
				return;
			}
			GamePiece gamePiece = this.OwningPlanner.TrueTurn.FetchGameItem<GamePiece>(this._targetItemId);
			if (gamePiece == null)
			{
				base.Disable(string.Format("Invalid target item {0}", this._targetItemId));
				return;
			}
			base.AddPrecondition(new WPSpawnPoint());
			base.AddPrecondition(new WPCommandRating(gamePiece.CommandCost));
			if (gamePiece.SubCategory == GamePieceCategory.Titan)
			{
				base.AddEffect(new WPHasTitan());
			}
			foreach (PlayerState playerState2 in this.OwningPlanner.TrueTurn.EnumeratePlayerStatesExcept(this.OwningPlanner.PlayerId, false, false))
			{
				base.AddEffect(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, playerState2.Id, 0.5f));
			}
			base.AddScalarCostModifier(-0.95f, PFCostModifier.Heuristic_Bonus);
			base.AddEffect(new WPUndermineArchfiend(targetPlayerId));
		}

		// Token: 0x0400021E RID: 542
		public const string RitualId = "convert_legion";

		// Token: 0x0400021F RID: 543
		private readonly Identifier _targetItemId;
	}
}
