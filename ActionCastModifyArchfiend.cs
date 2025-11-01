using System;

namespace LoG
{
	// Token: 0x020000F8 RID: 248
	public class ActionCastModifyArchfiend : ActionCastRitual<ModifyArchfiendRitualOrder>
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00011A64 File Offset: 0x0000FC64
		public bool IsDebuff
		{
			get
			{
				bool result;
				switch (this._modificationType)
				{
				case ActionCastModifyArchfiend.ModificationType.BlockOrders:
					result = true;
					break;
				case ActionCastModifyArchfiend.ModificationType.BlockEvents:
					result = true;
					break;
				case ActionCastModifyArchfiend.ModificationType.IncreaseTributeQuality:
					result = false;
					break;
				default:
					result = false;
					break;
				}
				return result;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00011AA0 File Offset: 0x0000FCA0
		public override ActionID ID
		{
			get
			{
				ActionID result;
				switch (this._modificationType)
				{
				case ActionCastModifyArchfiend.ModificationType.BlockOrders:
					result = ActionID.Cast_BlockOrders;
					break;
				case ActionCastModifyArchfiend.ModificationType.BlockEvents:
					result = ActionID.Cast_BlockEvents;
					break;
				case ActionCastModifyArchfiend.ModificationType.IncreaseTributeQuality:
					result = ActionID.Cast_Increase_Tribute_Quality;
					break;
				default:
					result = ActionID.Undefined;
					break;
				}
				return result;
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00011ADC File Offset: 0x0000FCDC
		protected override string GetRitualId()
		{
			string result;
			switch (this._modificationType)
			{
			case ActionCastModifyArchfiend.ModificationType.BlockOrders:
				result = "planar_lock";
				break;
			case ActionCastModifyArchfiend.ModificationType.BlockEvents:
				result = "malediction_of_the_seer";
				break;
			case ActionCastModifyArchfiend.ModificationType.IncreaseTributeQuality:
				result = "demand_of_supplication";
				break;
			default:
				result = "";
				break;
			}
			return result;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00011B28 File Offset: 0x0000FD28
		protected override PowerType GetPowerType()
		{
			PowerType result;
			switch (this._modificationType)
			{
			case ActionCastModifyArchfiend.ModificationType.BlockOrders:
				result = PowerType.Destruction;
				break;
			case ActionCastModifyArchfiend.ModificationType.BlockEvents:
				result = PowerType.Prophecy;
				break;
			case ActionCastModifyArchfiend.ModificationType.IncreaseTributeQuality:
				result = PowerType.Charisma;
				break;
			default:
				result = PowerType.None;
				break;
			}
			return result;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00011B61 File Offset: 0x0000FD61
		protected override int GetTargetPlayerId()
		{
			return this._targetPlayerID;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00011B69 File Offset: 0x0000FD69
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00011B6C File Offset: 0x0000FD6C
		protected override int CooldownDuration
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00011B6F File Offset: 0x0000FD6F
		public ActionCastModifyArchfiend(int targetPlayerID, ActionCastModifyArchfiend.ModificationType modificationType)
		{
			this._targetPlayerID = targetPlayerID;
			this._modificationType = modificationType;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00011B88 File Offset: 0x0000FD88
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
			switch (this._modificationType)
			{
			case ActionCastModifyArchfiend.ModificationType.BlockOrders:
				base.AddScalarCostReduction((playerState.OrderSlots > 2) ? 1f : 0.9f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, this._targetPlayerID, 0.5f));
				break;
			case ActionCastModifyArchfiend.ModificationType.BlockEvents:
				if (playerState.BlockEventCardUse)
				{
					base.AddScalarCostIncrease(1f, PFCostModifier.Heuristic_Bonus);
				}
				break;
			case ActionCastModifyArchfiend.ModificationType.IncreaseTributeQuality:
				if (!playerState.IsDrawTributeAvailable)
				{
					base.Disable("Requesting tribute is currently impossible");
					return;
				}
				base.AddEffect(new WPTributeBoost());
				base.AddScalarCostReduction(0.75f, PFCostModifier.Heuristic_Bonus);
				break;
			}
			if (this.IsDebuff)
			{
				base.AddEffect(new WPUndermineArchfiend(this._targetPlayerID));
			}
		}

		// Token: 0x04000232 RID: 562
		public const string RitualIdBlockOrders = "planar_lock";

		// Token: 0x04000233 RID: 563
		public const string RitualIdBlockEvents = "malediction_of_the_seer";

		// Token: 0x04000234 RID: 564
		public const string RitualIncreaseTributeQuality = "demand_of_supplication";

		// Token: 0x04000235 RID: 565
		private readonly ActionCastModifyArchfiend.ModificationType _modificationType;

		// Token: 0x04000236 RID: 566
		private readonly int _targetPlayerID;

		// Token: 0x020007CD RID: 1997
		public enum ModificationType
		{
			// Token: 0x040010FF RID: 4351
			BlockOrders = 2,
			// Token: 0x04001100 RID: 4352
			BlockEvents,
			// Token: 0x04001101 RID: 4353
			IncreaseTributeQuality
		}
	}
}
