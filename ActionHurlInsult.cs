using System;

namespace LoG
{
	// Token: 0x0200010A RID: 266
	public class ActionHurlInsult : ActionOrderGOAPNode<OrderInsult>
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00013F98 File Offset: 0x00012198
		public override ActionID ID
		{
			get
			{
				return ActionID.Diplo_Insult;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00013F9B File Offset: 0x0001219B
		public override string ActionName
		{
			get
			{
				return "Hurl insult to  " + base.Context.DebugName(this.TargetArchfiendID);
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00013FB8 File Offset: 0x000121B8
		public ActionHurlInsult(int targetArchfiendID)
		{
			this.TargetArchfiendID = targetArchfiendID;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00013FC7 File Offset: 0x000121C7
		protected override OrderInsult GenerateOrder()
		{
			return new OrderInsult
			{
				TargetID = this.TargetArchfiendID
			};
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00013FDA File Offset: 0x000121DA
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Result result = base.SubmitAction(context, playerState);
			if (result.successful)
			{
				this.OwningPlanner.AIPersistentData.RecordDiplomaticAbilityUsed(DiplomaticCooldownType.Insult, context.CurrentTurn);
			}
			return result;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00014003 File Offset: 0x00012203
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			return objectiveCondition is ObjectiveCondition_InsultPlayer || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00014018 File Offset: 0x00012218
		public override void Prepare()
		{
			base.AddConstraint(new WPIsActionPlannedThisTurn(this.ID)
			{
				InvertLogic = true
			});
			PlayerState playerState = this.OwningPlanner.PlayerState;
			TurnState trueTurn = this.OwningPlanner.TrueTurn;
			PlayerState playerState2 = trueTurn.FindPlayerState(this.TargetArchfiendID, null);
			if (this.OwningPlanner.AllowOpportunisticDiplomacy(this.TargetArchfiendID))
			{
				int num;
				switch (playerState.Rank)
				{
				case Rank.Marquis:
					num = 6;
					break;
				case Rank.Duke:
					num = 7;
					break;
				case Rank.Prince:
					num = 8;
					break;
				default:
					num = 5;
					break;
				}
				int value = num;
				Cost cost = new Cost();
				cost.Set(ResourceTypes.Prestige, value);
				base.AddEffect(new WPTribute(cost));
			}
			bool flag = this.OwningPlanner.IsDogPileTarget(this.TargetArchfiendID);
			if (flag)
			{
				base.AddEffect(new WPUndermineArchfiend(this.TargetArchfiendID));
			}
			VendettaHeuristics.VendettaParameters vendettaParameters;
			if (!this.OwningPlanner.ArchfiendHeuristics.TryGetLeastRiskyVendettaType(this.TargetArchfiendID, this.OwningPlanner.PlayerId, out vendettaParameters))
			{
				base.Disable(string.Format("Unable to retrieve least risky vendetta type for {0}->{1}", this.TargetArchfiendID, this.OwningPlanner.PlayerId));
				return;
			}
			float duelRisk = this.OwningPlanner.GetDuelRisk(playerState2, playerState);
			if (flag)
			{
				base.AddScalarCostModifier(1f - duelRisk, PFCostModifier.Heuristic_Bonus);
			}
			else
			{
				float num2 = 1f - MathF.Min(duelRisk, vendettaParameters.TypeRisk);
				base.AddScalarCostModifier(num2 - 0.3f, PFCostModifier.Heuristic_Bonus);
			}
			PlayerState playerState3;
			float num3;
			bool flag2;
			if (trueTurn.TryGetNemesis(playerState, out playerState3, out num3))
			{
				int? num4 = (playerState3 != null) ? new int?(playerState3.Id) : null;
				int num = playerState2.Id;
				flag2 = (num4.GetValueOrDefault() == num & num4 != null);
			}
			else
			{
				flag2 = false;
			}
			if (!flag2 && playerState.OrderSlots.Value < playerState2.OrderSlots.Value + 1)
			{
				base.AddScalarCostIncrease(0.75f, PFCostModifier.Heuristic_Bonus);
			}
			if (!flag)
			{
				base.AddConstraint(new WPDiplomaticCooldownConstraint(DiplomaticCooldownType.Insult, 2));
			}
			if (IEnumerableExtensions.Any<Praetor>(this.OwningPlanner.TrueTurn.GetGameItemsControlledBy<Praetor>(this.TargetArchfiendID)))
			{
				base.AddPrecondition(new WPHasAnyPraetor());
			}
			else
			{
				base.AddEffect(new WPCanAttack(this.TargetArchfiendID, false));
			}
			base.Prepare();
		}

		// Token: 0x04000261 RID: 609
		public const int Cooldown = 2;

		// Token: 0x04000262 RID: 610
		public int TargetArchfiendID;
	}
}
