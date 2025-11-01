using System;

namespace LoG
{
	// Token: 0x020006B9 RID: 1721
	public class ActionPhase_MessageBox : ActionPhase
	{
		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001F7F RID: 8063 RVA: 0x0006C5E0 File Offset: 0x0006A7E0
		// (set) Token: 0x06001F80 RID: 8064 RVA: 0x0006C5E8 File Offset: 0x0006A7E8
		public ActionMessageType MessageType { get; private set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x0006C5F1 File Offset: 0x0006A7F1
		// (set) Token: 0x06001F82 RID: 8066 RVA: 0x0006C5F9 File Offset: 0x0006A7F9
		public Action OnProceed { get; private set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x0006C602 File Offset: 0x0006A802
		// (set) Token: 0x06001F84 RID: 8068 RVA: 0x0006C60A File Offset: 0x0006A80A
		public Action OnCancel { get; private set; }

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x0006C613 File Offset: 0x0006A813
		// (set) Token: 0x06001F86 RID: 8070 RVA: 0x0006C61B File Offset: 0x0006A81B
		public Func<TurnContext, bool> ShowCondition { get; set; }

		// Token: 0x06001F87 RID: 8071 RVA: 0x0006C624 File Offset: 0x0006A824
		public ActionPhase_MessageBox(ActionMessageType messageType, Action onProceed = null, Action onCancel = null)
		{
			this.MessageType = messageType;
			this.OnProceed = onProceed;
			this.OnCancel = onCancel;
		}
	}
}
