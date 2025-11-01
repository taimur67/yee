using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.Utils;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003A0 RID: 928
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerPowerLevel : IDontSerializeIfDefault, IDeepClone<PlayerPowerLevel>
	{
		// Token: 0x060011C0 RID: 4544 RVA: 0x00043FE1 File Offset: 0x000421E1
		public bool IsDefault(DefaultValueAttribute defaultValueAttribute)
		{
			return this.CurrentLevel.IsDefault(0f, 0, 6, RoundingMode.RoundDown) && this._chosenAbilities.Count == 0;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00044008 File Offset: 0x00042208
		[JsonConstructor]
		public PlayerPowerLevel()
		{
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0004401B File Offset: 0x0004221B
		public PlayerPowerLevel(int initVal, int upperBound = 6)
		{
			this.CurrentLevel = new ModifiableValue(0f, 0, upperBound, RoundingMode.RoundDown);
			this.CurrentLevel.SetBase((float)initVal);
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0004404E File Offset: 0x0004224E
		public static implicit operator int(PlayerPowerLevel powerLevel)
		{
			return powerLevel.CurrentLevel.Value;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0004405B File Offset: 0x0004225B
		public void SetChosenAbility(int level, int choice)
		{
			if (this._chosenAbilities.ContainsKey(level))
			{
				this._chosenAbilities[level] = choice;
				return;
			}
			this._chosenAbilities.Add(level, choice);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00044088 File Offset: 0x00042288
		public int GetChosenAbility(int level)
		{
			int result;
			if (!this._chosenAbilities.TryGetValue(level, out result))
			{
				return -1;
			}
			return result;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x000440A8 File Offset: 0x000422A8
		public void SetLevel(int level)
		{
			this.CurrentLevel.SetBase((float)level);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x000440B7 File Offset: 0x000422B7
		public bool HasChosenAbility(int level)
		{
			return this._chosenAbilities.ContainsKey(level);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x000440C5 File Offset: 0x000422C5
		public void DeepClone(out PlayerPowerLevel clone)
		{
			clone = new PlayerPowerLevel
			{
				CurrentLevel = this.CurrentLevel.DeepClone<ModifiableValue>(),
				_chosenAbilities = this._chosenAbilities.DeepClone()
			};
		}

		// Token: 0x04000813 RID: 2067
		public const int DefaultUpperBound = 6;

		// Token: 0x04000814 RID: 2068
		[JsonProperty]
		public ModifiableValue CurrentLevel;

		// Token: 0x04000815 RID: 2069
		[JsonProperty]
		private Dictionary<int, int> _chosenAbilities = new Dictionary<int, int>();
	}
}
