using System;
using Core.StaticData;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020002B9 RID: 697
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class EventCard : GameItem
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00034C0D File Offset: 0x00032E0D
		public override GameItemCategory Category
		{
			get
			{
				return GameItemCategory.EventCard;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00034C10 File Offset: 0x00032E10
		public override bool RecordAsDeadEntityOnBanish
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00034C13 File Offset: 0x00032E13
		public override void ConfigureFrom(IdentifiableStaticData data)
		{
			base.ConfigureFrom(data);
			EventCardStaticData eventCardStaticData = data as EventCardStaticData;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00034C24 File Offset: 0x00032E24
		public sealed override void DeepClone(out GameItem gameItem)
		{
			EventCard eventCard = new EventCard();
			base.DeepCloneGameItemParts(eventCard);
			gameItem = eventCard;
		}
	}
}
