using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200055B RID: 1371
	[Serializable]
	public class ObjectiveCondition_CastRituals : ObjectiveCondition_EventFilter<RitualCastEvent>
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001A71 RID: 6769 RVA: 0x0005C297 File Offset: 0x0005A497
		public override string LocalizationKey
		{
			get
			{
				if (this.Category != PowerType.None)
				{
					return base.LocalizationKey;
				}
				return base.LocalizationKey + ".OfAnyType";
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0005C2B9 File Offset: 0x0005A4B9
		protected override bool Filter(TurnContext context, RitualCastEvent @event, PlayerState owner, PlayerState target)
		{
			return (this.Category == PowerType.None || @event.RitualCategory == this.Category) && (!this.MustBeSuccessful || @event.Succeeded) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0005C2F1 File Offset: 0x0005A4F1
		public override int GetHashCode()
		{
			return (int)((base.GetHashCode() * 23 + this.Category) * (PowerType)29 + (this.MustBeSuccessful ? 17 : 0));
		}

		// Token: 0x04000BFA RID: 3066
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		[DefaultValue(PowerType.None)]
		public PowerType Category = PowerType.None;

		// Token: 0x04000BFB RID: 3067
		[JsonProperty]
		public bool MustBeSuccessful;
	}
}
