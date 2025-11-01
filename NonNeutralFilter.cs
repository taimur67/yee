using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001E4 RID: 484
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class NonNeutralFilter : GameEntityFilter
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x0002C994 File Offset: 0x0002AB94
		public override bool Filter(TurnContext context, GameEntity entity)
		{
			PlayerState playerState = entity as PlayerState;
			if (playerState == null)
			{
				GameItem gameItem = entity as GameItem;
				if (gameItem != null)
				{
					playerState = context.CurrentTurn.FindControllingPlayer(gameItem);
				}
			}
			return playerState != null && playerState.Id != -1 && playerState.Id != int.MinValue;
		}
	}
}
