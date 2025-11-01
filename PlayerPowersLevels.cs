using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003A1 RID: 929
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerPowersLevels : IModifiable, IDeepClone<PlayerPowersLevels>
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x000440F0 File Offset: 0x000422F0
		public static IEnumerable<PowerType> PowerTypes
		{
			get
			{
				return PowerTypeExtensions.ValidTypes;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x000440F7 File Offset: 0x000422F7
		[JsonIgnore]
		public IEnumerable<PlayerPowerLevel> Powers
		{
			get
			{
				yield return this.Wrath;
				yield return this.Deceit;
				yield return this.Prophecy;
				yield return this.Destruction;
				yield return this.Charisma;
				yield break;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x00044107 File Offset: 0x00042307
		[JsonIgnore]
		public IEnumerable<ValueTuple<PowerType, PlayerPowerLevel>> PowerTypeLevels
		{
			get
			{
				return from t in PlayerPowersLevels.PowerTypes
				select new ValueTuple<PowerType, PlayerPowerLevel>(t, this[t]);
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00044120 File Offset: 0x00042320
		public ValueTuple<PowerType, PlayerPowerLevel> GetHighestPower()
		{
			PlayerPowerLevel playerPowerLevel = null;
			PowerType item = PowerType.None;
			foreach (PowerType powerType in PlayerPowersLevels.PowerTypes)
			{
				PlayerPowerLevel playerPowerLevel2 = this.Get(powerType);
				if (playerPowerLevel == null || playerPowerLevel2.CurrentLevel > playerPowerLevel.CurrentLevel)
				{
					item = powerType;
					playerPowerLevel = playerPowerLevel2;
				}
			}
			return new ValueTuple<PowerType, PlayerPowerLevel>(item, playerPowerLevel);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0004419C File Offset: 0x0004239C
		public PlayerPowerLevel Get(PowerType power)
		{
			PlayerPowerLevel result;
			switch (power)
			{
			case PowerType.Wrath:
				result = this.Wrath;
				break;
			case PowerType.Deceit:
				result = this.Deceit;
				break;
			case PowerType.Prophecy:
				result = this.Prophecy;
				break;
			case PowerType.Destruction:
				result = this.Destruction;
				break;
			case PowerType.Charisma:
				result = this.Charisma;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x000441F8 File Offset: 0x000423F8
		public void Set(PowerType power, PlayerPowerLevel level)
		{
			switch (power)
			{
			case PowerType.None:
				break;
			case PowerType.Wrath:
				this.Wrath = level;
				return;
			case PowerType.Deceit:
				this.Deceit = level;
				return;
			case PowerType.Prophecy:
				this.Prophecy = level;
				return;
			case PowerType.Destruction:
				this.Destruction = level;
				return;
			case PowerType.Charisma:
				this.Charisma = level;
				break;
			default:
				return;
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0004424D File Offset: 0x0004244D
		[JsonConstructor]
		public PlayerPowersLevels() : this(0, 6)
		{
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00044258 File Offset: 0x00042458
		public PlayerPowersLevels(int initVal, int upperBound = 6)
		{
			this.Wrath = new PlayerPowerLevel(initVal, upperBound);
			this.Deceit = new PlayerPowerLevel(initVal, upperBound);
			this.Prophecy = new PlayerPowerLevel(initVal, upperBound);
			this.Destruction = new PlayerPowerLevel(initVal, upperBound);
			this.Charisma = new PlayerPowerLevel(initVal, upperBound);
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x000442AC File Offset: 0x000424AC
		public void SetAll(int val)
		{
			foreach (PlayerPowerLevel playerPowerLevel in this.Powers)
			{
				playerPowerLevel.SetLevel(val);
			}
		}

		// Token: 0x17000298 RID: 664
		public PlayerPowerLevel this[PowerType type]
		{
			get
			{
				return this.Get(type);
			}
			set
			{
				this.Set(type, value);
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0004430B File Offset: 0x0004250B
		public PlayerPowersLevels SetValue(PowerType power, int value)
		{
			this.Get(power).SetLevel(value);
			return this;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0004431B File Offset: 0x0004251B
		public int GetChosenAbility(PowerType power, int level)
		{
			return this[power].GetChosenAbility(level);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0004432C File Offset: 0x0004252C
		public void ClearModifiers()
		{
			foreach (PlayerPowerLevel playerPowerLevel in this.Powers)
			{
				playerPowerLevel.CurrentLevel.ClearModifiers();
			}
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0004437C File Offset: 0x0004257C
		public void DeepClone(out PlayerPowersLevels clone)
		{
			clone = new PlayerPowersLevels
			{
				Wrath = this.Wrath.DeepClone<PlayerPowerLevel>(),
				Deceit = this.Deceit.DeepClone<PlayerPowerLevel>(),
				Prophecy = this.Prophecy.DeepClone<PlayerPowerLevel>(),
				Destruction = this.Destruction.DeepClone<PlayerPowerLevel>(),
				Charisma = this.Charisma.DeepClone<PlayerPowerLevel>()
			};
		}

		// Token: 0x04000816 RID: 2070
		public const int UNKNOWN_POWER_VALUE = -2147483648;

		// Token: 0x04000817 RID: 2071
		[JsonProperty]
		private PlayerPowerLevel Wrath;

		// Token: 0x04000818 RID: 2072
		[JsonProperty]
		private PlayerPowerLevel Deceit;

		// Token: 0x04000819 RID: 2073
		[JsonProperty]
		private PlayerPowerLevel Prophecy;

		// Token: 0x0400081A RID: 2074
		[JsonProperty]
		private PlayerPowerLevel Destruction;

		// Token: 0x0400081B RID: 2075
		[JsonProperty]
		private PlayerPowerLevel Charisma;
	}
}
