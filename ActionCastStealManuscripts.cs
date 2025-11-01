using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020000FB RID: 251
	public class ActionCastStealManuscripts : ActionCastRitual<StealTributeRitualOrder>
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00012734 File Offset: 0x00010934
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Steal_Manuscripts;
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00012738 File Offset: 0x00010938
		protected override string GetRitualId()
		{
			return "raid_the_library";
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001273F File Offset: 0x0001093F
		protected override PowerType GetPowerType()
		{
			return PowerType.Deceit;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00012742 File Offset: 0x00010942
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayerID;
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0001274A File Offset: 0x0001094A
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001274D File Offset: 0x0001094D
		protected override int CooldownDuration
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00012750 File Offset: 0x00010950
		public ActionCastStealManuscripts(int targetPlayerID)
		{
			this._targetPlayerID = targetPlayerID;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00012760 File Offset: 0x00010960
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
			if (new List<GameItem>(this.OwningPlanner.TrueTurn.GetGameItemsControlledBy(this._targetPlayerID).OfType<Manuscript>()).Count == 0)
			{
				base.Disable("target has no manuscripts");
				return;
			}
			base.AddEffect(new WPUndermineArchfiend(this._targetPlayerID));
			base.AddEffect(WPCompletedManuscript.CompletesAnyManuscript());
		}

		// Token: 0x0400023D RID: 573
		public const string RitualId = "raid_the_library";

		// Token: 0x0400023E RID: 574
		private readonly int _targetPlayerID;
	}
}
