using System;
using System.Collections.Generic;
using System.Linq;
using Game.Simulation.StaticData;

namespace LoG
{
	// Token: 0x02000529 RID: 1321
	[Serializable]
	public class VendettaObjective : Objective, IDeepClone<VendettaObjective>
	{
		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060019B4 RID: 6580 RVA: 0x0005A2B1 File Offset: 0x000584B1
		public virtual string ObjectiveKey
		{
			get
			{
				return "Vendetta";
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0005A2B8 File Offset: 0x000584B8
		public VendettaObjective()
		{
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0005A2C0 File Offset: 0x000584C0
		public VendettaObjective(params ObjectiveCondition[] conditions) : base(conditions)
		{
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0005A2C9 File Offset: 0x000584C9
		public VendettaObjective(IEnumerable<ObjectiveCondition> conditions) : base(conditions)
		{
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0005A2D2 File Offset: 0x000584D2
		public static IEnumerable<VendettaObjective> GenerateVendettaObjectiveOptions(GameDatabase database, TurnState turn, PlayerState player, PlayerState target)
		{
			foreach (VendettaObjectiveGroup group in database.Enumerate<VendettaObjectiveGroup>())
			{
				foreach (VendettaObjective vendettaObjective in group.GetObjectives(database, turn, player, target))
				{
					if (vendettaObjective != null)
					{
						vendettaObjective.MinTurnLimit = group.MinTurnLimit;
						vendettaObjective.MaxTurnLimit = group.MaxTurnLimit;
						yield return vendettaObjective;
					}
				}
				IEnumerator<VendettaObjective> enumerator2 = null;
				group = null;
			}
			IEnumerator<VendettaObjectiveGroup> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0005A2F7 File Offset: 0x000584F7
		public static IEnumerable<VendettaObjective> GenerateVendettaObjectiveOptions(VendettaHeuristics.ObjectiveType objectiveType, GameDatabase database, TurnState turn, PlayerState instigator, PlayerState target)
		{
			string objectiveTypeIdentifier = objectiveType.GetIdentifier();
			if (string.IsNullOrEmpty(objectiveTypeIdentifier))
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error(string.Format("Unable to get objective type identifier for {0}", objectiveType));
				}
				yield break;
			}
			foreach (VendettaObjective vendettaObjective in VendettaObjective.GenerateVendettaObjectiveOptions(database, turn, instigator, target))
			{
				if (vendettaObjective.Id.Contains(objectiveTypeIdentifier))
				{
					yield return vendettaObjective;
				}
			}
			IEnumerator<VendettaObjective> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0005A324 File Offset: 0x00058524
		public static bool TryGenerateObjective(VendettaHeuristics.VendettaParameters parameters, GameDatabase database, TurnState turn, PlayerState instigator, PlayerState target, out VendettaObjective result)
		{
			List<VendettaObjective> list = IEnumerableExtensions.ToList<VendettaObjective>(from obj in VendettaObjective.GenerateVendettaObjectiveOptions(parameters.ObjectiveType, database, turn, instigator, target)
			where obj.GetTotalConditionTargets() == parameters.ThresholdToReach
			select obj);
			SimLogger logger = SimLogger.Logger;
			if (logger != null)
			{
				logger.ErrorIf(list.Count != 1, string.Format("There must be exactly 1 objective of type {0} with rank {1}, but found {2}", parameters.ObjectiveType, parameters.ThresholdToReach, list.Count));
			}
			result = IEnumerableExtensions.FirstOrDefault<VendettaObjective>(list);
			return result != null;
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0005A3CC File Offset: 0x000585CC
		public static bool TryGetVendettaObjectiveBounds(VendettaHeuristics.VendettaParameters parameters, GameDatabase database, TurnState turn, PlayerState instigator, PlayerState target, out int minTurns, out int maxTurns, out int minThreshold, out int maxThreshold)
		{
			minTurns = (minThreshold = int.MaxValue);
			maxTurns = (maxThreshold = int.MinValue);
			foreach (VendettaObjective vendettaObjective in VendettaObjective.GenerateVendettaObjectiveOptions(parameters.ObjectiveType, database, turn, instigator, target))
			{
				if (minTurns > vendettaObjective.MinTurnLimit)
				{
					minTurns = vendettaObjective.MinTurnLimit;
				}
				if (maxTurns < vendettaObjective.MaxTurnLimit)
				{
					maxTurns = vendettaObjective.MaxTurnLimit;
				}
				int totalConditionTargets = vendettaObjective.GetTotalConditionTargets();
				if (minThreshold > totalConditionTargets)
				{
					minThreshold = totalConditionTargets;
				}
				if (maxThreshold < totalConditionTargets)
				{
					maxThreshold = totalConditionTargets;
				}
			}
			return minTurns < int.MaxValue && minThreshold < int.MaxValue && maxTurns > int.MinValue && maxThreshold > int.MinValue;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0005A4A8 File Offset: 0x000586A8
		public virtual bool IsCompleted(TurnState turn)
		{
			return base.IsComplete;
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x0005A4B0 File Offset: 0x000586B0
		public virtual int GetTotalConditionTargets()
		{
			return this._conditions.Sum((ObjectiveCondition t) => t.Target);
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0005A4DC File Offset: 0x000586DC
		protected void DeepCloneVendettaObjectiveParts(VendettaObjective clone)
		{
			clone.ObjectiveDifficulty = this.ObjectiveDifficulty;
			clone.MinTurnLimit = this.MinTurnLimit;
			clone.MaxTurnLimit = this.MaxTurnLimit;
			clone.SourceId = this.SourceId.DeepClone();
			clone.Id = this.Id.DeepClone();
			base.DeepCloneObjectiveParts(clone);
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0005A536 File Offset: 0x00058736
		public virtual void DeepClone(out VendettaObjective clone)
		{
			clone = new VendettaObjective();
			this.DeepCloneVendettaObjectiveParts(clone);
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0005A548 File Offset: 0x00058748
		public override void DeepClone(out Objective clone)
		{
			VendettaObjective vendettaObjective;
			this.DeepClone(out vendettaObjective);
			clone = vendettaObjective;
		}

		// Token: 0x04000BB9 RID: 3001
		public float ObjectiveDifficulty;

		// Token: 0x04000BBA RID: 3002
		public int MinTurnLimit;

		// Token: 0x04000BBB RID: 3003
		public int MaxTurnLimit;

		// Token: 0x04000BBC RID: 3004
		public string SourceId;

		// Token: 0x04000BBD RID: 3005
		public string Id;
	}
}
