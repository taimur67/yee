using System;
using System.Linq;

namespace LoG
{
	// Token: 0x0200010D RID: 269
	public class ActionMakeDemand : ActionOrderGOAPNode<OrderMakeDemand>
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x000143A4 File Offset: 0x000125A4
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Demand;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x000143A7 File Offset: 0x000125A7
		public override string ActionName
		{
			get
			{
				return string.Format("Make Demand from {0}, bkg: {1}", base.Context.DebugName(this.TargetArchfiendID), this.BackingType);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000143CF File Offset: 0x000125CF
		public ActionMakeDemand(int targetArchfiendID, DiplomaticBackingType backingType)
		{
			this.TargetArchfiendID = targetArchfiendID;
			this.BackingType = backingType;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000143E5 File Offset: 0x000125E5
		protected override OrderMakeDemand GenerateOrder()
		{
			return new OrderMakeDemand
			{
				TargetID = this.TargetArchfiendID,
				DemandOption = OrderMakeDemand.GetMaximumTokenDemandOption(this.OwningPlanner.PlayerState)
			};
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001440E File Offset: 0x0001260E
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Result result = base.SubmitAction(context, playerState);
			if (result.successful)
			{
				this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.Demand, context.CurrentTurn);
			}
			return result;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00014438 File Offset: 0x00012638
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			PlayerState playerState = this.OwningPlanner.PlayerState;
			TurnState trueTurn = this.OwningPlanner.TrueTurn;
			PlayerState targetArchfiend = trueTurn.FindPlayerState(this.TargetArchfiendID, null);
			base.AddConstraint(new WPNeutralTitanOnWarpath
			{
				InvertLogic = true
			});
			if (this.OwningPlanner.AllowOpportunisticDiplomacy(this.TargetArchfiendID))
			{
				Cost cost = new Cost();
				float maximumTokenCount = (float)OrderMakeDemand.GetMaximumTokenCount(playerState);
				int num = targetArchfiend.TributeQuality;
				int value = (int)MathF.Ceiling(maximumTokenCount * (float)num / 4f);
				cost.Set(ResourceTypes.Souls, value);
				cost.Set(ResourceTypes.Ichor, value);
				cost.Set(ResourceTypes.Hellfire, value);
				cost.Set(ResourceTypes.Darkness, value);
				int num2;
				switch (this.OwningPlanner.PlayerState.Rank)
				{
				case Rank.Marquis:
					num2 = 15;
					break;
				case Rank.Duke:
					num2 = 20;
					break;
				case Rank.Prince:
					num2 = 25;
					break;
				default:
					num2 = 10;
					break;
				}
				int value2 = num2;
				cost.Set(ResourceTypes.Prestige, value2);
				base.AddEffect(new WPTribute(cost));
			}
			bool flag = this.OwningPlanner.IsDogPileTarget(this.TargetArchfiendID);
			if (flag)
			{
				base.AddEffect(new WPUndermineArchfiend(targetArchfiend.Id));
			}
			PlayerState playerState2;
			float num3;
			bool flag2;
			if (trueTurn.TryGetNemesis(playerState, out playerState2, out num3))
			{
				int? num4 = (playerState2 != null) ? new int?(playerState2.Id) : null;
				int num2 = targetArchfiend.Id;
				flag2 = (num4.GetValueOrDefault() == num2 & num4 != null);
			}
			else
			{
				flag2 = false;
			}
			if (!flag2 && playerState.OrderSlots.Value < targetArchfiend.OrderSlots.Value + 1)
			{
				base.AddScalarCostIncrease(0.5f, PFCostModifier.Heuristic_Bonus);
			}
			if (!flag)
			{
				base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.Demand, 2));
			}
			if (this.BackingType == DiplomaticBackingType.LegionThreateningPositions)
			{
				VendettaHeuristics.VendettaParameters vendettaParameters;
				if (!this.OwningPlanner.ArchfiendHeuristics.TryGetLeastRiskyVendettaType(this.OwningPlanner.PlayerId, this.TargetArchfiendID, out vendettaParameters))
				{
					base.Disable(string.Format("Unable to retrieve least risky vendetta type for {0}->{1}", this.OwningPlanner.PlayerId, this.TargetArchfiendID));
					return;
				}
				base.AddPrecondition(new WPThreaten(playerState.Id, this.TargetArchfiendID));
				if (!flag)
				{
					base.AddPrecondition(new WPMilitarySuperiority(playerState.Id, this.TargetArchfiendID, 0.5f));
				}
				base.AddEffect(new WPCanAttack(this.TargetArchfiendID, false));
				if (trueTurn.EnumerateAllGamePieces().Any((GamePiece gp) => gp.ControllingPlayerId == targetArchfiend.Id && gp.SubCategory == GamePieceCategory.PoP && gp.IsCapturable()))
				{
					base.AddEffect(new WPPrestigeProduction(WorldProperty.MaxWeight));
				}
				base.AddScalarCostModifier(vendettaParameters.TypeRisk - 0.5f, PFCostModifier.Heuristic_Bonus);
			}
			else if (this.BackingType == DiplomaticBackingType.Praetor)
			{
				base.AddPrecondition(new WPHasAnyPraetor());
				base.AddEffect(new WPCanDuel(this.TargetArchfiendID));
				float duelRisk = this.OwningPlanner.GetDuelRisk(playerState, targetArchfiend);
				base.AddScalarCostModifier(duelRisk - 0.5f, PFCostModifier.Heuristic_Bonus);
			}
			base.Prepare();
		}

		// Token: 0x04000267 RID: 615
		public const int Cooldown = 2;

		// Token: 0x04000268 RID: 616
		public int TargetArchfiendID;

		// Token: 0x04000269 RID: 617
		public DiplomaticBackingType BackingType;
	}
}
