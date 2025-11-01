using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001DE RID: 478
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class PlayerFilter : GameEntityFilter<PlayerState>
	{
		// Token: 0x0600096D RID: 2413 RVA: 0x0002C894 File Offset: 0x0002AA94
		protected override bool TryResolve(TurnContext context, GameEntity entity, out PlayerState player)
		{
			if (base.TryResolve(context, entity, out player))
			{
				return true;
			}
			GameItem gameItem = entity as GameItem;
			if (gameItem != null)
			{
				player = context.CurrentTurn.FindControllingPlayer(gameItem);
			}
			return player != null;
		}
	}
}
