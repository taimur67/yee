using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000384 RID: 900
	[Serializable]
	public class SupportContext : ModifierContext
	{
		// Token: 0x06001128 RID: 4392 RVA: 0x00042A3A File Offset: 0x00040C3A
		public SupportContext()
		{
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00042A42 File Offset: 0x00040C42
		public SupportContext(Identifier gameItemId)
		{
			this.GameItemId = gameItemId;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00042A51 File Offset: 0x00040C51
		protected void DeepCloneSupportContextParts(SupportContext gameItemContext)
		{
			gameItemContext.GameItemId = this.GameItemId;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00042A60 File Offset: 0x00040C60
		public override void DeepClone(out ModifierContext modifierContext)
		{
			SupportContext supportContext = new SupportContext();
			this.DeepCloneSupportContextParts(supportContext);
			modifierContext = supportContext;
		}

		// Token: 0x040007F1 RID: 2033
		[JsonProperty]
		public Identifier GameItemId;
	}
}
