using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000228 RID: 552
	[JsonObject(MemberSerialization.OptIn)]
	[BindableGameEvent]
	[Serializable]
	public class ModifyOfferingQualityEvent : GrandEventPlayed
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x0002EDB6 File Offset: 0x0002CFB6
		[JsonConstructor]
		private ModifyOfferingQualityEvent()
		{
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002EDBE File Offset: 0x0002CFBE
		public ModifyOfferingQualityEvent(int playerID, string eventId) : base(playerID, eventId)
		{
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0002EDC8 File Offset: 0x0002CFC8
		public override void DeepClone(out GameEvent clone)
		{
			ModifyOfferingQualityEvent modifyOfferingQualityEvent = new ModifyOfferingQualityEvent();
			base.DeepCloneGrandEventPlayedParts(modifyOfferingQualityEvent);
			clone = modifyOfferingQualityEvent;
		}
	}
}
