using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200039F RID: 927
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerGameStatistics : IDeepClone<PlayerGameStatistics>
	{
		// Token: 0x060011B6 RID: 4534 RVA: 0x00043E94 File Offset: 0x00042094
		public int GetStatValue(string statisticKey)
		{
			int result;
			if (!this._stats.TryGetValue(statisticKey, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00043EB4 File Offset: 0x000420B4
		public void Clear()
		{
			this._stats.Clear();
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00043EC1 File Offset: 0x000420C1
		public void SetMax(string statisticKey, int value)
		{
			this.SetStatValue(statisticKey, Math.Max(value, this.GetStatValue(statisticKey)));
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00043ED7 File Offset: 0x000420D7
		public void SetMin(string statisticKey, int value)
		{
			this.SetStatValue(statisticKey, Math.Min(value, this.GetStatValue(statisticKey)));
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00043EED File Offset: 0x000420ED
		public void SetStatValue(string key, int value)
		{
			if (value == 0)
			{
				this._stats.Remove(key);
				return;
			}
			this._stats[key] = value;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00043F0D File Offset: 0x0004210D
		public void IncrementStatValue(string key, int amount = 1)
		{
			if (amount != 0)
			{
				this.SetStatValue(key, this.GetStatValue(key) + amount);
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00043F22 File Offset: 0x00042122
		public void Process(string key, StatisticMetricAccumulation accumulation, int value)
		{
			switch (accumulation)
			{
			case StatisticMetricAccumulation.Increment:
				this.IncrementStatValue(key, value);
				return;
			case StatisticMetricAccumulation.Max:
				this.SetMax(key, value);
				return;
			case StatisticMetricAccumulation.Min:
				this.SetMin(key, value);
				return;
			case StatisticMetricAccumulation.Set:
				this.SetStatValue(key, value);
				return;
			default:
				return;
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00043F60 File Offset: 0x00042160
		public void RandomizeAll(Random random, IEnumerable<string> keys, int min = 0, int max = 10)
		{
			foreach (string key in keys)
			{
				this.SetStatValue(key, random.Next(min, max + 1));
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00043FB4 File Offset: 0x000421B4
		public void DeepClone(out PlayerGameStatistics clone)
		{
			clone = new PlayerGameStatistics
			{
				_stats = this._stats.DeepClone()
			};
		}

		// Token: 0x04000812 RID: 2066
		[JsonProperty]
		private Dictionary<string, int> _stats = new Dictionary<string, int>();
	}
}
