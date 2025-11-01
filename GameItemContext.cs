using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000382 RID: 898
	[Serializable]
	public class GameItemContext : ModifierContext
	{
		// Token: 0x06001122 RID: 4386 RVA: 0x000429DA File Offset: 0x00040BDA
		public GameItemContext()
		{
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x000429E2 File Offset: 0x00040BE2
		public GameItemContext(string sourceId)
		{
			this.SourceId = sourceId;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x000429F1 File Offset: 0x00040BF1
		protected void DeepCloneGameItemContextParts(GameItemContext gameItemContext)
		{
			gameItemContext.SourceId = this.SourceId;
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x00042A00 File Offset: 0x00040C00
		public override void DeepClone(out ModifierContext modifierContext)
		{
			GameItemContext gameItemContext = new GameItemContext();
			this.DeepCloneGameItemContextParts(gameItemContext);
			modifierContext = gameItemContext;
		}

		// Token: 0x040007EF RID: 2031
		[JsonProperty]
		public string SourceId;
	}
}
