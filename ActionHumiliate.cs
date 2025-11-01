using System;

namespace LoG
{
	// Token: 0x02000109 RID: 265
	public class ActionHumiliate : ActionOrderGOAPNode<OrderHumiliate>
	{
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00013C4E File Offset: 0x00011E4E
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Humiliate;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00013C52 File Offset: 0x00011E52
		public override string ActionName
		{
			get
			{
				return "Humiliate " + base.Context.DebugName(this.ArchfiendID) + ", threat " + this.GetGrievanceType();
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00013C7C File Offset: 0x00011E7C
		private string GetGrievanceType()
		{
			if (this._grievance != null)
			{
				VendettaContext vendettaContext = this._grievance as VendettaContext;
				if (vendettaContext != null)
				{
					return vendettaContext.Objective.Id;
				}
				if (this._grievance is PraetorBattleContext)
				{
					return "duel";
				}
			}
			return "";
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00013CC4 File Offset: 0x00011EC4
		public ActionHumiliate(int archfiendID)
		{
			this.ArchfiendID = archfiendID;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00013CD4 File Offset: 0x00011ED4
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			base.AddConstraint(new WPNeutralTitanOnWarpath
			{
				InvertLogic = true
			});
			base.AddPrecondition(new WPInfernalRank(Rank.Prince));
			PlayerState playerState = this.OwningPlanner.AIPreviewContext.CurrentTurn.FindPlayerState(this.ArchfiendID, null);
			if (playerState == null)
			{
				base.Disable(string.Format("Invalid target player {0}", this.ArchfiendID));
				return;
			}
			if (!this.OwningPlanner.TryGenerateGrievance(playerState, out this._grievance) || this._grievance == null)
			{
				base.Disable("Humiliate couldn't generate a good grievance.");
				return;
			}
			if (this.OwningPlanner.AllowOpportunisticDiplomacy(playerState.Id))
			{
				ModifiableValue prestigeReward = this._grievance.PrestigeReward;
				if (prestigeReward != null && prestigeReward.Value > 0)
				{
					base.AddEffect(WPTribute.PrestigeOnly(this._grievance.PrestigeReward));
				}
			}
			bool flag = this.OwningPlanner.IsDogPileTarget(playerState.Id);
			if (flag)
			{
				base.AddEffect(new WPUndermineArchfiend(playerState.Id));
			}
			if (!flag)
			{
				base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.Humiliate, 1));
			}
			if (this._grievance is VendettaContext)
			{
				if (!flag)
				{
					base.AddPrecondition(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, this.ArchfiendID, 0.5f));
				}
				base.AddPrecondition(new WPThreaten(this.OwningPlanner.PlayerId, this.ArchfiendID));
				base.AddEffect(new WPCanAttack(this.ArchfiendID, false));
				base.AddEffect(new WPCanEliminate(this.ArchfiendID));
				base.AddEffect(new WPPrestigeProduction(WorldProperty.MaxWeight));
			}
			else if (this._grievance is PraetorBattleContext)
			{
				base.AddPrecondition(new WPHasAnyPraetor());
				base.AddEffect(new WPCanDuel(this.ArchfiendID));
			}
			float num = -1f;
			TurnState trueTurn = this.OwningPlanner.TrueTurn;
			PlayerState playerState2 = this.OwningPlanner.PlayerState;
			PlayerState playerState3;
			float num2;
			bool flag2;
			if (trueTurn.TryGetNemesis(playerState2, out playerState3, out num2))
			{
				int? num3 = (playerState3 != null) ? new int?(playerState3.Id) : null;
				int id = playerState.Id;
				flag2 = (num3.GetValueOrDefault() == id & num3 != null);
			}
			else
			{
				flag2 = false;
			}
			if (!flag2 && this.OwningPlanner.PlayerState.OrderSlots.Value < playerState.OrderSlots.Value + 1)
			{
				num += 1.5f;
			}
			num = Math.Clamp(num, 0f, 1f);
			base.AddScalarCostModifier(num, PFCostModifier.Heuristic_Bonus);
			base.Prepare();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00013F58 File Offset: 0x00012158
		protected override OrderHumiliate GenerateOrder()
		{
			return new OrderHumiliate
			{
				TargetID = this.ArchfiendID,
				GrievanceResponse = this._grievance
			};
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00013F77 File Offset: 0x00012177
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.Humiliate, context.CurrentTurn);
			return base.SubmitAction(context, playerState);
		}

		// Token: 0x0400025E RID: 606
		public const int Cooldown = 1;

		// Token: 0x0400025F RID: 607
		public int ArchfiendID;

		// Token: 0x04000260 RID: 608
		private GrievanceContext _grievance;
	}
}
