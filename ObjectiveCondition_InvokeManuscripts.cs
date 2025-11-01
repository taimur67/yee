using System;

namespace LoG
{
	// Token: 0x02000593 RID: 1427
	[Serializable]
	public class ObjectiveCondition_InvokeManuscripts : ObjectiveCondition_EventFilter<ManuscriptEvent>
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06001B0C RID: 6924 RVA: 0x0005E5B0 File Offset: 0x0005C7B0
		public override string LocalizationKey
		{
			get
			{
				if (!this.AnyCategory)
				{
					return string.Format("InvokeManuscript.{0}", this.ManuscriptCategory);
				}
				return "InvokeManuscript";
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x0005E5D5 File Offset: 0x0005C7D5
		protected override bool CanSupportTargets
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x0005E5D8 File Offset: 0x0005C7D8
		protected override bool Filter(TurnContext context, ManuscriptEvent @event, PlayerState owner, PlayerState target)
		{
			return (this.ManuscriptCategory == @event.Category || this.AnyCategory) && base.Filter(context, @event, owner, target);
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0005E5FD File Offset: 0x0005C7FD
		public override int GetHashCode()
		{
			return (int)((base.GetHashCode() * 23 + this.ManuscriptCategory) * (ManuscriptCategory)29 + (this.AnyCategory ? 17 : 0));
		}

		// Token: 0x04000C47 RID: 3143
		public bool AnyCategory;

		// Token: 0x04000C48 RID: 3144
		public ManuscriptCategory ManuscriptCategory;
	}
}
