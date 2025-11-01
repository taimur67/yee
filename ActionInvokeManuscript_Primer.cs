using System;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x020000E2 RID: 226
	public class ActionInvokeManuscript_Primer : ActionOrderGOAPNode<InvokeManuscriptOrder>
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000F358 File Offset: 0x0000D558
		public override ActionID ID
		{
			get
			{
				return ActionID.Invoke_Manuscript_Primer;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000F35C File Offset: 0x0000D55C
		public override string ActionName
		{
			get
			{
				return string.Format("Invoke primer: {0} on legion {1}", this.StaticData.Id, this.TargetLegion);
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000F379 File Offset: 0x0000D579
		public ActionInvokeManuscript_Primer(ManuscriptStaticData staticData, GamePiece targetLegion)
		{
			this.StaticData = staticData;
			this.TargetLegion = targetLegion;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000F398 File Offset: 0x0000D598
		private void PrepareMovementPrimer(TerrainType terrainType)
		{
			if (LegionMovementProcessor.HasMovementCostReduction(this.TargetLegion, terrainType))
			{
				base.Disable(string.Format("{0} can already cross {1} without penalty", this.TargetLegion, terrainType));
				return;
			}
			base.AddEffect(new WPMapHazardWalk(this.TargetLegion, TerrainType.Swamp));
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000F3E8 File Offset: 0x0000D5E8
		private void PrepareCombatPrimer(int bonusRanged, int bonusMelee, int bonusInfernal)
		{
			CombatStats bonus = new CombatStats
			{
				Ranged = bonusRanged,
				Melee = bonusMelee,
				Infernal = bonusInfernal
			};
			base.AddEffect(WPCombatAdvantage.BonusFor(this.TargetLegion, bonus));
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000F438 File Offset: 0x0000D638
		public override void Prepare()
		{
			float num = 0f;
			float num2 = 0.5f;
			base.AddConstraint(new WPActionCooldown(ActionID.Invoke_Manuscript_Primer, this.Cooldown));
			base.AddPrecondition(new WPCompletedManuscript(this.StaticData, ""));
			base.AddPrecondition(new WPLegionBadlyDamaged(this.TargetLegion, 0.7f)
			{
				InvertLogic = true
			});
			if (this.TargetLegion.LearntManuscriptsCount >= this.TargetLegion.Level)
			{
				base.Disable(string.Format("{0} has already studied {1} of a possible {2} Primers", this.TargetLegion, this.TargetLegion.LearntManuscriptsCount, this.TargetLegion.Level));
				return;
			}
			string id = this.StaticData.Id;
			if (!(id == "Manuscript_SwampTraining"))
			{
				if (!(id == "Manuscript_RavineTraining"))
				{
					if (!(id == "Manuscript_LavaTraining"))
					{
						if (!(id == "Manuscript_VentsTraining"))
						{
							if (!(id == "Manuscript_PortalTraining"))
							{
								foreach (PlayerState playerState in this.OwningPlanner.TrueTurn.EnumeratePlayerStates(false, false))
								{
									if (playerState.Id != this.OwningPlanner.PlayerId)
									{
										base.AddEffect(new WPMilitarySuperiority(this.OwningPlanner.PlayerId, playerState.Id, 0.5f));
									}
								}
								this.PrepareCombatPrimer(2, 2, 2);
							}
							else
							{
								if (this.TargetLegion.CanTeleport)
								{
									base.Disable("Target already knows how to teleport");
									return;
								}
								this.PrepareCombatPrimer(1, 1, 1);
								ref num2.LerpTo01(0.4f);
							}
						}
						else
						{
							if (this.StaticData.Id == "Manuscript_VentsTraining" && this.TargetLegion.TotalRanged() <= 3)
							{
								base.Disable("Not worth giving vent training to low-ranged unit");
								return;
							}
							this.PrepareCombatPrimer(1, 0, 0);
							ref num2.LerpTo01(0.5f);
						}
					}
					else
					{
						this.PrepareMovementPrimer(TerrainType.Lava);
						ref num2.LerpTo01(0.3f);
					}
				}
				else
				{
					this.PrepareMovementPrimer(TerrainType.Ravine);
					ref num2.LerpTo01(0.3f);
				}
			}
			else
			{
				if (this.TargetLegion.TotalMelee() <= 3 || this.TargetLegion.GroundMoveDistance < 2)
				{
					base.Disable("Not worth giving swamp walk to slow / low-melee legion");
					return;
				}
				this.PrepareMovementPrimer(TerrainType.Swamp);
				ref num2.LerpTo01(0.3f);
			}
			if (this.TargetLegion.Level > 2)
			{
				ref num.LerpTo01((float)Math.Min(5, this.TargetLegion.Level) * 0.1f);
			}
			if (this.TargetLegion.IsPersonalGuard(this.OwningPlanner.PlayerViewOfTurnState))
			{
				ref num.LerpTo01(0.5f);
			}
			base.AddScalarCostModifier(num2 - num, PFCostModifier.Heuristic_Bonus);
			base.Prepare();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000F728 File Offset: 0x0000D928
		protected override InvokeManuscriptOrder GenerateOrder()
		{
			ManuscriptGamePieceOrder manuscriptGamePieceOrder = new ManuscriptGamePieceOrder();
			manuscriptGamePieceOrder.AbilityId = this.StaticData.ProvidedAbility.Id;
			TargetContext targetContext = new TargetContext(this.TargetLegion);
			targetContext.SetTargetPlayer(this.OwningPlanner.PlayerId);
			manuscriptGamePieceOrder.TargetContext = targetContext;
			manuscriptGamePieceOrder.AbilityId = this.StaticData.ProvidedAbility.Id;
			return manuscriptGamePieceOrder;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000F790 File Offset: 0x0000D990
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			Manuscript manuscript = context.CurrentTurn.GetGameItemsControlledBy<Manuscript>(playerState.Id).First((Manuscript t) => t.StaticDataId == this.StaticData.Id);
			if (manuscript != null && manuscript.Id != Identifier.Invalid)
			{
				base.Order.ManuscriptId = manuscript.Id;
				Result result = base.SubmitAction(context, playerState);
				if (result.successful)
				{
					this.OwningPlanner.AIPersistentData.RecordActionUsed(ActionID.Invoke_Manuscript_Primer, this.OwningPlanner.TrueTurn);
				}
				return result;
			}
			return Result.Failure;
		}

		// Token: 0x04000207 RID: 519
		public GamePiece TargetLegion;

		// Token: 0x04000208 RID: 520
		public ManuscriptStaticData StaticData;

		// Token: 0x04000209 RID: 521
		public int Cooldown = 1;
	}
}
