using System;
using System.Linq;

namespace LoG
{
	// Token: 0x020000FC RID: 252
	public class ActionCastStealTribute : ActionCastRitual<StealTributeRitualOrder>
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00012818 File Offset: 0x00010A18
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Steal_Tribute;
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001281C File Offset: 0x00010A1C
		protected override string GetRitualId()
		{
			return "loot_the_vaults";
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00012823 File Offset: 0x00010A23
		protected override PowerType GetPowerType()
		{
			return PowerType.Deceit;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00012826 File Offset: 0x00010A26
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayerID;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0001282E File Offset: 0x00010A2E
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00012831 File Offset: 0x00010A31
		protected override int CooldownDuration
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00012834 File Offset: 0x00010A34
		public ActionCastStealTribute(int targetPlayerID)
		{
			this._targetPlayerID = targetPlayerID;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00012844 File Offset: 0x00010A44
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindPlayerState(this._targetPlayerID, null);
			if (playerState == null || playerState.Id == -2147483648 || playerState.Id == -1)
			{
				base.Disable(string.Format("Invalid target player {0}", this._targetPlayerID));
				return;
			}
			PlayerState playerState2 = this.OwningPlanner.PlayerState;
			int value = playerState2.PowersLevels[PowerType.Deceit].CurrentLevel.Value;
			int num = Math.Min(playerState.Resources.Count, value / 2 + 1);
			if (num == 0)
			{
				base.Disable(string.Format("Target player {0} has no tokens to steal", this._targetPlayerID));
				return;
			}
			int value2 = playerState.PowersLevels[PowerType.Deceit].CurrentLevel.Value;
			if (value >= value2 + 2 || value >= 4)
			{
				Cost cost = new Cost();
				int num2 = (int)playerState.Resources.Average((ResourceNFT r) => r.ValueSum);
				int value3 = (int)MathF.Ceiling((float)(num * num2) / 4f);
				cost.Set(ResourceTypes.Souls, value3);
				cost.Set(ResourceTypes.Ichor, value3);
				cost.Set(ResourceTypes.Hellfire, value3);
				cost.Set(ResourceTypes.Darkness, value3);
				base.AddEffect(new WPTribute(cost));
			}
			base.AddEffect(new WPUndermineArchfiend(this._targetPlayerID));
			if (WPHasTitan.Check(this.OwningPlanner.TrueTurn, playerState))
			{
				base.AddEffect(new WPMilitarySuperiority(playerState2.Id, this._targetPlayerID, 0.5f));
				return;
			}
			base.AddScalarCostIncrease(0.4f, PFCostModifier.Heuristic_Bonus);
		}

		// Token: 0x0400023F RID: 575
		public const string RitualId = "loot_the_vaults";

		// Token: 0x04000240 RID: 576
		private readonly int _targetPlayerID;
	}
}
