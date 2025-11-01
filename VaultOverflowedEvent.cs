using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001FF RID: 511
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class VaultOverflowedEvent : GameEvent
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x0002D924 File Offset: 0x0002BB24
		[JsonConstructor]
		public VaultOverflowedEvent()
		{
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0002D937 File Offset: 0x0002BB37
		public VaultOverflowedEvent(int affectedPlayerId, List<ResourceNFT> lostResources) : base(-1)
		{
			base.AddAffectedPlayerId(affectedPlayerId);
			this.LostResources = lostResources;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0002D95C File Offset: 0x0002BB5C
		public override void DeepClone(out GameEvent clone)
		{
			VaultOverflowedEvent vaultOverflowedEvent = new VaultOverflowedEvent();
			vaultOverflowedEvent.LostResources = this.LostResources.DeepClone<ResourceNFT>();
			base.DeepCloneGameEventParts<VaultOverflowedEvent>(vaultOverflowedEvent);
			clone = vaultOverflowedEvent;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0002D98B File Offset: 0x0002BB8B
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			if (forPlayerID != base.AffectedPlayerID)
			{
				return TurnLogEntryType.None;
			}
			return TurnLogEntryType.VaultOverflowed;
		}

		// Token: 0x040004BC RID: 1212
		[JsonProperty]
		public List<ResourceNFT> LostResources = new List<ResourceNFT>();
	}
}
