using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000387 RID: 903
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class DiplomaticStateContext : PlayerContext
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x00042B21 File Offset: 0x00040D21
		[JsonIgnore]
		public int ActorId
		{
			get
			{
				return this.PlayerId;
			}
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00042B29 File Offset: 0x00040D29
		[JsonConstructor]
		public DiplomaticStateContext()
		{
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00042B31 File Offset: 0x00040D31
		public DiplomaticStateContext(DiplomaticStateValue state, int actorId, int targetId) : base(actorId, "")
		{
			this.TargetId = targetId;
			this.State = state;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00042B50 File Offset: 0x00040D50
		public override void DeepClone(out ModifierContext modifierContext)
		{
			DiplomaticStateContext diplomaticStateContext = new DiplomaticStateContext
			{
				State = this.State,
				TargetId = this.TargetId
			};
			base.DeepClonePlayerContextParts(diplomaticStateContext);
			modifierContext = diplomaticStateContext;
		}

		// Token: 0x040007F5 RID: 2037
		[JsonProperty]
		public DiplomaticStateValue State;

		// Token: 0x040007F6 RID: 2038
		[JsonProperty]
		public int TargetId;
	}
}
