using System;

namespace LoG
{
	// Token: 0x020002C4 RID: 708
	public interface IIdentifiable : IEquatable<IIdentifiable>
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000DC0 RID: 3520
		Identifier Id { get; }
	}
}
