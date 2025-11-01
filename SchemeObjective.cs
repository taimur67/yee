using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003A4 RID: 932
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SchemeObjective : Objective, IDeepClone<SchemeObjective>
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0004453C File Offset: 0x0004273C
		// (set) Token: 0x060011E2 RID: 4578 RVA: 0x00044544 File Offset: 0x00042744
		public SchemeId Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x0004454D File Offset: 0x0004274D
		[JsonIgnore]
		public bool IsVisible
		{
			get
			{
				return this.IsRevealed || this.IsPublic;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x0004455F File Offset: 0x0004275F
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x0004456A File Offset: 0x0004276A
		[JsonIgnore]
		public bool IsPublic
		{
			get
			{
				return !this.IsPrivate;
			}
			set
			{
				this.IsPrivate = !value;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00044576 File Offset: 0x00042776
		[JsonIgnore]
		public int Reward
		{
			get
			{
				if (!this.IsPrivate && !this.IsRevealed)
				{
					return this.PublicReward;
				}
				return this.PrivateReward;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00044595 File Offset: 0x00042795
		// (set) Token: 0x060011E8 RID: 4584 RVA: 0x0004459D File Offset: 0x0004279D
		[JsonProperty]
		public bool IsGrandScheme { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x000445A6 File Offset: 0x000427A6
		// (set) Token: 0x060011EA RID: 4586 RVA: 0x000445AE File Offset: 0x000427AE
		[JsonProperty]
		[DefaultValue(true)]
		public bool IsValid { get; private set; } = true;

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x000445B7 File Offset: 0x000427B7
		[JsonIgnore]
		public bool IsSimpleScheme
		{
			get
			{
				return !this.IsGrandScheme;
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x000445C2 File Offset: 0x000427C2
		[JsonConstructor]
		public SchemeObjective()
		{
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x000445D8 File Offset: 0x000427D8
		public SchemeObjective(params ObjectiveCondition[] conditions) : base(conditions)
		{
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x000445EF File Offset: 0x000427EF
		public SchemeObjective(IEnumerable<ObjectiveCondition> conditions) : base(conditions)
		{
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00044606 File Offset: 0x00042806
		public string FormatDebugStr()
		{
			return this.SourceId + " - " + base.FormatConditionStr(", ");
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00044623 File Offset: 0x00042823
		public void RevealPrivateScheme()
		{
			this.IsRevealed = true;
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0004462C File Offset: 0x0004282C
		public void SetStartingProgress(TurnContext context, PlayerState owner)
		{
			foreach (ObjectiveCondition objectiveCondition in this._conditions)
			{
				objectiveCondition.SetStartingProgress(context, owner);
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00044680 File Offset: 0x00042880
		public override void Update(TurnContext context, PlayerState owner)
		{
			base.Update(context, owner);
			this.IsValid = this.CanBeCompleted(context, owner).successful;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x000446A0 File Offset: 0x000428A0
		private Result CanBeCompleted(TurnContext context, PlayerState player)
		{
			foreach (ObjectiveCondition objectiveCondition in base.Conditions)
			{
				if (!objectiveCondition.IsComplete)
				{
					Problem problem = objectiveCondition.CanBeCompleted(context, player) as Problem;
					if (problem != null)
					{
						return problem;
					}
				}
			}
			return Result.Success;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0004470C File Offset: 0x0004290C
		public void AddCondition(ObjectiveCondition condition)
		{
			this._conditions.Add(condition);
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0004471C File Offset: 0x0004291C
		public ObjectiveDifficulty CalculateDynamicDifficulty(TurnContext context, PlayerState player)
		{
			if (base.Conditions.Count <= 0)
			{
				return ObjectiveDifficulty.Easy;
			}
			List<IDynamicObjective> list = IEnumerableExtensions.ToList<IDynamicObjective>(base.Conditions.OfType<IDynamicObjective>());
			if (!IEnumerableExtensions.Any<IDynamicObjective>(list))
			{
				return ObjectiveDifficulty.Easy;
			}
			return (ObjectiveDifficulty)Math.Round((double)(from x in list
			select x.CalculateDifficulty(context, player)).Average((ObjectiveDifficulty x) => (float)x), 0);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x000447A8 File Offset: 0x000429A8
		protected void DeepCloneSchemeObjectiveParts(SchemeObjective clone)
		{
			clone.IsRevealed = this.IsRevealed;
			clone.IsPrivate = this.IsPrivate;
			clone._id = this._id;
			clone.PrivateReward = this.PrivateReward;
			clone.PublicReward = this.PublicReward;
			clone.SourceId = this.SourceId.DeepClone();
			clone.IsGrandScheme = this.IsGrandScheme;
			clone.IsValid = this.IsValid;
			base.DeepCloneObjectiveParts(clone);
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00044821 File Offset: 0x00042A21
		public virtual void DeepClone(out SchemeObjective clone)
		{
			clone = new SchemeObjective();
			this.DeepCloneSchemeObjectiveParts(clone);
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00044834 File Offset: 0x00042A34
		public override void DeepClone(out Objective clone)
		{
			SchemeObjective schemeObjective;
			this.DeepClone(out schemeObjective);
			clone = schemeObjective;
		}

		// Token: 0x04000823 RID: 2083
		[JsonProperty]
		public bool IsRevealed;

		// Token: 0x04000824 RID: 2084
		[JsonProperty]
		public bool IsPrivate;

		// Token: 0x04000825 RID: 2085
		[JsonProperty]
		[DefaultValue(SchemeId.Invalid)]
		private SchemeId _id = SchemeId.Invalid;

		// Token: 0x04000826 RID: 2086
		[JsonProperty]
		public int PrivateReward;

		// Token: 0x04000827 RID: 2087
		[JsonProperty]
		public int PublicReward;

		// Token: 0x04000828 RID: 2088
		[JsonProperty]
		public string SourceId;
	}
}
