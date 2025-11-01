using System;
using System.Collections.Generic;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x0200012F RID: 303
	public abstract class GOAPNode : PFNode
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001A719 File Offset: 0x00018919
		public GameRules GameRules
		{
			get
			{
				return this.OwningPlanner.Rules;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001A726 File Offset: 0x00018926
		public GameDatabase GameDatabase
		{
			get
			{
				return this.OwningPlanner.Database;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001A733 File Offset: 0x00018933
		protected TurnContext Context
		{
			get
			{
				return this.OwningPlanner.PristineContext;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0001A740 File Offset: 0x00018940
		public IReadOnlyList<WorldProperty> Preconditions
		{
			get
			{
				return this._preconditions;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001A748 File Offset: 0x00018948
		public IReadOnlyList<WorldProperty> Effects
		{
			get
			{
				return this._effects;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001A750 File Offset: 0x00018950
		public IReadOnlyList<WorldProperty> Constraints
		{
			get
			{
				return this._constraints;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000584 RID: 1412
		public abstract ActionID ID { get; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001A758 File Offset: 0x00018958
		public virtual string ActionName
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001A765 File Offset: 0x00018965
		public virtual GOAPNodeType NodeType
		{
			get
			{
				return GOAPNodeType.Action;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001A768 File Offset: 0x00018968
		public virtual ActionOrderPriority Priority
		{
			get
			{
				return ActionOrderPriority.Normal_DontCare;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001A76B File Offset: 0x0001896B
		public virtual bool DoDynamicScoring
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001A76E File Offset: 0x0001896E
		public virtual bool ConsumesActionSlot
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001A771 File Offset: 0x00018971
		public IReadOnlyDictionary<PFNode, float> Heuristics
		{
			get
			{
				return this._heuristicDict;
			}
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001A77C File Offset: 0x0001897C
		protected GOAPNode()
		{
			base.BaseCost = GoapNodeCosts.Base;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001A7D7 File Offset: 0x000189D7
		public GOAPNode Initialize(GOAPPlanner planner)
		{
			this.OwningPlanner = planner;
			this.Prepare();
			return this;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001A7E7 File Offset: 0x000189E7
		public override bool IsDisabled()
		{
			return base.IsDisabled() || !this._constraintStatus;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001A7FC File Offset: 0x000189FC
		public void ClearAndPrepare()
		{
			this.Clear();
			this.Prepare();
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001A80A File Offset: 0x00018A0A
		public virtual void Prepare()
		{
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001A80C File Offset: 0x00018A0C
		public virtual void Clear()
		{
			this._preconditions.Clear();
			this._effects.Clear();
			this._constraints.Clear();
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001A830 File Offset: 0x00018A30
		public void AddTurnsToReachCostModifier(GamePiece movingPiece, int turnsToReach)
		{
			float num = 0f;
			if (this.OwningPlanner.AITransientData.LegionsInDangerousPositions.Contains(movingPiece))
			{
				num += 0.95f;
			}
			if (movingPiece.SubCategory == GamePieceCategory.Titan && !WPEveryTitanHasOrders.Check(this.OwningPlanner, movingPiece))
			{
				num += 0.9f;
			}
			if (this.OwningPlanner.AIPersistentData.IsABlockingPiece(movingPiece))
			{
				num += 0.925f;
			}
			if (turnsToReach > 0)
			{
				num /= (float)turnsToReach;
			}
			if (num > 0f)
			{
				this.AddScalarCostReduction(num, PFCostModifier.Terrain_Cost);
				return;
			}
			this.AddScalarCostIncrease(0.1f * MathF.Pow((float)turnsToReach - 1f, 2f), PFCostModifier.Terrain_Cost);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001A8E5 File Offset: 0x00018AE5
		public void AddScalarCostModifier(float amount, PFCostModifier modifier = PFCostModifier.Heuristic_Bonus)
		{
			amount = Math.Clamp(amount, -1f, 1f);
			if (amount > 0f)
			{
				this.AddScalarCostIncrease(amount, modifier);
				return;
			}
			if (amount < 0f)
			{
				this.AddScalarCostReduction(-amount, modifier);
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001A91B File Offset: 0x00018B1B
		public void AddScalarCostReduction(float amount, PFCostModifier modifier = PFCostModifier.Heuristic_Bonus)
		{
			amount = Math.Clamp(amount, 0f, 1f);
			base.AddRawScalarCostModifier(modifier, -amount);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001A938 File Offset: 0x00018B38
		public void AddScalarCostIncrease(float amount, PFCostModifier modifier = PFCostModifier.Heuristic_Bonus)
		{
			amount = Math.Clamp(amount, 0f, 1f);
			base.AddRawScalarCostModifier(modifier, amount * GoapNodeCosts.UnattractiveMultiplier);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001A95A File Offset: 0x00018B5A
		public void AddPrecondition(WorldProperty precondition)
		{
			if (precondition == null)
			{
				return;
			}
			this._preconditions.Add(precondition.Setup(this.OwningPlanner));
			precondition.OnAddedAsPrecondition(this, this.OwningPlanner.PlayerViewOfTurnState, this.OwningPlanner.PlayerState);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001A994 File Offset: 0x00018B94
		public bool IsDestination()
		{
			return this.AreAllPreconditionsMet();
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001A99C File Offset: 0x00018B9C
		public void AddEffect(WorldProperty effect)
		{
			if (effect == null)
			{
				return;
			}
			this._effects.Add(effect.Setup(this.OwningPlanner));
			effect.OnAddedAsEffect(this, this.OwningPlanner.PlayerViewOfTurnState, this.OwningPlanner.PlayerState);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001A9D6 File Offset: 0x00018BD6
		public void AddConstraint(WorldProperty constraint)
		{
			if (constraint == null)
			{
				return;
			}
			this._constraints.Add(constraint.Setup(this.OwningPlanner));
			constraint.OnAddedAsConstraint(this, this.OwningPlanner.PlayerViewOfTurnState, this.OwningPlanner.PlayerState);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001AA10 File Offset: 0x00018C10
		public override float Heuristic(PFNode destination)
		{
			float result;
			if (this.TryGetHeuristic(destination, out result))
			{
				return result;
			}
			GOAPNode goapnode = destination as GOAPNode;
			if (goapnode != null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Warn(this.ActionName + " has no heuristic cost to goal " + goapnode.ActionName + ". How is this possible?");
				}
			}
			else
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Warn(this.ActionName + " has no heuristic cost to goal " + destination.GetType().Name + " that is not a GOAPNode. How is this possible?");
				}
			}
			return 999f;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001AA96 File Offset: 0x00018C96
		public bool TryGetHeuristic(PFNode destination, out float heuristic)
		{
			return this._heuristicDict.TryGetValue(destination, out heuristic);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001AAA5 File Offset: 0x00018CA5
		public IEnumerable<string> DebugHeuristics()
		{
			foreach (KeyValuePair<PFNode, float> keyValuePair in this._heuristicDict)
			{
				string text = (keyValuePair.Key as GOAPNode).ID.ToString() + " , " + keyValuePair.Value.ToString();
				yield return text;
			}
			Dictionary<PFNode, float>.Enumerator enumerator = default(Dictionary<PFNode, float>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001AAB5 File Offset: 0x00018CB5
		public void AddHeuristic(PFNode other, float distance)
		{
			this._heuristicDict[other] = distance;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001AAC4 File Offset: 0x00018CC4
		public bool AddHeuristicIfBetter(PFNode other, float distance)
		{
			float maxValue;
			if (!this.TryGetHeuristic(other, out maxValue))
			{
				maxValue = float.MaxValue;
			}
			if (distance < maxValue)
			{
				this.AddHeuristic(other, distance);
				return true;
			}
			return false;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001AAF1 File Offset: 0x00018CF1
		public bool AreAllPreconditionsMet()
		{
			return this._preconditionStatus;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001AAF9 File Offset: 0x00018CF9
		public bool AreAllConstraintsMet()
		{
			return this._constraintStatus;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001AB01 File Offset: 0x00018D01
		public bool EvaluatePreconditions()
		{
			return this.AreFulfilled(this.Preconditions);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001AB0F File Offset: 0x00018D0F
		public bool EvaluateConstraints()
		{
			return this.AreFulfilled(this.Constraints);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001AB1D File Offset: 0x00018D1D
		public void EvaluateAndCachePropertyStatus()
		{
			this._constraintStatus = this.EvaluateConstraints();
			this._preconditionStatus = this.EvaluatePreconditions();
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001AB37 File Offset: 0x00018D37
		public IEnumerable<WorldProperty> GetFailedConstraints()
		{
			foreach (WorldProperty worldProperty in this._constraints)
			{
				if (!worldProperty.IsFulfilled(this.OwningPlanner.AIPreviewContext, this.OwningPlanner.AIPreviewPlayerState, this.OwningPlanner))
				{
					yield return worldProperty;
				}
			}
			List<WorldProperty>.Enumerator enumerator = default(List<WorldProperty>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001AB48 File Offset: 0x00018D48
		private bool AreFulfilled(IReadOnlyList<WorldProperty> worldProperties)
		{
			using (IEnumerator<WorldProperty> enumerator = worldProperties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsFulfilled(this.OwningPlanner.AIPreviewContext, this.OwningPlanner.AIPreviewPlayerState, this.OwningPlanner))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001ABB4 File Offset: 0x00018DB4
		public bool IsABidNode()
		{
			ActionID id = this.ID;
			return id == ActionID.Bid_Bazaar || id == ActionID.Bid_On_Artifact || id == ActionID.Bid_On_Legion || id == ActionID.Bid_On_Praetor;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001ABE4 File Offset: 0x00018DE4
		public override float GetEdgeCost(PFNode fromNode = null)
		{
			float num = base.GetEdgeCost(fromNode);
			GOAPNode goapnode = fromNode as GOAPNode;
			if (goapnode != null && goapnode.DoDynamicScoring)
			{
				foreach (WorldProperty worldProperty in goapnode._preconditions)
				{
					float num2 = worldProperty.CalculateDynamicCostScalar(this.OwningPlanner, this._effects);
					num *= num2;
				}
			}
			return num;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001AC60 File Offset: 0x00018E60
		public IEnumerable<GOAPNode> GetGOAPNeighbours()
		{
			foreach (PFNode pfnode in this.GetNeighbours())
			{
				GOAPNode goapnode = pfnode as GOAPNode;
				if (goapnode != null)
				{
					yield return goapnode;
				}
			}
			IEnumerator<PFNode> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001AC70 File Offset: 0x00018E70
		public override bool IsValidNeighbour(PFAgent agentData, PFNode callingNode)
		{
			return !this.IsDisabled();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001AC7D File Offset: 0x00018E7D
		public bool TryAddNeighbour(GOAPNode other)
		{
			if (!this.ShouldBeNeighbour(other))
			{
				return false;
			}
			base.AddNeighbour(other);
			return true;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001AC92 File Offset: 0x00018E92
		public bool ShouldBeNeighbour(GOAPNode other)
		{
			return this.ContributesToPreconditions(other.Effects);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001ACA0 File Offset: 0x00018EA0
		public bool ContributesToPreconditions(IReadOnlyList<WorldProperty> effects)
		{
			bool result;
			using (SimProfilerBlock.ProfilerBlock(""))
			{
				bool flag = false;
				foreach (WorldProperty worldProperty in this._preconditions)
				{
					bool mustBeSuccessfullFulfilledAsPrecondition = worldProperty.MustBeSuccessfullFulfilledAsPrecondition;
					if (mustBeSuccessfullFulfilledAsPrecondition || !flag)
					{
						bool flag2 = false;
						foreach (WorldProperty worldProperty2 in effects)
						{
							WPProvidesEffect wpprovidesEffect = worldProperty2.ProvidesEffect(worldProperty);
							flag2 |= (wpprovidesEffect == WPProvidesEffect.Yes);
							if (flag2)
							{
								break;
							}
						}
						flag = (flag || flag2);
						if (mustBeSuccessfullFulfilledAsPrecondition && !flag2)
						{
							return false;
						}
					}
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x060005AC RID: 1452
		public abstract Result SubmitAction(TurnContext context, PlayerState playerState);

		// Token: 0x060005AD RID: 1453 RVA: 0x0001AD88 File Offset: 0x00018F88
		public virtual void OnActionFailed()
		{
			base.Disable("Submission Failed");
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001AD95 File Offset: 0x00018F95
		public virtual void OnActionSubmitted()
		{
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001AD97 File Offset: 0x00018F97
		public virtual bool IsOrderOfType<TOrderType>() where TOrderType : ActionableOrder
		{
			return false;
		}

		// Token: 0x040002C8 RID: 712
		public GOAPPlanner OwningPlanner;

		// Token: 0x040002C9 RID: 713
		private Dictionary<PFNode, float> _heuristicDict = new Dictionary<PFNode, float>();

		// Token: 0x040002CA RID: 714
		private List<WorldProperty> _preconditions = new List<WorldProperty>(0);

		// Token: 0x040002CB RID: 715
		private List<WorldProperty> _effects = new List<WorldProperty>(0);

		// Token: 0x040002CC RID: 716
		private List<WorldProperty> _constraints = new List<WorldProperty>(0);

		// Token: 0x040002CD RID: 717
		private bool _constraintStatus = true;

		// Token: 0x040002CE RID: 718
		private bool _preconditionStatus = true;
	}
}
