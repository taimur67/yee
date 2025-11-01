using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200038B RID: 907
	[Serializable]
	public class GrandEventContext : PlayerContext
	{
		// Token: 0x06001143 RID: 4419 RVA: 0x00042C66 File Offset: 0x00040E66
		[JsonConstructor]
		public GrandEventContext()
		{
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00042C6E File Offset: 0x00040E6E
		public GrandEventContext(int playerId, string archfiendId, string eventId)
		{
			this.ArchfiendId = archfiendId;
			this.PlayerId = playerId;
			this.EventId = eventId;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00042C8B File Offset: 0x00040E8B
		public override string ToString()
		{
			return string.Format("{0} from {1}({2})", this.EventId, this.ArchfiendId, this.PlayerId);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00042CB0 File Offset: 0x00040EB0
		public override void DeepClone(out ModifierContext modifierContext)
		{
			GrandEventContext grandEventContext = new GrandEventContext
			{
				EventId = this.EventId.DeepClone()
			};
			base.DeepClonePlayerContextParts(grandEventContext);
			modifierContext = grandEventContext;
		}

		// Token: 0x040007FA RID: 2042
		[JsonProperty]
		public string EventId;
	}
}
