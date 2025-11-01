using System;
using Game.StaticData;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000372 RID: 882
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class GameEntityModifier : GameEntityModifier<GameEntity, ModifierStaticData>
	{
		// Token: 0x060010C4 RID: 4292 RVA: 0x00041B4F File Offset: 0x0003FD4F
		public GameEntityModifier(ModifierStaticData data) : base(data)
		{
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00041B58 File Offset: 0x0003FD58
		public override void InstallInto(GameEntity item, TurnState turn, bool baseAdjust = false)
		{
		}
	}
}
