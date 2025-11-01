using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200036C RID: 876
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public sealed class GameItemTargetGroup : ModifiableTargetGroup<Identifier>
	{
		// Token: 0x060010AC RID: 4268 RVA: 0x000419C5 File Offset: 0x0003FBC5
		[JsonConstructor]
		public GameItemTargetGroup()
		{
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x000419CD File Offset: 0x0003FBCD
		public GameItemTargetGroup(IModifier modifier, params Identifier[] affectedIds) : base(modifier, affectedIds)
		{
		}
	}
}
