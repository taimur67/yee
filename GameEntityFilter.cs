using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001DC RID: 476
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class GameEntityFilter
	{
		// Token: 0x06000967 RID: 2407
		public abstract bool Filter(TurnContext context, GameEntity entity);
	}
}
