using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000537 RID: 1335
	public abstract class TurnModuleProcessor : ProcessorBase
	{
		// Token: 0x060019E7 RID: 6631 RVA: 0x0005AA32 File Offset: 0x00058C32
		public TurnModuleProcessor Configure(TurnProcessContext context, TurnModuleInstance instance)
		{
			this.TurnProcessContext = context;
			this._instance = instance;
			this.Initialize();
			return this;
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0005AA4C File Offset: 0x00058C4C
		public virtual void Invoke(TurnProcessStage ev)
		{
			TurnModuleProcessor.ProcessEvent processEvent;
			if (this._events.TryGetValue(ev, out processEvent))
			{
				processEvent();
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x0005AA6F File Offset: 0x00058C6F
		protected void SubscribeTo(TurnProcessStage ev, TurnModuleProcessor.ProcessEvent proc)
		{
			this._events[ev] = proc;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x0005AA7E File Offset: 0x00058C7E
		public virtual void Initialize()
		{
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x0005AA80 File Offset: 0x00058C80
		public virtual void OnAdded()
		{
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x0005AA82 File Offset: 0x00058C82
		public virtual void OnRemoved()
		{
		}

		// Token: 0x04000BD0 RID: 3024
		protected TurnModuleInstance _instance;

		// Token: 0x04000BD1 RID: 3025
		private Dictionary<TurnProcessStage, TurnModuleProcessor.ProcessEvent> _events = new Dictionary<TurnProcessStage, TurnModuleProcessor.ProcessEvent>();

		// Token: 0x02000A03 RID: 2563
		// (Invoke) Token: 0x06002F1F RID: 12063
		protected delegate void ProcessEvent();
	}
}
