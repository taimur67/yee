using System;

namespace LoG
{
	// Token: 0x0200048E RID: 1166
	[Serializable]
	public class ForgeStratagemConflict : ActionConflict<ForgeStratagemConflict>
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x00051D99 File Offset: 0x0004FF99
		public ForgeStratagemConflict()
		{
		}

		// Token: 0x060015D7 RID: 5591 RVA: 0x00051DA1 File Offset: 0x0004FFA1
		public ForgeStratagemConflict(Identifier gamePiece)
		{
			this.GamePiece = gamePiece;
		}

		// Token: 0x060015D8 RID: 5592 RVA: 0x00051DB0 File Offset: 0x0004FFB0
		protected override bool ConflictsWith(ForgeStratagemConflict other)
		{
			return other.GamePiece == this.GamePiece;
		}

		// Token: 0x04000AFF RID: 2815
		public Identifier GamePiece;
	}
}
