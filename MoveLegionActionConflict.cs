using System;

namespace LoG
{
	// Token: 0x02000490 RID: 1168
	[Serializable]
	public class MoveLegionActionConflict : ActionConflict<MoveLegionActionConflict>
	{
		// Token: 0x060015DB RID: 5595 RVA: 0x00051DCB File Offset: 0x0004FFCB
		public MoveLegionActionConflict()
		{
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x00051DD3 File Offset: 0x0004FFD3
		public MoveLegionActionConflict(Identifier gamePiece)
		{
			this.GamePiece = gamePiece;
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x00051DE2 File Offset: 0x0004FFE2
		protected override bool ConflictsWith(MoveLegionActionConflict other)
		{
			return other.GamePiece == this.GamePiece;
		}

		// Token: 0x04000B00 RID: 2816
		public Identifier GamePiece;
	}
}
