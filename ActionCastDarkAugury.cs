using System;

namespace LoG
{
	// Token: 0x020000F0 RID: 240
	public class ActionCastDarkAugury : ActionCastRitual<DarkAuguryRitualOrder>
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003BA RID: 954 RVA: 0x0001122D File Offset: 0x0000F42D
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Reveal_Vault;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00011231 File Offset: 0x0000F431
		protected override string GetRitualId()
		{
			return "dark_augury";
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00011238 File Offset: 0x0000F438
		protected override PowerType GetPowerType()
		{
			return PowerType.Prophecy;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001123B File Offset: 0x0000F43B
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayerID;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00011243 File Offset: 0x0000F443
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Low_AlwaysLast;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00011246 File Offset: 0x0000F446
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00011249 File Offset: 0x0000F449
		public ActionCastDarkAugury(int targetPlayerID)
		{
			this._targetPlayerID = targetPlayerID;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00011258 File Offset: 0x0000F458
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			base.AddEffect(new WPVaultRevealed(this._targetPlayerID));
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001127A File Offset: 0x0000F47A
		private bool CanRevealVault()
		{
			return this.OwningPlanner.PlayerState.PowersLevels[PowerType.Prophecy] > 1;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001129A File Offset: 0x0000F49A
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			return (objectiveCondition is ObjectiveCondition_RevealVaults && this.CanRevealVault()) || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x04000224 RID: 548
		public const string RitualId = "dark_augury";

		// Token: 0x04000225 RID: 549
		private readonly int _targetPlayerID;
	}
}
