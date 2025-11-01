using System;
using System.Collections.Generic;
using System.Linq;

namespace LoG
{
	// Token: 0x020000F2 RID: 242
	public class ActionCastDestroyManuscripts : ActionCastRitual<DestroyTributeRitualOrder>
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00011594 File Offset: 0x0000F794
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Destroy_Manuscripts;
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00011598 File Offset: 0x0000F798
		protected override string GetRitualId()
		{
			return "blight_wisdom";
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001159F File Offset: 0x0000F79F
		protected override PowerType GetPowerType()
		{
			return PowerType.Destruction;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000115A2 File Offset: 0x0000F7A2
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayerID;
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x000115AA File Offset: 0x0000F7AA
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x000115AD File Offset: 0x0000F7AD
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000115B0 File Offset: 0x0000F7B0
		public ActionCastDestroyManuscripts(int targetPlayerID)
		{
			this._targetPlayerID = targetPlayerID;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000115C0 File Offset: 0x0000F7C0
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
			if (new List<GameItem>(this.OwningPlanner.TrueTurn.GetGameItemsControlledBy(this._targetPlayerID).OfType<Manuscript>()).Count == 0)
			{
				base.Disable("target has no manuscripts");
				return;
			}
			base.AddEffect(new WPUndermineArchfiend(this._targetPlayerID));
		}

		// Token: 0x04000228 RID: 552
		public const string RitualId = "blight_wisdom";

		// Token: 0x04000229 RID: 553
		private readonly int _targetPlayerID;
	}
}
