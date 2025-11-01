using System;
using System.ComponentModel;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002AB RID: 683
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EntityTag_PreventArchfiendStatBaseAdjustmentsBelowValue : EntityTag
	{
		// Token: 0x06000D20 RID: 3360 RVA: 0x0003496C File Offset: 0x00032B6C
		public override void DeepClone(out EntityTag clone)
		{
			EntityTag_PreventArchfiendStatBaseAdjustmentsBelowValue entityTag_PreventArchfiendStatBaseAdjustmentsBelowValue = new EntityTag_PreventArchfiendStatBaseAdjustmentsBelowValue
			{
				Stat = this.Stat,
				Threshold = this.Threshold
			};
			base.DeepCloneEntityTagParts(entityTag_PreventArchfiendStatBaseAdjustmentsBelowValue);
			clone = entityTag_PreventArchfiendStatBaseAdjustmentsBelowValue;
		}

		// Token: 0x040005D1 RID: 1489
		[JsonProperty]
		[DefaultValue(ArchfiendStat.None)]
		public ArchfiendStat Stat = ArchfiendStat.None;

		// Token: 0x040005D2 RID: 1490
		[JsonProperty]
		public int Threshold;
	}
}
