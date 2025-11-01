using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002F3 RID: 755
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class GameItemKnowledgeModifier : KnowledgeModifier
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x0003AB88 File Offset: 0x00038D88
		[JsonConstructor]
		protected GameItemKnowledgeModifier()
		{
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0003AB97 File Offset: 0x00038D97
		protected GameItemKnowledgeModifier(Identifier itemId)
		{
			this.GamePieceId = itemId;
		}

		// Token: 0x040006BE RID: 1726
		[DefaultValue(Identifier.Invalid)]
		[JsonProperty]
		public Identifier GamePieceId = Identifier.Invalid;
	}
}
