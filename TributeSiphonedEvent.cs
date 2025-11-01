using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200026C RID: 620
	public class TributeSiphonedEvent : GameEvent
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00030C4C File Offset: 0x0002EE4C
		protected override GameEventVisibility GameEventVisibility
		{
			get
			{
				return GameEventVisibility.Private;
			}
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00030C4F File Offset: 0x0002EE4F
		public override TurnLogEntryType GetTurnLogEntryType(int forPlayerID)
		{
			return TurnLogEntryType.ChainsOfAvariceTributeReceived;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00030C56 File Offset: 0x0002EE56
		[JsonIgnore]
		private int ToPlayer
		{
			get
			{
				return this.TriggeringPlayerID;
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00030C5E File Offset: 0x0002EE5E
		[JsonConstructor]
		public TributeSiphonedEvent()
		{
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00030C71 File Offset: 0x0002EE71
		public TributeSiphonedEvent(int toPlayer, IEnumerable<ResourceNFT> resources) : base(toPlayer)
		{
			this.Resources = IEnumerableExtensions.ToList<ResourceNFT>(resources);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00030C91 File Offset: 0x0002EE91
		public TributeSiphonedEvent(int toPlayer, params ResourceNFT[] resources) : this(toPlayer, resources.AsEnumerable<ResourceNFT>())
		{
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00030CA0 File Offset: 0x0002EEA0
		public override void DeepClone(out GameEvent clone)
		{
			TributeSiphonedEvent tributeSiphonedEvent = new TributeSiphonedEvent
			{
				Resources = this.Resources.DeepClone<ResourceNFT>()
			};
			base.DeepCloneGameEventParts<TributeSiphonedEvent>(tributeSiphonedEvent);
			clone = tributeSiphonedEvent;
		}

		// Token: 0x0400053F RID: 1343
		[JsonProperty]
		public List<ResourceNFT> Resources = new List<ResourceNFT>();
	}
}
