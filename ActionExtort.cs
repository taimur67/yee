using System;

namespace LoG
{
	// Token: 0x02000108 RID: 264
	public class ActionExtort : ActionOrderGOAPNode<OrderExtort>
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0001399F File Offset: 0x00011B9F
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Extort;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x000139A2 File Offset: 0x00011BA2
		public override string ActionName
		{
			get
			{
				return string.Format("Extort {0} for: {1} bk: {2}", base.Context.DebugName(this.ArchfiendID), this.ExtortType, this.BackingType);
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x000139D5 File Offset: 0x00011BD5
		public ActionExtort(int archfiendID, ExtortType extortType, DiplomaticBackingType backingType)
		{
			this.ArchfiendID = archfiendID;
			this.ExtortType = extortType;
			this.BackingType = backingType;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000139FC File Offset: 0x00011BFC
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.Extort, 1));
			base.AddConstraint(new WPNeutralTitanOnWarpath
			{
				InvertLogic = true
			});
			base.AddPrecondition(new WPInfernalRank(Rank.Duke));
			TurnState trueTurn = this.OwningPlanner.TrueTurn;
			PlayerState playerState = this.OwningPlanner.PlayerState;
			PlayerState playerState2 = trueTurn.FindPlayerState(this.ArchfiendID, null);
			PlayerState playerState3;
			float num;
			bool flag;
			if (trueTurn.TryGetNemesis(playerState, out playerState3, out num))
			{
				int? num2 = (playerState3 != null) ? new int?(playerState3.Id) : null;
				int id = playerState2.Id;
				flag = (num2.GetValueOrDefault() == id & num2 != null);
			}
			else
			{
				flag = false;
			}
			if (this.BackingType == DiplomaticBackingType.LegionThreateningPositions)
			{
				if (!this.OwningPlanner.IsDogPileTarget(this.ArchfiendID))
				{
					base.AddPrecondition(new WPMilitarySuperiority(playerState.Id, this.ArchfiendID, 0.5f));
				}
				base.AddPrecondition(new WPThreaten(playerState.Id, this.ArchfiendID));
				base.AddEffect(new WPCanAttack(this.ArchfiendID, false));
			}
			else if (this.BackingType == DiplomaticBackingType.Praetor)
			{
				base.AddPrecondition(new WPHasAnyPraetor());
				base.AddEffect(new WPCanDuel(this.ArchfiendID));
			}
			if (this.OwningPlanner.AllowOpportunisticDiplomacy(this.ArchfiendID))
			{
				base.AddEffect(WPTribute.PrestigeOnly(this.PrestigeReward));
				if (this.ExtortType == ExtortType.Praetor)
				{
					base.AddEffect(new WPHasAnyPraetor());
					base.AddEffect(new WPDuelAdvantage(this.ArchfiendID));
				}
				else if (this.ExtortType == ExtortType.Artifact)
				{
					base.AddEffect(new WPHasAnyArtifact());
				}
			}
			float num3 = -0.9f;
			if (!flag && playerState.OrderSlots.Value < playerState2.OrderSlots.Value + 1)
			{
				num3 += 1.5f;
			}
			num3 = Math.Clamp(num3, 0f, 1f);
			base.AddScalarCostModifier(num3, PFCostModifier.Heuristic_Bonus);
			base.Prepare();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00013BEC File Offset: 0x00011DEC
		protected override OrderExtort GenerateOrder()
		{
			OrderExtort orderExtort = new OrderExtort();
			orderExtort.TargetID = this.ArchfiendID;
			if (this.ExtortType == ExtortType.Artifact)
			{
				orderExtort.DemandOption = DemandOptions.SelectedArtifact;
			}
			else if (this.ExtortType == ExtortType.Praetor)
			{
				orderExtort.DemandOption = DemandOptions.SelectedPraetor;
			}
			return orderExtort;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00013C2D File Offset: 0x00011E2D
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.Extort, context.CurrentTurn);
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x04000259 RID: 601
		public const int Cooldown = 1;

		// Token: 0x0400025A RID: 602
		public int ArchfiendID;

		// Token: 0x0400025B RID: 603
		public ExtortType ExtortType;

		// Token: 0x0400025C RID: 604
		public int PrestigeReward = 10;

		// Token: 0x0400025D RID: 605
		public DiplomaticBackingType BackingType;
	}
}
