using System;
using System.Collections.Generic;
using System.Linq;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020000FD RID: 253
	public class ActionDanseMacabre : ActionCastRitual<SummonLegionRitualOrder>
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x000129F8 File Offset: 0x00010BF8
		public override ActionID ID
		{
			get
			{
				return ActionID.Cast_Danse_Macabre_Dark_Art;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000129FC File Offset: 0x00010BFC
		protected override string GetRitualId()
		{
			return "murmur_danse_macabre";
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00012A03 File Offset: 0x00010C03
		public static bool CanBeUsedByArchfiend(PlayerState wouldBeCaster)
		{
			return wouldBeCaster.ArchfiendId == "Murmur";
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x00012A15 File Offset: 0x00010C15
		public override ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.High_AlwaysFirst;
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00012A18 File Offset: 0x00010C18
		protected override PowerType GetPowerType()
		{
			return PowerType.Destruction;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00012A1B File Offset: 0x00010C1B
		protected override HexCoord GetTargetHexCoord()
		{
			return this._targetHexCoord;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x00012A23 File Offset: 0x00010C23
		protected override int CooldownDuration
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00012A26 File Offset: 0x00010C26
		public ActionDanseMacabre(HexCoord targetHexCoord)
		{
			this._targetHexCoord = targetHexCoord;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00012A38 File Offset: 0x00010C38
		public override void Prepare()
		{
			base.Prepare();
			if (this.IsDisabled())
			{
				return;
			}
			HexCoord hexCoord = this.DetermineSupportPosition();
			if (hexCoord == HexCoord.Invalid)
			{
				base.Disable(string.Format("Macabre Host no valid support position around {0}", this._targetHexCoord));
				return;
			}
			base.AddEffect(new WPOpportunisticSupport());
			base.AddScalarCostModifier(-1f, PFCostModifier.Archfiend_Bonus);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00012A9C File Offset: 0x00010C9C
		private HexCoord DetermineSupportPosition()
		{
			TurnContext context = this.OwningPlanner.AIPreviewContext;
			PlayerState playerState = this.OwningPlanner.AIPreviewPlayerState;
			List<HexCoord> source = IEnumerableExtensions.ToList<HexCoord>(context.HexBoard.GetNeighbours(this._targetHexCoord, false));
			GamePiece myStronghold = context.CurrentTurn.GetStronghold(playerState.Id);
			source.SortOnValueDescending((HexCoord t) => this.OwningPlanner.PlayerViewOfTurnState.HexBoard.ShortestDistance(myStronghold.Location, t));
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(from t in source
			where context.HexBoard[t].ControllingPlayerID == playerState.Id
			where !this.OwningPlanner.AIPersistentData.IsHexClaimedForMovement(t)
			select t);
			if (IEnumerableExtensions.Any<HexCoord>(list))
			{
				HexCoord hexCoord = this.OwningPlanner.PlayerViewOfTurnState.FirstVacantTraversableHexCoord(list);
				if (hexCoord != HexCoord.Invalid)
				{
					return hexCoord;
				}
			}
			return HexCoord.Invalid;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00012B80 File Offset: 0x00010D80
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			HexCoord targetHex = this.DetermineSupportPosition();
			if (targetHex == HexCoord.Invalid)
			{
				return Result.Failure;
			}
			TargetContext targetContext = new TargetContext();
			targetContext.SetTargetHex(targetHex);
			base.Order.TargetContext = targetContext;
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			this.OwningPlanner.AIPersistentData.SetBattleSupportedAt(this._targetHexCoord, true);
			this.OwningPlanner.AIPersistentData.RegisterHexClaimedForMovement(this._targetHexCoord);
			return Result.Success;
		}

		// Token: 0x04000241 RID: 577
		public const string RitualId = "murmur_danse_macabre";

		// Token: 0x04000242 RID: 578
		private const string ArchfiendId = "Murmur";

		// Token: 0x04000243 RID: 579
		private readonly HexCoord _targetHexCoord;
	}
}
