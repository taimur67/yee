using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001E2 RID: 482
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class GamePieceFilter : GameEntityFilter<GamePiece>
	{
	}
}
