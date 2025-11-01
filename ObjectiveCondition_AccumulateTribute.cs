using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000546 RID: 1350
	[Serializable]
	public class ObjectiveCondition_AccumulateTribute : ObjectiveCondition
	{
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001A2F RID: 6703 RVA: 0x0005B5AF File Offset: 0x000597AF
		public override string Name
		{
			get
			{
				return string.Format("Accumulate {0} {1}", this.Target, this.ResourceType.ToString());
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0005B5D7 File Offset: 0x000597D7
		public override string LocalizationKey
		{
			get
			{
				return "AccumulateTribute";
			}
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0005B5DE File Offset: 0x000597DE
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return owner.TotalResourcesIncludingPrestige[this.ResourceType];
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0005B5F1 File Offset: 0x000597F1
		public override int GetHashCode()
		{
			return (int)(base.GetHashCode() * 19 + this.ResourceType);
		}

		// Token: 0x04000BE1 RID: 3041
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public ResourceTypes ResourceType;
	}
}
