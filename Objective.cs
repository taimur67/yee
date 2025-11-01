using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200053E RID: 1342
	[Serializable]
	public class Objective : IDeepClone<Objective>
	{
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001A02 RID: 6658 RVA: 0x0005AE2C File Offset: 0x0005902C
		[JsonIgnore]
		public IReadOnlyList<ObjectiveCondition> Conditions
		{
			get
			{
				return this._conditions;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0005AE34 File Offset: 0x00059034
		public bool IsComplete
		{
			get
			{
				return this.NumComplete == this._conditions.Count && this._conditions.Count > 0;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001A04 RID: 6660 RVA: 0x0005AE59 File Offset: 0x00059059
		public int NumComplete
		{
			get
			{
				return this._conditions.Count((ObjectiveCondition t) => t.IsComplete);
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0005AE88 File Offset: 0x00059088
		public float StageProgress
		{
			get
			{
				if (this._conditions.Count <= 0)
				{
					return 1f;
				}
				return (float)this._conditions.Count((ObjectiveCondition t) => t.IsComplete) / (float)this._conditions.Count;
			}
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0005AEE1 File Offset: 0x000590E1
		public void ClearConditions()
		{
			this._conditions.Clear();
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0005AEF0 File Offset: 0x000590F0
		public float TotalProgress
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (ObjectiveCondition objectiveCondition in this._conditions)
				{
					num2 += objectiveCondition.Target;
					num += objectiveCondition.Count;
				}
				if (num2 == 0)
				{
					return 1f;
				}
				return (float)num / (float)num2;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x0005AF60 File Offset: 0x00059160
		public float WeightedProgress
		{
			get
			{
				if (this._conditions.Count == 0)
				{
					return 1f;
				}
				float num = 1f / (float)this._conditions.Count;
				float num2 = 0f;
				foreach (ObjectiveCondition objectiveCondition in this._conditions)
				{
					num2 += objectiveCondition.Progress * num;
				}
				num2 = MathUtils.RoundToNearest(num2, 0.01f);
				return num2;
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0005AFF0 File Offset: 0x000591F0
		public Objective() : this(Enumerable.Empty<ObjectiveCondition>())
		{
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x0005AFFD File Offset: 0x000591FD
		public Objective(params ObjectiveCondition[] conditions) : this(conditions.AsEnumerable<ObjectiveCondition>())
		{
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x0005B00B File Offset: 0x0005920B
		public Objective(IEnumerable<ObjectiveCondition> conditions)
		{
			this._conditions = IEnumerableExtensions.ToList<ObjectiveCondition>(conditions);
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0005B02C File Offset: 0x0005922C
		public virtual void Update(TurnContext context, PlayerState owner)
		{
			foreach (ObjectiveCondition objectiveCondition in this._conditions)
			{
				objectiveCondition.Update(context, owner);
			}
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0005B080 File Offset: 0x00059280
		public virtual bool IsPossible(TurnContext context, PlayerState owner)
		{
			return this._conditions.All((ObjectiveCondition condition) => condition.IsValidFor(context, owner).successful);
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0005B0B8 File Offset: 0x000592B8
		public string ToPercentage(float val)
		{
			return string.Format("{0:P2}.", val);
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0005B0CC File Offset: 0x000592CC
		public string FormatDebugStr(bool includeConditions)
		{
			string text = string.Format("{0} / {1} TP:{2} WP:{3}", new object[]
			{
				this.NumComplete,
				this._conditions.Count,
				this.ToPercentage(this.TotalProgress),
				this.ToPercentage(this.WeightedProgress)
			});
			if (includeConditions)
			{
				foreach (ObjectiveCondition objectiveCondition in this._conditions)
				{
					text = text + "\n " + objectiveCondition.FormatDebugStr();
				}
			}
			return text;
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0005B180 File Offset: 0x00059380
		public string FormatConditionStr(string separator = ", ")
		{
			return string.Join(separator, from t in this._conditions
			select t.Name);
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0005B1B4 File Offset: 0x000593B4
		public override int GetHashCode()
		{
			int num = 17;
			foreach (ObjectiveCondition objectiveCondition in this._conditions)
			{
				num = num * 19 + objectiveCondition.GetHashCode();
			}
			return num;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0005B210 File Offset: 0x00059410
		protected void DeepCloneObjectiveParts(Objective clone)
		{
			clone._conditions = this._conditions.DeepClone(CloneFunction.FastClone);
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0005B224 File Offset: 0x00059424
		public virtual void DeepClone(out Objective clone)
		{
			clone = new Objective();
			this.DeepCloneObjectiveParts(clone);
		}

		// Token: 0x04000BD5 RID: 3029
		[JsonProperty]
		protected List<ObjectiveCondition> _conditions = new List<ObjectiveCondition>();
	}
}
