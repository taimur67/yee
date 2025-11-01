using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020001D6 RID: 470
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class HexBoard : IDeepClone<HexBoard>
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x0002AC6D File Offset: 0x00028E6D
		[JsonIgnore]
		public HexCoord Center
		{
			get
			{
				return new HexCoord((int)Math.Ceiling((double)((float)this.Rows / 2f)) - 1, (int)Math.Ceiling((double)((float)this.Columns / 2f)) - 1);
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x0002ACA0 File Offset: 0x00028EA0
		[JsonIgnore]
		public Hex CenterHex
		{
			get
			{
				return this[this.Center];
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002ACAE File Offset: 0x00028EAE
		public int ToIndex(int row, int column)
		{
			return this.WrapColumnIndex(column) + this.WrapRowIndex(row) * this.Columns;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002ACC6 File Offset: 0x00028EC6
		public int ToIndex(HexCoord coord)
		{
			return this.ToIndex(coord.row, coord.column);
		}

		// Token: 0x170001AA RID: 426
		public Hex this[int row, int column]
		{
			get
			{
				return this.Hexes[this.ToIndex(row, column)];
			}
		}

		// Token: 0x170001AB RID: 427
		public Hex this[HexCoord coord]
		{
			get
			{
				return this[coord.row, coord.column];
			}
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0002AD04 File Offset: 0x00028F04
		public HexCoord Coord(int row, int column)
		{
			HexCoord hexCoord = new HexCoord(row, column);
			return this.ToRelativeHex(hexCoord);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002AD24 File Offset: 0x00028F24
		private static List<Hex> GenerateHexList(int rows, int columns, TerrainType terrain = TerrainType.Plain)
		{
			List<Hex> list = new List<Hex>();
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					list.Add(new Hex(i, j, terrain));
				}
			}
			return list;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0002AD5E File Offset: 0x00028F5E
		[JsonConstructor]
		public HexBoard() : this(0, 0)
		{
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0002AD68 File Offset: 0x00028F68
		public HexBoard(int rows, int columns) : this(rows, columns, HexBoard.GenerateHexList(rows, columns, TerrainType.Plain))
		{
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0002AD7A File Offset: 0x00028F7A
		public HexBoard(int rows, int columns, params Hex[] hexes) : this(rows, columns, IEnumerableExtensions.ToList<Hex>(hexes))
		{
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002AD8A File Offset: 0x00028F8A
		public HexBoard(int rows, int columns, List<Hex> hexes)
		{
			this.Rows = rows;
			this.Columns = columns;
			this.Hexes = hexes;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002ADB4 File Offset: 0x00028FB4
		public void Resize(int rows, int columns)
		{
			List<Hex> list = HexBoard.GenerateHexList(rows, columns, TerrainType.Plain);
			int num = 0;
			while (num < this.Rows && num < rows)
			{
				int num2 = num * columns;
				int num3 = 0;
				while (num3 < this.Columns && num3 < columns)
				{
					Hex hex = list[num3 + num2];
					Hex hex2 = this[num, num3];
					hex.ControllingPlayerID = hex2.ControllingPlayerID;
					hex.Type = hex2.Type;
					num3++;
				}
				num++;
			}
			this.Hexes = list;
			this.Rows = rows;
			this.Columns = columns;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002AE39 File Offset: 0x00029039
		public IReadOnlyCollection<Hex> GetAllHexes()
		{
			return this.Hexes;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002AE44 File Offset: 0x00029044
		public IEnumerable<Hex> GetHexesControlledByPlayer(int playerId)
		{
			return from hex in this.Hexes
			where hex.ControllingPlayerID == playerId
			select hex;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002AE75 File Offset: 0x00029075
		public IEnumerable<HexCoord> GetHexCoordsControlledByPlayer(int playerId)
		{
			return from t in this.GetHexesControlledByPlayer(playerId)
			select t.HexCoord;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0002AEA2 File Offset: 0x000290A2
		public IEnumerable<HexCoord> GetHexCoordControlledByPlayerAround(int playerId, HexCoord coord)
		{
			foreach (HexCoord hexCoord in this.GetNeighbours(coord, false))
			{
				if (this[hexCoord].ControllingPlayerID == playerId)
				{
					yield return hexCoord;
				}
			}
			IEnumerator<HexCoord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002AEC0 File Offset: 0x000290C0
		public IEnumerable<Hex> GetNeutralHexes()
		{
			return from x in this.Hexes
			where x.ControllingPlayerID == -1
			select x;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002AEEC File Offset: 0x000290EC
		public HexBoard RandomizeTerrain(SimulationRandom rand, TerrainType[] allowedTerrainTypes = null)
		{
			if (allowedTerrainTypes == null)
			{
				allowedTerrainTypes = (TerrainType[])Enum.GetValues(typeof(TerrainType));
			}
			Enum.GetValues(typeof(TerrainType));
			foreach (Hex hex in this.Hexes)
			{
				int num = rand.Next(0, allowedTerrainTypes.Length);
				hex.Type = allowedTerrainTypes[num];
			}
			return this;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002AF74 File Offset: 0x00029174
		private int WrapRowIndex(int row)
		{
			return this.WrapIndex(row, this.Rows);
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002AF83 File Offset: 0x00029183
		private int WrapColumnIndex(int column)
		{
			return this.WrapIndex(column, this.Columns);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002AF92 File Offset: 0x00029192
		private int WrapIndex(int value, int size)
		{
			return (value % size + size) % size;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002AF9B File Offset: 0x0002919B
		public HexCoord ToRelativeHex(in HexCoord cord)
		{
			return cord.Normalized(this.Columns, this.Rows);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002AFB0 File Offset: 0x000291B0
		public HexCoord CalculateOffsetFromCenter(HexCoord center, HexCoord coord)
		{
			coord = this.ToRelativeHex(coord);
			int column = (int)Math.Ceiling((double)((float)this.Columns / 2f));
			int row = (int)Math.Ceiling((double)((float)this.Rows / 2f));
			HexCoord hexCoord = new HexCoord(row, column);
			HexCoord hexCoord2 = center + coord;
			HexCoord hexCoord3 = hexCoord2 + hexCoord;
			HexCoord hexCoord4 = this.ToRelativeHex(hexCoord3);
			return hexCoord4 - hexCoord;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002B024 File Offset: 0x00029224
		public bool TryGetGeneralDirection(HexCoord from, HexCoord to, out Vector2 direction)
		{
			direction = Vector2.Zero;
			CubeCoord cubeCoord = this.Diff(from, to);
			HexCoord hexCoord = (HexCoord)cubeCoord;
			if (hexCoord.column == 0 && hexCoord.row == 0)
			{
				return false;
			}
			direction = Vector2.Normalize(new Vector2((float)hexCoord.column, (float)hexCoord.row));
			return true;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002B07E File Offset: 0x0002927E
		public IEnumerable<HexCoord> ToRelativePath(params HexCoord[] coords)
		{
			return this.ToRelativePath(coords);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0002B087 File Offset: 0x00029287
		public IEnumerable<HexCoord> ToRelativePath(IEnumerable<HexCoord> coords)
		{
			foreach (HexCoord hexCoord in coords)
			{
				yield return this.ToRelativeHex(hexCoord);
			}
			IEnumerator<HexCoord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002B09E File Offset: 0x0002929E
		public bool AreConsecutivelyAdjacent(HexCoord start, List<HexCoord> coords)
		{
			return this.AreAdjacent(start, IEnumerableExtensions.First<HexCoord>(coords)) && this.AreConsecutivelyAdjacent(coords);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002B0C0 File Offset: 0x000292C0
		public bool AreConsecutivelyAdjacent(List<HexCoord> coords)
		{
			for (int i = 1; i < coords.Count; i++)
			{
				if (!this.AreAdjacent(coords[i - 1], coords[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002B0FC File Offset: 0x000292FC
		public static CubeCoord GetOffset(HexDirection direction)
		{
			switch (direction)
			{
			case HexDirection.Up:
				return CubeOffset.Up;
			case HexDirection.UpRight:
				return CubeOffset.UpRight;
			case HexDirection.DownRight:
				return CubeOffset.DownRight;
			case HexDirection.Down:
				return CubeOffset.Down;
			case HexDirection.DownLeft:
				return CubeOffset.DownLeft;
			case HexDirection.UpLeft:
				return CubeOffset.UpLeft;
			default:
				return CubeOffset.None;
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002B154 File Offset: 0x00029354
		public static HexCoord GetOffsetOnOddColumn(HexDirection direction)
		{
			HexCoord result;
			switch (direction)
			{
			case HexDirection.Up:
				result = HexOffset.Up;
				break;
			case HexDirection.UpRight:
				result = HexOffset.UpRightOdd;
				break;
			case HexDirection.DownRight:
				result = HexOffset.DownRightOdd;
				break;
			case HexDirection.Down:
				result = HexOffset.Down;
				break;
			case HexDirection.DownLeft:
				result = HexOffset.DownLeftOdd;
				break;
			case HexDirection.UpLeft:
				result = HexOffset.UpLeftOdd;
				break;
			default:
				result = HexOffset.None;
				break;
			}
			return result;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002B1B8 File Offset: 0x000293B8
		public static HexCoord GetOffsetOnEvenColumn(HexDirection direction)
		{
			HexCoord result;
			switch (direction)
			{
			case HexDirection.Up:
				result = HexOffset.Up;
				break;
			case HexDirection.UpRight:
				result = HexOffset.UpRightEven;
				break;
			case HexDirection.DownRight:
				result = HexOffset.DownRightEven;
				break;
			case HexDirection.Down:
				result = HexOffset.Down;
				break;
			case HexDirection.DownLeft:
				result = HexOffset.DownLeftEven;
				break;
			case HexDirection.UpLeft:
				result = HexOffset.UpLeftEven;
				break;
			default:
				result = HexOffset.None;
				break;
			}
			return result;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002B21C File Offset: 0x0002941C
		public HexCoord GetNeighbour(HexCoord pos, HexDirection dir)
		{
			CubeCoord neighbor = HexBoard.GetNeighbor((CubeCoord)pos, dir);
			HexCoord hexCoord = (HexCoord)neighbor;
			return this.ToRelativeHex(hexCoord);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002B248 File Offset: 0x00029448
		public static HexCoord GetNonRelativeNeighbour(HexCoord pos, HexDirection dir)
		{
			CubeCoord neighbor = HexBoard.GetNeighbor((CubeCoord)pos, dir);
			return (HexCoord)neighbor;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0002B26C File Offset: 0x0002946C
		public static CubeCoord GetNeighbor(CubeCoord pos, HexDirection direction)
		{
			CubeCoord offset = HexBoard.GetOffset(direction);
			return pos + offset;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0002B289 File Offset: 0x00029489
		public IEnumerable<HexCoord> ResolvePathFrom(HexCoord start, params HexDirection[] directions)
		{
			foreach (HexDirection dir in directions)
			{
				start = this.GetNeighbour(start, dir);
				yield return start;
			}
			HexDirection[] array = null;
			yield break;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002B2A7 File Offset: 0x000294A7
		public IEnumerable<HexCoord> ResolvePathIncluding(HexCoord start, params HexDirection[] directions)
		{
			yield return start;
			foreach (HexCoord hexCoord in this.ResolvePathFrom(start, directions))
			{
				yield return hexCoord;
			}
			IEnumerator<HexCoord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0002B2C5 File Offset: 0x000294C5
		public IEnumerable<HexCoord> EnumerateNeighbours(HexCoord coord)
		{
			if (coord.column % 2 == 0)
			{
				HexCoord hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.Up);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.UpRight);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.DownRight);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.Down);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.DownLeft);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.UpLeft);
				yield return coord + hexCoord;
			}
			else
			{
				HexCoord hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.Up);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.UpRight);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.DownRight);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.Down);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.DownLeft);
				yield return coord + hexCoord;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.UpLeft);
				yield return coord + hexCoord;
			}
			yield break;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002B2D5 File Offset: 0x000294D5
		public IEnumerable<HexCoord> EnumerateNeighboursNormalized(HexCoord coord)
		{
			foreach (HexCoord hexCoord in this.EnumerateNeighbours(coord))
			{
				yield return this.ToRelativeHex(hexCoord);
			}
			IEnumerator<HexCoord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002B2EC File Offset: 0x000294EC
		public bool TryGetNeighbours(HexCoord coord, HexCoord[] neighbours)
		{
			if (neighbours == null)
			{
				SimLogger logger = SimLogger.Logger;
				if (logger != null)
				{
					logger.Error("Neighbours array is null");
				}
				return false;
			}
			if (neighbours.Length != 6)
			{
				SimLogger logger2 = SimLogger.Logger;
				if (logger2 != null)
				{
					logger2.Error("Neighbours array must have 6 elements");
				}
				return false;
			}
			if (coord.column % 2 == 0)
			{
				int num = 0;
				HexCoord hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.Up);
				neighbours[num] = coord + hexCoord;
				int num2 = 1;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.UpRight);
				neighbours[num2] = coord + hexCoord;
				int num3 = 2;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.DownRight);
				neighbours[num3] = coord + hexCoord;
				int num4 = 3;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.Down);
				neighbours[num4] = coord + hexCoord;
				int num5 = 4;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.DownLeft);
				neighbours[num5] = coord + hexCoord;
				int num6 = 5;
				hexCoord = HexBoard.GetOffsetOnEvenColumn(HexDirection.UpLeft);
				neighbours[num6] = coord + hexCoord;
			}
			else
			{
				int num7 = 0;
				HexCoord hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.Up);
				neighbours[num7] = coord + hexCoord;
				int num8 = 1;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.UpRight);
				neighbours[num8] = coord + hexCoord;
				int num9 = 2;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.DownRight);
				neighbours[num9] = coord + hexCoord;
				int num10 = 3;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.Down);
				neighbours[num10] = coord + hexCoord;
				int num11 = 4;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.DownLeft);
				neighbours[num11] = coord + hexCoord;
				int num12 = 5;
				hexCoord = HexBoard.GetOffsetOnOddColumn(HexDirection.UpLeft);
				neighbours[num12] = coord + hexCoord;
			}
			return true;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0002B458 File Offset: 0x00029658
		public bool TryGetNeighboursNormalized(HexCoord coord, HexCoord[] neighbours)
		{
			if (this.TryGetNeighbours(coord, neighbours))
			{
				for (int i = 0; i < neighbours.Length; i++)
				{
					neighbours[i] = this.ToRelativeHex(neighbours[i]);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002B494 File Offset: 0x00029694
		public IEnumerable<CubeCoord> EnumerateNeighbours(CubeCoord centre)
		{
			CubeCoord offset = HexBoard.GetOffset(HexDirection.Up);
			yield return centre + offset;
			offset = HexBoard.GetOffset(HexDirection.UpRight);
			yield return centre + offset;
			offset = HexBoard.GetOffset(HexDirection.DownRight);
			yield return centre + offset;
			offset = HexBoard.GetOffset(HexDirection.Down);
			yield return centre + offset;
			offset = HexBoard.GetOffset(HexDirection.DownLeft);
			yield return centre + offset;
			offset = HexBoard.GetOffset(HexDirection.UpLeft);
			yield return centre + offset;
			yield break;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002B4A4 File Offset: 0x000296A4
		public IEnumerable<HexCoord> EnumerateRangeNormalized(HexCoord coord, int range)
		{
			return from t in this.EnumerateRange(coord, range)
			select this.ToRelativeHex(t);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0002B4BF File Offset: 0x000296BF
		public IEnumerable<HexCoord> EnumerateRange(HexCoord coord, int range)
		{
			return from x in this.EnumerateRange((CubeCoord)coord, range)
			select (HexCoord)x;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0002B4F3 File Offset: 0x000296F3
		public IEnumerable<CubeCoord> EnumerateRange(CubeCoord coord, int range)
		{
			int num3;
			for (int q = -range; q <= range; q = num3 + 1)
			{
				int num = Math.Max(-range, -q - range);
				int b = Math.Min(range, -q + range);
				for (int r = num; r <= b; r = num3 + 1)
				{
					int num2 = -q - r;
					CubeCoord cubeCoord = new CubeCoord((float)q, (float)r, (float)num2);
					yield return coord + cubeCoord;
					num3 = r;
				}
				num3 = q;
			}
			yield break;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002B50A File Offset: 0x0002970A
		public bool AreAdjacent(HexCoord a, HexCoord b)
		{
			return this.ShortestDistance(a, b) == 1;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002B518 File Offset: 0x00029718
		public bool AreAdjacent(HexCoord a, HexCoord b, out HexDirection? direction)
		{
			a = this.ToRelativeHex(a);
			b = this.ToRelativeHex(b);
			int num = ArrayExtensions.IndexOf<HexCoord>(IEnumerableExtensions.ToArray<HexCoord>(this.GetNeighbours(a, false)), b);
			if (num < 0)
			{
				direction = null;
				return false;
			}
			direction = new HexDirection?((HexDirection)num);
			return true;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0002B568 File Offset: 0x00029768
		public HexCoord CalculateNearestWorldspaceHexOf(HexCoord focusCoord, HexCoord instanceCoord)
		{
			instanceCoord = this.ToRelativeHex(instanceCoord);
			HexCoord hexCoord = this.ToRelativeHex(focusCoord);
			HexCoord hexCoord2 = this.Center;
			HexCoord hexCoord3 = hexCoord2 - hexCoord;
			hexCoord2 = instanceCoord + hexCoord3;
			HexCoord hexCoord4 = this.ToRelativeHex(hexCoord2);
			hexCoord2 = this.Center;
			HexCoord hexCoord5 = hexCoord4 - hexCoord2;
			return focusCoord + hexCoord5;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002B5C9 File Offset: 0x000297C9
		public bool AreAdjacent(CubeCoord lhs, CubeCoord rhs)
		{
			return this.AreAdjacent((HexCoord)lhs, (HexCoord)rhs);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002B5E0 File Offset: 0x000297E0
		public bool AreEqual(HexCoord lhs, HexCoord rhs)
		{
			HexCoord hexCoord = this.ToRelativeHex(lhs);
			HexCoord hexCoord2 = this.ToRelativeHex(rhs);
			return hexCoord == hexCoord2;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0002B608 File Offset: 0x00029808
		public IEnumerable<HexCoord> GetNeighbours(HexCoord centre, bool includeCentre = false)
		{
			if (includeCentre)
			{
				yield return centre;
			}
			foreach (HexCoord hexCoord in this.EnumerateNeighboursNormalized(centre))
			{
				yield return hexCoord;
			}
			IEnumerator<HexCoord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0002B628 File Offset: 0x00029828
		public IEnumerable<HexCoord> GetNeighboursForStagingAttack(GamePiece attacker, GamePiece target)
		{
			return this.GetNeighbours(target.Location, false).Where(delegate(HexCoord h)
			{
				Hex hex = this[h];
				int num = (hex != null) ? hex.ControllingPlayerID : int.MinValue;
				return num == -1 || num == target.ControllingPlayerId || num == attacker.ControllingPlayerId;
			});
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0002B674 File Offset: 0x00029874
		public void SetOwnership(HexCoord hexCoord, int playerID)
		{
			HexCoord coord = this.ToRelativeHex(hexCoord);
			this[coord].SetControllingPlayerID(playerID);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002B698 File Offset: 0x00029898
		public int GetOwnership(HexCoord hexCoord)
		{
			HexCoord coord = this.ToRelativeHex(hexCoord);
			return this[coord].GetControllingPlayerID();
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002B6BA File Offset: 0x000298BA
		public bool IsValid()
		{
			return this.Columns > 0 && this.Rows > 0;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002B6D0 File Offset: 0x000298D0
		public int ShortestDistance(HexCoord a, HexCoord b)
		{
			return this.Diff(a, b).Length;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002B6F0 File Offset: 0x000298F0
		public float ShortestDistancef(HexCoord a, HexCoord b)
		{
			return this.Diff(a, b).Magnitude;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002B710 File Offset: 0x00029910
		public CubeCoord Diff(HexCoord a, HexCoord b)
		{
			a = this.ToRelativeHex(a);
			b = this.CalculateNearestWorldspaceHexOf(a, b);
			CubeCoord cubeCoord = HexUtils.ToCubeCoord(a);
			CubeCoord cubeCoord2 = HexUtils.ToCubeCoord(b);
			return cubeCoord - cubeCoord2;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002B74A File Offset: 0x0002994A
		public IEnumerable<HexCoord> GetBorderCantons(PlayerState player, bool uniqueOnly = true)
		{
			return this.GetBorderCantons(player.Id, uniqueOnly);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002B75C File Offset: 0x0002995C
		public IEnumerable<HexCoord> GetBorderCantons(int player, bool uniqueOnly = true)
		{
			IEnumerable<HexCoord> ownedCoords = from t in this.GetHexesControlledByPlayer(player)
			select t.HexCoord;
			return this.GetBorderCantons(player, ownedCoords, uniqueOnly);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002B79E File Offset: 0x0002999E
		public IEnumerable<HexCoord> GetBorderCantons(int player, IEnumerable<HexCoord> ownedCoords, bool uniqueOnly = true)
		{
			HashSet<HexCoord> visited = uniqueOnly ? new HashSet<HexCoord>() : null;
			Func<HexCoord, bool> <>9__0;
			foreach (HexCoord centre in ownedCoords)
			{
				IEnumerable<HexCoord> neighbours = this.GetNeighbours(centre, false);
				Func<HexCoord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((HexCoord t) => this.GetOwnership(t) != player));
				}
				IEnumerable<HexCoord> enumerable = neighbours.Where(predicate);
				foreach (HexCoord hexCoord in enumerable)
				{
					if (!uniqueOnly || visited.Add(hexCoord))
					{
						yield return hexCoord;
					}
				}
				IEnumerator<HexCoord> enumerator2 = null;
			}
			IEnumerator<HexCoord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002B7C4 File Offset: 0x000299C4
		public void GetBorderCantons(int playerId, IEnumerable<HexCoord> ownedCantons, ref HashSet<HexCoord> internalBorder, ref HashSet<HexCoord> externalBorder)
		{
			Func<HexCoord, bool> <>9__0;
			foreach (HexCoord hexCoord in ownedCantons)
			{
				IEnumerable<HexCoord> neighbours = this.GetNeighbours(hexCoord, false);
				Func<HexCoord, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((HexCoord t) => this.GetOwnership(t) != playerId));
				}
				IEnumerable<HexCoord> enumerable = neighbours.Where(predicate);
				int count = externalBorder.Count;
				CollectionExtensions.AddRange<HexCoord>(externalBorder, enumerable);
				if (externalBorder.Count != count)
				{
					internalBorder.Add(hexCoord);
				}
			}
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002B870 File Offset: 0x00029A70
		public bool ArePlayersNeighbours(int player1ID, int player2ID)
		{
			return this.GetBorderCantons(player1ID, true).Any((HexCoord t) => this.GetOwnership(t) == player2ID);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0002B8AC File Offset: 0x00029AAC
		public bool CantonBordersPlayersRealm(int player, HexCoord coord, bool excludeImpassible = false)
		{
			return this.GetOwnership(coord) != player && this.GetNeighbours(coord, false).Any(delegate(HexCoord t)
			{
				Hex hex = this[this.ToRelativeHex(t)];
				return (!excludeImpassible || hex.Type.IsPassible()) && hex.GetControllingPlayerID() == player;
			});
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002B900 File Offset: 0x00029B00
		public bool IsBorderCanton(HexCoord coord)
		{
			int owner = this.GetOwnership(coord);
			return this.GetNeighbours(coord, false).Any((HexCoord t) => owner != this.GetOwnership(t));
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0002B940 File Offset: 0x00029B40
		public int CalculateEnclaveCount(int playerID)
		{
			return this.CalculateEnclaves(this.GetHexCoordsControlledByPlayer(playerID).ToHashSet<HexCoord>()).Count<HashSet<HexCoord>>();
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0002B959 File Offset: 0x00029B59
		public int CalculateEnclaveCount(IEnumerable<Hex> ownedCantons)
		{
			return this.CalculateEnclaveCount((from t in ownedCantons
			select t.HexCoord).ToHashSet<HexCoord>());
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0002B98B File Offset: 0x00029B8B
		public int CalculateEnclaveCount(HashSet<HexCoord> ownedCantons)
		{
			return this.CalculateEnclaves(ownedCantons).Count<HashSet<HexCoord>>();
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002B999 File Offset: 0x00029B99
		public IEnumerable<HashSet<HexCoord>> CalculateEnclaves(int playerId)
		{
			return this.CalculateEnclaves(this.GetHexCoordsControlledByPlayer(playerId));
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0002B9A8 File Offset: 0x00029BA8
		public IEnumerable<HashSet<HexCoord>> CalculateEnclaves(IEnumerable<HexCoord> ownedCantons)
		{
			HashSet<HexCoord> cantons = ownedCantons.ToHashSet<HexCoord>();
			while (cantons.Count > 0)
			{
				HexCoord start = IEnumerableExtensions.First<HexCoord>(cantons);
				HashSet<HexCoord> enclave = this.CalculateEnclave(start, cantons);
				yield return enclave;
				CollectionExtensions.Remove<HexCoord>(cantons, enclave);
				enclave = null;
			}
			yield break;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0002B9C0 File Offset: 0x00029BC0
		private HashSet<HexCoord> CalculateEnclave(HexCoord start, HashSet<HexCoord> ownedCantons)
		{
			HashSet<HexCoord> hashSet = new HashSet<HexCoord>();
			HashSet<HexCoord> perimeter = IEnumerableExtensions.ToEnumerable<HexCoord>(start).ToHashSet<HexCoord>();
			this.CalculateEnclave(hashSet, perimeter, ownedCantons);
			return hashSet;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0002B9EC File Offset: 0x00029BEC
		private void CalculateEnclave(HashSet<HexCoord> enclave, HashSet<HexCoord> perimeter, HashSet<HexCoord> ownedCantons)
		{
			CollectionExtensions.AddRange<HexCoord>(enclave, perimeter);
			HashSet<HexCoord> hashSet = (from t in perimeter.SelectMany((HexCoord t) => this.EnumerateNeighboursNormalized(t)).Where(new Func<HexCoord, bool>(ownedCantons.Contains))
			where !enclave.Contains(t)
			select t).ToHashSet<HexCoord>();
			if (hashSet.Count == 0)
			{
				return;
			}
			this.CalculateEnclave(enclave, hashSet, ownedCantons);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002BA6C File Offset: 0x00029C6C
		public void DeepClone(out HexBoard clone)
		{
			using (SimProfilerBlock.ProfilerBlock("HexBoard.DeepClone"))
			{
				List<Hex> hexes = this.Hexes.DeepClone<Hex>();
				clone = new HexBoard(this.Rows, this.Columns, hexes);
			}
		}

		// Token: 0x04000475 RID: 1141
		public static readonly HexDirection[] AllHexDirections = (HexDirection[])Enum.GetValues(typeof(HexDirection));

		// Token: 0x04000476 RID: 1142
		[JsonProperty]
		public int Columns;

		// Token: 0x04000477 RID: 1143
		[JsonProperty]
		public int Rows;

		// Token: 0x04000478 RID: 1144
		[JsonProperty]
		public List<Hex> Hexes = new List<Hex>();
	}
}
