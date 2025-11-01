using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200036B RID: 875
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public sealed class PlayerTargetGroup : ModifiableTargetGroup<int>
	{
		// Token: 0x060010AA RID: 4266 RVA: 0x000419B3 File Offset: 0x0003FBB3
		[JsonConstructor]
		public PlayerTargetGroup()
		{
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x000419BB File Offset: 0x0003FBBB
		public PlayerTargetGroup(IModifier modifier, params int[] affectedIds) : base(modifier, affectedIds)
		{
		}
	}
}
