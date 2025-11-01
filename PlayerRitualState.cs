using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003A2 RID: 930
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class PlayerRitualState : IModifiable, IDeepClone<PlayerRitualState>
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x000443F4 File Offset: 0x000425F4
		public int AvailableSlots
		{
			get
			{
				return this.NumRitualSlots.Value - this.SlottedItems.Count;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x0004440D File Offset: 0x0004260D
		public bool HasSpace
		{
			get
			{
				return this.AvailableSlots > 0;
			}
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00044418 File Offset: 0x00042618
		public ModifiableValue GetStrengthBonus(PowerType powerType)
		{
			return this.RitualStrengthBonus[powerType].CurrentLevel;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0004442B File Offset: 0x0004262B
		public ModifiableValue GetResistanceBonus(PowerType powerType)
		{
			return this.RitualResistanceBonus[powerType].CurrentLevel;
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0004443E File Offset: 0x0004263E
		public bool TryAndRemoveItem(Identifier item)
		{
			return this.SlottedItems.Remove(item);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x0004444C File Offset: 0x0004264C
		public void ClearModifiers()
		{
			this.RitualStrengthBonus.ClearModifiers();
			this.RitualResistanceBonus.ClearModifiers();
			this.ClearStatModifiers();
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x0004446C File Offset: 0x0004266C
		public void DeepClone(out PlayerRitualState clone)
		{
			clone = new PlayerRitualState
			{
				RitualStrengthBonus = this.RitualStrengthBonus.DeepClone<PlayerPowersLevels>(),
				RitualResistanceBonus = this.RitualResistanceBonus.DeepClone<PlayerPowersLevels>(),
				NumRitualSlots = this.NumRitualSlots.DeepClone<ModifiableValue>(),
				ScryingAvailable = this.ScryingAvailable.DeepClone<ModifiableBool>(),
				SlottedItems = this.SlottedItems.DeepClone()
			};
		}

		// Token: 0x0400081C RID: 2076
		[JsonProperty]
		public PlayerPowersLevels RitualStrengthBonus = new PlayerPowersLevels(0, int.MaxValue);

		// Token: 0x0400081D RID: 2077
		[JsonProperty]
		public PlayerPowersLevels RitualResistanceBonus = new PlayerPowersLevels(0, int.MaxValue);

		// Token: 0x0400081E RID: 2078
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableValue NumRitualSlots = new ModifiableValue(1f, 1, int.MaxValue, RoundingMode.RoundDown);

		// Token: 0x0400081F RID: 2079
		[JsonProperty]
		[DefaultValue(1)]
		public ModifiableBool ScryingAvailable = new ModifiableBool(false, LogicOperation.Or);

		// Token: 0x04000820 RID: 2080
		[JsonProperty]
		public List<Identifier> SlottedItems = new List<Identifier>();
	}
}
