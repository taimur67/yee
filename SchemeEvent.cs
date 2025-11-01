using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200025F RID: 607
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public abstract class SchemeEvent : GameEvent
	{
		// Token: 0x06000BE4 RID: 3044 RVA: 0x0003063E File Offset: 0x0002E83E
		protected SchemeEvent()
		{
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00030646 File Offset: 0x0002E846
		protected SchemeEvent(int playerId, SchemeObjective scheme) : base(playerId)
		{
			this.Scheme = scheme;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00030656 File Offset: 0x0002E856
		protected void DeepCloneSchemeEventParts(SchemeEvent schemeEvent)
		{
			schemeEvent.Scheme = this.Scheme.DeepClone<SchemeObjective>();
			base.DeepCloneGameEventParts<SchemeEvent>(schemeEvent);
		}

		// Token: 0x06000BE7 RID: 3047
		public abstract override void DeepClone(out GameEvent clone);

		// Token: 0x04000530 RID: 1328
		[BindableValue(null, BindingOption.None)]
		[JsonProperty]
		public SchemeObjective Scheme;
	}
}
