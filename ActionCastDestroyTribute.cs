using System;

namespace LoG
{
	// Token: 0x020000F3 RID: 243
	public class ActionCastDestroyTribute : ActionCastRitual<DestroyTributeRitualOrder>
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x00011689 File Offset: 0x0000F889
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Destroy_Tribute;
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001168D File Offset: 0x0000F88D
		protected override string GetRitualId()
		{
			return "corrupt_tribute";
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00011694 File Offset: 0x0000F894
		protected override PowerType GetPowerType()
		{
			return PowerType.Destruction;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00011697 File Offset: 0x0000F897
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayerID;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001169F File Offset: 0x0000F89F
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003DB RID: 987 RVA: 0x000116A2 File Offset: 0x0000F8A2
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000116A5 File Offset: 0x0000F8A5
		public ActionCastDestroyTribute(int targetPlayerID)
		{
			this._targetPlayerID = targetPlayerID;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000116B4 File Offset: 0x0000F8B4
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
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindPlayerState(this._targetPlayerID, null);
			if (playerState == null || playerState.Id == -2147483648 || playerState.Id == -1)
			{
				base.Disable(string.Format("Invalid target player {0}", this._targetPlayerID));
				return;
			}
			PlayerState playerState2 = this.OwningPlanner.PlayerState;
			int value = playerState2.PowersLevels[PowerType.Destruction].CurrentLevel.Value;
			if (playerState.Resources.Count < value / 3 + 2)
			{
				base.Disable(string.Format("Target player {0} doesn't have enough tokens to destroy", this._targetPlayerID));
				return;
			}
			base.AddEffect(new WPUndermineArchfiend(this._targetPlayerID));
			if (WPHasTitan.Check(this.OwningPlanner.TrueTurn, playerState))
			{
				base.AddEffect(new WPMilitarySuperiority(playerState2.Id, this._targetPlayerID, 0.5f));
				base.AddScalarCostReduction(0.75f, PFCostModifier.Heuristic_Bonus);
			}
		}

		// Token: 0x0400022A RID: 554
		public const string RitualId = "corrupt_tribute";

		// Token: 0x0400022B RID: 555
		private readonly int _targetPlayerID;
	}
}
