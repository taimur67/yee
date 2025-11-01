using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200036A RID: 874
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public abstract class ModifiableTargetGroup<T> : ModifiableTargetGroup
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00041956 File Offset: 0x0003FB56
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x0004195E File Offset: 0x0003FB5E
		[JsonProperty]
		public List<T> Targets { get; protected set; } = new List<T>();

		// Token: 0x060010A7 RID: 4263 RVA: 0x00041967 File Offset: 0x0003FB67
		[JsonConstructor]
		public ModifiableTargetGroup()
		{
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0004197A File Offset: 0x0003FB7A
		public ModifiableTargetGroup(IModifier modifier) : this(modifier, Enumerable.Empty<T>())
		{
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00041988 File Offset: 0x0003FB88
		public ModifiableTargetGroup(IModifier modifier, IEnumerable<T> targets)
		{
			this.Modifiers.Add(modifier);
			this.Targets = IEnumerableExtensions.ToList<T>(targets);
		}
	}
}
