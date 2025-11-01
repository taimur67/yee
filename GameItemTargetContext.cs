using System;

namespace LoG
{
	// Token: 0x020002BC RID: 700
	public struct GameItemTargetContext
	{
		// Token: 0x06000D53 RID: 3411 RVA: 0x00034EC8 File Offset: 0x000330C8
		public static implicit operator GameItem(GameItemTargetContext ctx)
		{
			return ctx.GameItem;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00034ED0 File Offset: 0x000330D0
		public static explicit operator GameItemTargetContext(GameItem item)
		{
			return new GameItemTargetContext(item, Result.Success);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00034EDD File Offset: 0x000330DD
		public GameItemTargetContext(GameItem gameItem, Result result)
		{
			this.GameItem = gameItem;
			this.Result = result;
		}

		// Token: 0x040005E0 RID: 1504
		public readonly GameItem GameItem;

		// Token: 0x040005E1 RID: 1505
		public readonly Result Result;
	}
}
