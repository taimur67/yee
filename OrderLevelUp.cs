using System;
using System.Collections.Generic;
using System.ComponentModel;
using Game.Simulation.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000638 RID: 1592
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class OrderLevelUp : ActionableOrder
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x00065B33 File Offset: 0x00063D33
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.LevelUp;
			}
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x00065B37 File Offset: 0x00063D37
		[JsonConstructor]
		public OrderLevelUp()
		{
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x00065B46 File Offset: 0x00063D46
		public OrderLevelUp(PowerType powerType, int level)
		{
			this.PowerType = powerType;
			this.Level = level;
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x00065B63 File Offset: 0x00063D63
		public bool HasOptions(GameDatabase db)
		{
			return db.GetPowerLevelData(this.PowerType, this.Level).HasOptions();
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x00065B7C File Offset: 0x00063D7C
		public override IEnumerable<ActionConflict> GeneratePotentialConflicts()
		{
			yield return new PowerUpConflict(this.PowerType);
			yield break;
		}

		// Token: 0x04000C8B RID: 3211
		[BindableValue("power_name", BindingOption.None)]
		[JsonProperty]
		[DefaultValue(PowerType.None)]
		public PowerType PowerType = PowerType.None;

		// Token: 0x04000C8C RID: 3212
		[JsonProperty]
		public int Level;
	}
}
