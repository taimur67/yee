using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x0200064E RID: 1614
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public struct PlayerPair : IEquatable<PlayerPair>
	{
		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001DC9 RID: 7625 RVA: 0x00066D1F File Offset: 0x00064F1F
		public int OrderedFirst
		{
			get
			{
				return Math.Min(this.First, this.Second);
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x00066D32 File Offset: 0x00064F32
		public int OrderedSecond
		{
			get
			{
				return Math.Max(this.First, this.Second);
			}
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x00066D45 File Offset: 0x00064F45
		public PlayerPair(int player1, int player2)
		{
			this.First = player1;
			this.Second = player2;
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x00066D55 File Offset: 0x00064F55
		public PlayerPair(PlayerPair other)
		{
			this.First = other.First;
			this.Second = other.Second;
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x00066D6F File Offset: 0x00064F6F
		public override int GetHashCode()
		{
			return 1 << this.First | 1 << this.Second;
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x00066D88 File Offset: 0x00064F88
		public bool AssociatedWith(int playerId)
		{
			return this.First == playerId || this.Second == playerId;
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x00066D9E File Offset: 0x00064F9E
		public bool GetOther(int playerId, out int other)
		{
			if (playerId == this.First)
			{
				other = this.Second;
				return true;
			}
			if (playerId == this.Second)
			{
				other = this.First;
				return true;
			}
			other = int.MinValue;
			return false;
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00066DCE File Offset: 0x00064FCE
		public bool PlayersAreEqual()
		{
			return this.First == this.Second;
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x00066DDE File Offset: 0x00064FDE
		public bool Equals(PlayerPair x, PlayerPair y)
		{
			return x.OrderedFirst == y.OrderedFirst && x.OrderedSecond == y.OrderedSecond;
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00066E02 File Offset: 0x00065002
		public int GetHashCode(PlayerPair obj)
		{
			return obj.GetHashCode();
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00066E11 File Offset: 0x00065011
		public bool Equals(PlayerPair other)
		{
			return this.OrderedFirst == other.OrderedFirst && this.OrderedSecond == other.OrderedSecond;
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x00066E34 File Offset: 0x00065034
		public override bool Equals(object obj)
		{
			if (obj is PlayerPair)
			{
				PlayerPair other = (PlayerPair)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x00066E59 File Offset: 0x00065059
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.First, this.Second);
		}

		// Token: 0x04000CBF RID: 3263
		[JsonProperty]
		public int First;

		// Token: 0x04000CC0 RID: 3264
		[JsonProperty]
		public int Second;
	}
}
