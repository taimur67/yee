using System;
using System.Collections.Generic;
using Core.StaticData;
using Game.StaticData;

namespace LoG
{
	// Token: 0x02000117 RID: 279
	public class ActionMarchLegionOnPoP : ActionOrderGOAPNode<OrderMarchLegion>
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00015D94 File Offset: 0x00013F94
		public override bool ReducePriorityWhenTitansNeedActions
		{
			get
			{
				return this.Legion != null && this.Legion.SubCategory != GamePieceCategory.Titan;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00015DB1 File Offset: 0x00013FB1
		public override bool DoDynamicScoring
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00015DB4 File Offset: 0x00013FB4
		public override ActionID ID
		{
			get
			{
				return ActionID.March_Legion_Attack_PoP;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00015DB8 File Offset: 0x00013FB8
		public override string ActionName
		{
			get
			{
				return string.Format("March {0} towards PoP {1}", this.Legion, this.TargetPoP);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00015DD0 File Offset: 0x00013FD0
		public ActionMarchLegionOnPoP(GamePiece legion, GamePiece target, float advantageRequired)
		{
			this.Legion = legion;
			this.TargetPoP = target;
			this.AdvantageRequired = advantageRequired;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00015DF0 File Offset: 0x00013FF0
		public override void Prepare()
		{
			TurnState playerViewOfTurnState = this.OwningPlanner.PlayerViewOfTurnState;
			int controllingPlayerId = this.TargetPoP.ControllingPlayerId;
			PlayerState playerState = playerViewOfTurnState.FindPlayerState(controllingPlayerId, null);
			if (playerState == null)
			{
				base.Disable(string.Format("Could not retrieve owner of POP {0}", this.TargetPoP));
				return;
			}
			if (this.Legion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostModifier(-0.7f, PFCostModifier.Heuristic_Bonus);
				base.AddEffect(new WPEveryTitanHasOrders());
			}
			base.AddConstraint(new WPGamePieceActive(this.TargetPoP));
			base.AddConstraint(new WPLegionCanMove(this.Legion));
			if (this.OwningPlanner.ContestedPOPs.Contains(this.TargetPoP))
			{
				base.AddPrecondition(WPCombatAdvantage.MustOneShot(this.Legion, this.TargetPoP));
			}
			else
			{
				base.AddPrecondition(WPCombatAdvantage.MeetsThreshold(this.Legion, this.TargetPoP, this.AdvantageRequired));
			}
			bool flag = this.TargetPoP.IsCurrentlyCapturable();
			Identifier strongholdId = playerState.StrongholdId;
			if (this.TargetPoP.Id == strongholdId)
			{
				if (flag)
				{
					base.AddEffect(new WPArchfiendEliminated(controllingPlayerId));
				}
				base.AddPrecondition(new WPCanEliminate(controllingPlayerId));
			}
			else if (controllingPlayerId != -1)
			{
				base.AddPrecondition(new WPCanAttack(controllingPlayerId, false));
			}
			InfluenceData influenceData = this.OwningPlanner.TerrainInfluenceMap[this.TargetPoP.Location];
			if (!influenceData.TryGetTurnsToReach(this.Legion, out this._turnsToReach, true))
			{
				base.Disable(string.Format("{0} cannot reach target {1}", this.Legion, this.TargetPoP));
				return;
			}
			if (this.OwningPlanner.TrueContext.CurrentTurn.GetDiplomaticStatus(this.OwningPlanner.PlayerId, controllingPlayerId).DiplomaticState is DiplomaticState_DraconicRazzia && this._turnsToReach > 1)
			{
				base.Disable(string.Format("Razzia will end before {0} reaches {1}", this.Legion, this.TargetPoP));
				return;
			}
			base.AddTurnsToReachCostModifier(this.Legion, this._turnsToReach);
			float num;
			if (!influenceData.TryGetSpawnProximity(this.OwningPlanner.PlayerId, out num))
			{
				base.Disable(string.Format("{0} is inaccessible from our Stronghold", this.TargetPoP));
				return;
			}
			if (controllingPlayerId == -1)
			{
				base.AddScalarCostIncrease(1f - num, PFCostModifier.Heuristic_Bonus);
			}
			if (flag)
			{
				int baseValue = this.TargetPoP.PassivePrestige.BaseValue;
				base.AddEffect(new WPPrestigeProduction(baseValue));
				int battlePrestigeReward = BattleProcessor.GetBattlePrestigeReward(this.OwningPlanner.PlayerViewOfTurnState, this.Legion, this.TargetPoP);
				int num2 = this.TargetPoP.PassivePrestige.BaseValue * 3;
				Cost cost = new Cost();
				cost.Set(ResourceTypes.Prestige, battlePrestigeReward + num2);
				base.AddEffect(new WPTribute(cost));
				base.AddEffect(new WPPoPStolenFrom(controllingPlayerId));
				base.AddEffect(new WPSpecificPopCaptured(this.TargetPoP));
			}
			base.AddEffect(new WPCombatVsGamepiece(this.TargetPoP));
			base.AddEffect(new WPCombatVsPlayer(controllingPlayerId));
			this.AddEffectsIfAttackingDarkPylon(this.TargetPoP);
			base.AddEffect(WPLegionTileSafety.SafetyFromMovement(this.OwningPlanner, this.Legion, this.TargetPoP.Location));
			if (this.Legion.SubCategory == GamePieceCategory.Titan)
			{
				base.AddScalarCostReduction(0.5f, PFCostModifier.Heuristic_Bonus);
			}
			base.Prepare();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001613A File Offset: 0x0001433A
		protected override OrderMarchLegion GenerateOrder()
		{
			return new OrderMarchLegion(this.Legion, AttackOutcomeIntent.Default, FlankIntent.Undefined, Array.Empty<HexCoord>());
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00016154 File Offset: 0x00014354
		public override Result SubmitAction(TurnContext context, PlayerState playerState)
		{
			List<HexCoord> list = IEnumerableExtensions.ToList<HexCoord>(this.OwningPlanner.FindTerrainPath(this.Legion, this.Legion.Location, this.TargetPoP.Location, ~GamePieceAvoidance.FriendlyFixture, true, false, true));
			if (list != null && list.Count == 0)
			{
				return Result.Failure;
			}
			list.RemoveAt(0);
			int numberOfHexesToClaim;
			if (!this.OwningPlanner.CanClaimPathForMovement(list, out numberOfHexesToClaim, this.Legion))
			{
				return Result.Failure;
			}
			base.Order.MovePath = list;
			Problem problem = base.SubmitAction(context, playerState) as Problem;
			if (problem != null)
			{
				return problem;
			}
			if (this._turnsToReach <= 1)
			{
				this.OwningPlanner.AIPersistentData.RegisterExpectedBattle(context, this.TargetPoP.Location, this.Legion);
			}
			this.OwningPlanner.AIPersistentData.ClaimPathForMovement(list, numberOfHexesToClaim);
			this.OwningPlanner.RecordPotentialCaptures(base.Order);
			return Result.Success;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001623E File Offset: 0x0001443E
		public override void OnActionFailed()
		{
			base.OnActionFailed();
			ActionMovement.RegisterBlockages(this.OwningPlanner, this.Legion, this.TargetPoP.Location, true, false, true);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00016268 File Offset: 0x00014468
		public override bool ContributesToScheme(ObjectiveCondition objectiveCondition)
		{
			ObjectiveCondition_CapturePlacesOfPower objectiveCondition_CapturePlacesOfPower = objectiveCondition as ObjectiveCondition_CapturePlacesOfPower;
			if (objectiveCondition_CapturePlacesOfPower != null)
			{
				ConfigRef<GamePieceStaticData> targetItem = objectiveCondition_CapturePlacesOfPower.TargetItem;
				if (((targetItem != null) ? targetItem.Id : null) == "")
				{
					return true;
				}
				if (objectiveCondition_CapturePlacesOfPower.TargetItem.Id == this.TargetPoP.ToString())
				{
					return true;
				}
				GamePiece gamePiece = this.OwningPlanner.TrueTurn.FetchGameItem<GamePiece>(this.TargetPoP);
				if (gamePiece != null)
				{
					if (objectiveCondition_CapturePlacesOfPower.IsTargeted)
					{
						int controllingPlayerId = gamePiece.ControllingPlayerId;
						int? targetingPlayer = objectiveCondition_CapturePlacesOfPower.TargetingPlayer;
						if (controllingPlayerId == targetingPlayer.GetValueOrDefault() & targetingPlayer != null)
						{
							return true;
						}
					}
					if (objectiveCondition_CapturePlacesOfPower.MustBePlayerControlled && gamePiece.ControllingPlayerId != -1)
					{
						return true;
					}
				}
			}
			return objectiveCondition is ObjectiveCondition_LevelUpLegion || base.ContributesToScheme(objectiveCondition);
		}

		// Token: 0x04000286 RID: 646
		public float AdvantageRequired;

		// Token: 0x04000287 RID: 647
		private readonly GamePiece Legion;

		// Token: 0x04000288 RID: 648
		private readonly GamePiece TargetPoP;

		// Token: 0x04000289 RID: 649
		private int _turnsToReach;
	}
}
