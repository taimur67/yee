using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x020006B8 RID: 1720
	public class ActionPhase_MovementInput : ActionPhase
	{
		// Token: 0x06001F7C RID: 8060 RVA: 0x0006C5BA File Offset: 0x0006A7BA
		public ActionPhase_MovementInput(Action<List<HexCoord>> setMovePath, Func<GamePiece> getActor)
		{
			this.SetMovePath = setMovePath;
			this.GetActor = getActor;
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x0006C5D0 File Offset: 0x0006A7D0
		public Action<List<HexCoord>> SetMovePath { get; }

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001F7E RID: 8062 RVA: 0x0006C5D8 File Offset: 0x0006A7D8
		public Func<GamePiece> GetActor { get; }
	}
}
