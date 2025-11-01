using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020003EF RID: 1007
	[Serializable]
	public class GameItemProblem : Problem
	{
		// Token: 0x06001422 RID: 5154 RVA: 0x0004D4D8 File Offset: 0x0004B6D8
		[JsonConstructor]
		protected GameItemProblem()
		{
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0004D4E0 File Offset: 0x0004B6E0
		public GameItemProblem(Identifier gameItem)
		{
			this.GameItemId = gameItem;
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001424 RID: 5156 RVA: 0x0004D4EF File Offset: 0x0004B6EF
		public override string LocKey
		{
			get
			{
				return "Result.GameItem.DefaultProblem";
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x0004D4F6 File Offset: 0x0004B6F6
		public override string DebugString
		{
			get
			{
				return string.Format("Item {0} is invalid", this.GameItemId);
			}
		}

		// Token: 0x040008F5 RID: 2293
		[JsonProperty]
		[BindableValue(null, BindingOption.None)]
		public Identifier GameItemId;
	}
}
