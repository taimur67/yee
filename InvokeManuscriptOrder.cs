using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200061D RID: 1565
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class InvokeManuscriptOrder : TargetedActionableOrder
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x00063E2F File Offset: 0x0006202F
		public override OrderTypes OrderType
		{
			get
			{
				return OrderTypes.InvokeManuscript;
			}
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x00063E33 File Offset: 0x00062033
		[JsonConstructor]
		protected InvokeManuscriptOrder()
		{
		}

		// Token: 0x04000C81 RID: 3201
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		public Identifier ManuscriptId = Identifier.Invalid;
	}
}
