using System;
using System.Collections.Generic;

namespace LoG
{
	// Token: 0x02000292 RID: 658
	public class HexIsland
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x000342B8 File Offset: 0x000324B8
		public int Size
		{
			get
			{
				return this.Hexes.Count;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x000342C5 File Offset: 0x000324C5
		public virtual bool SuccessfullyCompleted
		{
			get
			{
				return this.Size >= this.TerrainTypeData.PatchMinSize;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x000342DD File Offset: 0x000324DD
		public virtual bool CanExpandFurther
		{
			get
			{
				return this.Size < this.TerrainTypeData.PatchMaxSize && this.DeadEnds.Count != this.Size;
			}
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0003430A File Offset: 0x0003250A
		public void Add(HexCoord coord)
		{
			if (this.Hexes.Contains(coord))
			{
				return;
			}
			this.Hexes.Add(coord);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00034328 File Offset: 0x00032528
		public void AddRange(IEnumerable<HexCoord> coords)
		{
			foreach (HexCoord coord in coords)
			{
				this.Add(coord);
			}
		}

		// Token: 0x040005B8 RID: 1464
		public readonly List<HexCoord> Hexes = new List<HexCoord>();

		// Token: 0x040005B9 RID: 1465
		public readonly List<HexCoord> DeadEnds = new List<HexCoord>();

		// Token: 0x040005BA RID: 1466
		public TerrainStaticData TerrainTypeData;
	}
}
