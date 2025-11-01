using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006DD RID: 1757
	[Serializable]
	public struct BitMask : IEquatable<BitMask>, IComparable<BitMask>
	{
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x00073333 File Offset: 0x00071533
		[JsonIgnore]
		public BitMask Inverted
		{
			get
			{
				return new BitMask(~this.Value);
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x00073341 File Offset: 0x00071541
		public BitMask(int mask)
		{
			this.Value = mask;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0007334A File Offset: 0x0007154A
		public static BitMask From<T>(T index) where T : Enum
		{
			return BitMask.From(Convert.ToInt32(index));
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x0007335C File Offset: 0x0007155C
		public static BitMask From(int index)
		{
			return new BitMask(1 << index);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x00073369 File Offset: 0x00071569
		public static BitMask From(params int[] indices)
		{
			return new BitMask(BitMaskUtils.Set(indices));
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00073376 File Offset: 0x00071576
		public static BitMask From(IEnumerable<int> indices)
		{
			return new BitMask(BitMaskUtils.Set(indices));
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00073383 File Offset: 0x00071583
		public static BitMask AllExcept(int index)
		{
			return new BitMask(BitMaskUtils.AllBut(index));
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00073390 File Offset: 0x00071590
		public static BitMask AllBut(params int[] indices)
		{
			return new BitMask(BitMaskUtils.AllBut(indices));
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x0007339D File Offset: 0x0007159D
		public static BitMask AllBut(IEnumerable<int> indices)
		{
			return new BitMask(BitMaskUtils.AllBut(indices));
		}

		// Token: 0x17000476 RID: 1142
		public bool this[int index]
		{
			get
			{
				return this.IsSet(index);
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000733BD File Offset: 0x000715BD
		public void Set(int index, bool state)
		{
			if (state)
			{
				this.Set(index);
				return;
			}
			this.Unset(index);
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000733D1 File Offset: 0x000715D1
		public void Set(int index)
		{
			this.Value |= 1 << index;
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000733E6 File Offset: 0x000715E6
		public void Unset(int index)
		{
			this.Value &= ~(1 << index);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000733FC File Offset: 0x000715FC
		public bool IsSet(int index)
		{
			return (1 << index & this.Value) != 0;
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x0007340E File Offset: 0x0007160E
		public bool NotSet(int index)
		{
			return !this.IsSet(index);
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x0007341A File Offset: 0x0007161A
		public bool IsEmpty()
		{
			return this.Value == 0;
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00073425 File Offset: 0x00071625
		public void Invert()
		{
			this.Value = ~this.Value;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00073434 File Offset: 0x00071634
		public BitMask And(in BitMask other)
		{
			return new BitMask(this.Value & other.Value);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x00073448 File Offset: 0x00071648
		public BitMask Or(in BitMask other)
		{
			return new BitMask(this.Value | other.Value);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x0007345C File Offset: 0x0007165C
		public BitMask Xor(in BitMask other)
		{
			return new BitMask(this.Value ^ other.Value);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x00073470 File Offset: 0x00071670
		public BitMask Nor(in BitMask other)
		{
			return new BitMask(~(this.Value | other.Value));
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x00073485 File Offset: 0x00071685
		public static implicit operator int(in BitMask b)
		{
			return b.Value;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0007348D File Offset: 0x0007168D
		public static BitMask operator ~(BitMask other)
		{
			return other.Inverted;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x00073496 File Offset: 0x00071696
		public static BitMask operator !(BitMask other)
		{
			return other.Inverted;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0007349F File Offset: 0x0007169F
		public static BitMask operator &(BitMask lhs, BitMask rhs)
		{
			return lhs.And(rhs);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000734AA File Offset: 0x000716AA
		public static BitMask operator |(BitMask lhs, BitMask rhs)
		{
			return lhs.Or(rhs);
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000734B5 File Offset: 0x000716B5
		public static BitMask operator ^(BitMask lhs, BitMask rhs)
		{
			return lhs.Xor(rhs);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000734C0 File Offset: 0x000716C0
		public static bool operator ==(BitMask lhs, BitMask rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000734CA File Offset: 0x000716CA
		public static bool operator !=(BitMask lhs, BitMask rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x000734D6 File Offset: 0x000716D6
		public bool Equals(BitMask other)
		{
			return this.Value == other.Value;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x000734E8 File Offset: 0x000716E8
		public override bool Equals(object obj)
		{
			if (obj is BitMask)
			{
				BitMask other = (BitMask)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x0007350D File Offset: 0x0007170D
		public int CompareTo(BitMask other)
		{
			return this.Value.CompareTo(other.Value);
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x00073520 File Offset: 0x00071720
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x04000E69 RID: 3689
		public static readonly BitMask None = new BitMask(0);

		// Token: 0x04000E6A RID: 3690
		public static readonly BitMask All = new BitMask(-1);

		// Token: 0x04000E6B RID: 3691
		public int Value;
	}
}
