using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002AA RID: 682
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_LocalRitualResistance : EntityTag
	{
		// Token: 0x06000D1D RID: 3357 RVA: 0x000348DC File Offset: 0x00032ADC
		public int PowerTypeToResistanceValue(PowerType ritualPowerType)
		{
			int result;
			switch (ritualPowerType)
			{
			case PowerType.Deceit:
				result = this.DeceitResistance;
				break;
			case PowerType.Prophecy:
				result = this.ProphecyResistance;
				break;
			case PowerType.Destruction:
				result = this.DestructionResistance;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00034920 File Offset: 0x00032B20
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_LocalRitualResistance entityTag_LocalRitualResistance = new EntityTag_LocalRitualResistance
			{
				DeceitResistance = this.DeceitResistance,
				ProphecyResistance = this.ProphecyResistance,
				DestructionResistance = this.DestructionResistance
			};
			base.DeepCloneEntityTagParts(entityTag_LocalRitualResistance);
			clone = entityTag_LocalRitualResistance;
		}

		// Token: 0x040005CE RID: 1486
		[JsonProperty]
		[DefaultValue(0)]
		public int DeceitResistance;

		// Token: 0x040005CF RID: 1487
		[JsonProperty]
		[DefaultValue(0)]
		public int ProphecyResistance;

		// Token: 0x040005D0 RID: 1488
		[JsonProperty]
		[DefaultValue(0)]
		public int DestructionResistance;
	}
}
