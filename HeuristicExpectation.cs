using System;

namespace LoG
{
	// Token: 0x020006E3 RID: 1763
	public struct HeuristicExpectation
	{
		// Token: 0x0600215E RID: 8542 RVA: 0x00073E10 File Offset: 0x00072010
		private HeuristicExpectation(HeuristicKey key, HeuristicValueType valueType = HeuristicValueType.Value)
		{
			this.Key = key;
			this.ValueType = valueType;
			this.MinValue = float.MinValue;
			this.MaxValue = float.MaxValue;
			this.Coverage = 1f;
			this.Enabled = true;
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x00073E48 File Offset: 0x00072048
		public HeuristicExpectation Disable()
		{
			this.Enabled = false;
			return this;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00073E58 File Offset: 0x00072058
		public static HeuristicExpectation Min(HeuristicKey key, float val, float coverage = 1f)
		{
			return new HeuristicExpectation(key, HeuristicValueType.Value)
			{
				MinValue = val,
				Coverage = coverage
			};
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x00073E80 File Offset: 0x00072080
		public static HeuristicExpectation Min(HeuristicKey key, HeuristicValueType valType, float val, float coverage = 1f)
		{
			return new HeuristicExpectation(key, valType)
			{
				MinValue = val,
				Coverage = coverage
			};
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x00073EA8 File Offset: 0x000720A8
		public static HeuristicExpectation Max(HeuristicKey key, float val, float coverage = 1f)
		{
			return new HeuristicExpectation(key, HeuristicValueType.Value)
			{
				MaxValue = val,
				Coverage = coverage
			};
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x00073ED0 File Offset: 0x000720D0
		public static HeuristicExpectation Max(HeuristicKey key, HeuristicValueType valType, float val, float coverage = 1f)
		{
			return new HeuristicExpectation(key, valType)
			{
				MaxValue = val,
				Coverage = coverage
			};
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00073EF8 File Offset: 0x000720F8
		public static HeuristicExpectation Between(HeuristicKey key, float min, float max, float coverage = 1f)
		{
			return new HeuristicExpectation(key, HeuristicValueType.Value)
			{
				MinValue = min,
				MaxValue = max,
				Coverage = coverage
			};
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00073F28 File Offset: 0x00072128
		public static HeuristicExpectation Between(HeuristicKey key, HeuristicValueType valType, float min, float max, float coverage = 1f)
		{
			return new HeuristicExpectation(key, valType)
			{
				MinValue = min,
				MaxValue = max,
				Coverage = coverage
			};
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x00073F58 File Offset: 0x00072158
		public string ConditionString()
		{
			if (this.MinValue == this.MaxValue)
			{
				return string.Format("=={0}", this.MaxValue);
			}
			if (this.MinValue == -3.4028235E+38f)
			{
				return string.Format("<={0}", this.MaxValue);
			}
			if (this.MaxValue == 3.4028235E+38f)
			{
				return string.Format(">={0}", this.MinValue);
			}
			return string.Format("{0}<{1}", this.MinValue, this.MaxValue);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x00073FF0 File Offset: 0x000721F0
		public string NameString()
		{
			string text = this.Enabled ? string.Empty : "(DISABLED)";
			return string.Format("{0}{1}.{2} {3}", new object[]
			{
				text,
				this.Key,
				this.ValueType,
				this.ConditionString()
			});
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0007404B File Offset: 0x0007224B
		public override string ToString()
		{
			return string.Format("{0} in {1:F2}%", this.NameString(), this.Coverage * 100f);
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x00074070 File Offset: 0x00072270
		public string ToString(float successRate)
		{
			string text = (successRate >= this.Coverage) ? ">=" : "<";
			return string.Format("{0} Coverage: ({1:F2} {2} {3:F2})", new object[]
			{
				this.NameString(),
				successRate,
				text,
				this.Coverage
			});
		}

		// Token: 0x04000EF0 RID: 3824
		public HeuristicKey Key;

		// Token: 0x04000EF1 RID: 3825
		public HeuristicValueType ValueType;

		// Token: 0x04000EF2 RID: 3826
		public float MinValue;

		// Token: 0x04000EF3 RID: 3827
		public float MaxValue;

		// Token: 0x04000EF4 RID: 3828
		public float Coverage;

		// Token: 0x04000EF5 RID: 3829
		public bool Enabled;
	}
}
