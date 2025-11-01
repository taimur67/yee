using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000487 RID: 1159
	[Serializable]
	public class DebugActionConflict : ActionConflict<DebugActionConflict>
	{
		// Token: 0x060015C2 RID: 5570 RVA: 0x00051C5F File Offset: 0x0004FE5F
		public DebugActionConflict()
		{
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00051C67 File Offset: 0x0004FE67
		public DebugActionConflict(string conflict)
		{
			this.Conflict = conflict;
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x00051C76 File Offset: 0x0004FE76
		protected override bool ConflictsWith(DebugActionConflict other)
		{
			return this.Conflict == other.Conflict;
		}

		// Token: 0x04000AF7 RID: 2807
		[JsonProperty]
		private string Conflict;
	}
}
