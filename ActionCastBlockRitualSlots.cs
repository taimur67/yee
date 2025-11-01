using System;

namespace LoG
{
	// Token: 0x020000ED RID: 237
	public class ActionCastBlockRitualSlots : ActionCastRitual<BlockRitualSlotsRitualOrder>
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00010D3B File Offset: 0x0000EF3B
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_BlockRituals;
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00010D3F File Offset: 0x0000EF3F
		protected override string GetRitualId()
		{
			return "demonic_interference";
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00010D46 File Offset: 0x0000EF46
		protected override PowerType GetPowerType()
		{
			return PowerType.Prophecy;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00010D49 File Offset: 0x0000EF49
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayerID;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00010D51 File Offset: 0x0000EF51
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00010D54 File Offset: 0x0000EF54
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00010D57 File Offset: 0x0000EF57
		public ActionCastBlockRitualSlots(int targetPlayerID)
		{
			this._targetPlayerID = targetPlayerID;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00010D68 File Offset: 0x0000EF68
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			PlayerState playerState = this.OwningPlanner.TrueTurn.FindPlayerState(this._targetPlayerID, null);
			if (playerState == null)
			{
				base.Disable(string.Format("Invalid target player {0}", this._targetPlayerID));
				return;
			}
			base.AddScalarCostReduction(IEnumerableExtensions.Any<Identifier>(playerState.RitualState.SlottedItems) ? 1f : 0.75f, PFCostModifier.Heuristic_Bonus);
			base.AddEffect(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, this._targetPlayerID, 0.5f));
			base.AddEffect(new WPUndermineArchfiend(this._targetPlayerID));
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00010E14 File Offset: 0x0000F014
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			ObjectiveCondition_ClearRitualTable objectiveCondition_ClearRitualTable = objectiveCondition as ObjectiveCondition_ClearRitualTable;
			return (objectiveCondition_ClearRitualTable != null && this.OwningPlanner.TrueTurn.FindPlayerState(this._targetPlayerID, null).RitualState.SlottedItems.Count >= objectiveCondition_ClearRitualTable.MinimumSlotsCleared) || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x0400021C RID: 540
		public const string RitualId = "demonic_interference";

		// Token: 0x0400021D RID: 541
		private readonly int _targetPlayerID;
	}
}
