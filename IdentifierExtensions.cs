using System;

namespace LoG
{
	// Token: 0x020002C6 RID: 710
	public static class IdentifierExtensions
	{
		// Token: 0x06000DC1 RID: 3521 RVA: 0x000367ED File Offset: 0x000349ED
		public static bool IsValid(this Identifier identifier)
		{
			return identifier != Identifier.Invalid;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000367F6 File Offset: 0x000349F6
		public static bool IsInvalid(this Identifier identifier)
		{
			return identifier == Identifier.Invalid;
		}
	}
}
