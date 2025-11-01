using System;
using System.Linq;

namespace LoG
{
	// Token: 0x02000559 RID: 1369
	[Serializable]
	public class ObjectiveCondition_CardPower : ObjectiveCondition
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0005C094 File Offset: 0x0005A294
		public override string LocalizationKey
		{
			get
			{
				string result;
				switch (this.PowerType)
				{
				case ObjectiveCondition_CardPower.CardPowerTargetType.AnyValue:
					result = base.LocalizationKey + ".Any";
					break;
				case ObjectiveCondition_CardPower.CardPowerTargetType.AllValues:
					result = base.LocalizationKey + ".All";
					break;
				case ObjectiveCondition_CardPower.CardPowerTargetType.TotalValue:
					result = base.LocalizationKey + ".Total";
					break;
				default:
					result = base.LocalizationKey;
					break;
				}
				return result;
			}
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0005C0FD File Offset: 0x0005A2FD
		protected override int CalculateTotalProgress(TurnContext context, PlayerState owner, bool isInitialProgress)
		{
			return owner.Resources.Count(new Func<ResourceNFT, bool>(this.ValidResources));
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0005C118 File Offset: 0x0005A318
		public bool ValidResources(ResourceNFT resource)
		{
			bool result;
			switch (this.PowerType)
			{
			case ObjectiveCondition_CardPower.CardPowerTargetType.AnyValue:
				result = resource.Values.AnyGreaterThanOrEqualTo(this.TargetPower, false);
				break;
			case ObjectiveCondition_CardPower.CardPowerTargetType.AllValues:
				result = resource.Values.GreaterThanOrEqualTo(this.TargetPower, false);
				break;
			case ObjectiveCondition_CardPower.CardPowerTargetType.TotalValue:
				result = (resource.Values.ValueSumNotIncludingPrestige() >= this.TargetPower);
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0005C186 File Offset: 0x0005A386
		public override int GetHashCode()
		{
			return (int)((base.GetHashCode() * 23 + this.PowerType) * (ObjectiveCondition_CardPower.CardPowerTargetType)29 + this.TargetPower);
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x0005C1A2 File Offset: 0x0005A3A2
		public override string Name
		{
			get
			{
				return string.Format("Own {0} tribute with {1} >= {2}", this.Target, this.PowerType, this.TargetPower);
			}
		}

		// Token: 0x04000BF6 RID: 3062
		public ObjectiveCondition_CardPower.CardPowerTargetType PowerType;

		// Token: 0x04000BF7 RID: 3063
		[BindableValue("target_value", BindingOption.None)]
		public int TargetPower;

		// Token: 0x02000A14 RID: 2580
		public enum CardPowerTargetType
		{
			// Token: 0x0400188E RID: 6286
			AnyValue,
			// Token: 0x0400188F RID: 6287
			AllValues,
			// Token: 0x04001890 RID: 6288
			TotalValue
		}
	}
}
