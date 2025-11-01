using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000229 RID: 553
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class UnholyCrusadeEvent : GrandEventPlayed
	{
		// Token: 0x06000ACE RID: 2766 RVA: 0x0002EDE5 File Offset: 0x0002CFE5
		[JsonConstructor]
		private UnholyCrusadeEvent()
		{
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0002EDED File Offset: 0x0002CFED
		public UnholyCrusadeEvent(int playerID, string eventId) : base(playerID, eventId)
		{
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002EDF7 File Offset: 0x0002CFF7
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (this.AffectedPlayerIds.Contains(forPlayerID))
			{
				return TurnLogEntryType.UnholyCrusadeExcommunicated;
			}
			return TurnLogEntryType.None;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002EE10 File Offset: 0x0002D010
		public override void DeepClone(out GameEvent clone)
		{
			UnholyCrusadeEvent unholyCrusadeEvent = new UnholyCrusadeEvent();
			base.DeepCloneGrandEventPlayedParts(unholyCrusadeEvent);
			clone = unholyCrusadeEvent;
		}
	}
}
