using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000399 RID: 921
	[Serializable]
	public class StatModifier : StatModifierBase, IDeepClone<StatModifier>
	{
		// Token: 0x060011A3 RID: 4515 RVA: 0x00043B9D File Offset: 0x00041D9D
		[JsonConstructor]
		public StatModifier()
		{
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060011A4 RID: 4516 RVA: 0x00043BA5 File Offset: 0x00041DA5
		public bool IsNegativeContribution
		{
			get
			{
				return this.ApplyModification(1f) < 1f;
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00043BB9 File Offset: 0x00041DB9
		public StatModifier(int value, ModifierContext provider, ModifierTarget targetType = ModifierTarget.ValueOffset)
		{
			this.Provider = provider;
			this.Value = value;
			this.TargetType = targetType;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00043BD8 File Offset: 0x00041DD8
		public float ApplyModification(float workingValue)
		{
			if (this.TargetType == ModifierTarget.ValueOffset)
			{
				workingValue += (float)this.Value;
			}
			else
			{
				ModifierTarget targetType = this.TargetType;
				if (targetType == ModifierTarget.ValueMultiplierOffset || targetType == ModifierTarget.ValueMultiplierCompound)
				{
					workingValue += workingValue * (float)this.Value;
				}
				else if (this.TargetType == ModifierTarget.ValueScalar)
				{
					workingValue *= (float)this.Value;
				}
				else if (this.TargetType == ModifierTarget.ValuePercentagePointScalar)
				{
					workingValue *= (float)this.Value / 100f;
				}
			}
			return workingValue;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00043C4C File Offset: 0x00041E4C
		public override void DeepClone(out StatModifierBase clone)
		{
			StatModifier statModifier;
			this.DeepClone(out statModifier);
			clone = statModifier;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00043C64 File Offset: 0x00041E64
		public void DeepClone(out StatModifier clone)
		{
			clone = new StatModifier
			{
				TargetType = this.TargetType,
				Value = this.Value
			};
			base.DeepCloneModifierBaseParts(clone);
		}

		// Token: 0x0400080A RID: 2058
		[JsonProperty]
		public ModifierTarget TargetType;

		// Token: 0x0400080B RID: 2059
		[JsonProperty]
		public int Value;
	}
}
