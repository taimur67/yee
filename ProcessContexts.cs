using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000508 RID: 1288
	public class ProcessContexts
	{
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x000587C9 File Offset: 0x000569C9
		// (set) Token: 0x060018C1 RID: 6337 RVA: 0x000587D1 File Offset: 0x000569D1
		public Dictionary<int, List<ResourceNFT>> SiphonedTribute { get; private set; } = new Dictionary<int, List<ResourceNFT>>();

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x000587DA File Offset: 0x000569DA
		// (set) Token: 0x060018C3 RID: 6339 RVA: 0x000587E2 File Offset: 0x000569E2
		public BidContext BiddingContext { get; private set; } = new BidContext();

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x000587EB File Offset: 0x000569EB
		// (set) Token: 0x060018C5 RID: 6341 RVA: 0x000587F3 File Offset: 0x000569F3
		public TributeContext TributeContext { get; private set; } = new TributeContext();

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x000587FC File Offset: 0x000569FC
		// (set) Token: 0x060018C7 RID: 6343 RVA: 0x00058804 File Offset: 0x00056A04
		public DiplomaticContext DiplomaticContext { get; private set; } = new DiplomaticContext();

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x0005880D File Offset: 0x00056A0D
		// (set) Token: 0x060018C9 RID: 6345 RVA: 0x00058815 File Offset: 0x00056A15
		public GrandEventsContext EventsContext { get; private set; } = new GrandEventsContext();

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x0005881E File Offset: 0x00056A1E
		// (set) Token: 0x060018CB RID: 6347 RVA: 0x00058826 File Offset: 0x00056A26
		public EventDrawContext EventDrawContext { get; private set; } = new EventDrawContext();

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x0005882F File Offset: 0x00056A2F
		// (set) Token: 0x060018CD RID: 6349 RVA: 0x00058837 File Offset: 0x00056A37
		public List<ValueTuple<PlayerState, ActiveRitual>> RitualsToEnd { get; private set; } = new List<ValueTuple<PlayerState, ActiveRitual>>();
	}
}
