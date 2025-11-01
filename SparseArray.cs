using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LoG.Simulation.Extensions;

namespace LoG
{
	// Token: 0x020006EF RID: 1775
	[Serializable]
	public class SparseArray<T> : IEnumerable<!0>, IEnumerable, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		// Token: 0x060021EE RID: 8686 RVA: 0x000766A9 File Offset: 0x000748A9
		public SparseArray()
		{
			this._data = new T[8];
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000766BD File Offset: 0x000748BD
		public SparseArray(int size)
		{
			if (size < 8)
			{
				size = 8;
			}
			else if (!this.IsPowerOfTwo(size))
			{
				this.NextPowerOf2(size);
			}
			this._data = new T[size];
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000766EB File Offset: 0x000748EB
		public SparseArray(IEnumerable<T> data)
		{
			this._data = IEnumerableExtensions.ToArray<T>(data);
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x000766FF File Offset: 0x000748FF
		public SparseArray(params T[] data)
		{
			this._data = IEnumerableExtensions.ToArray<T>(data);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00076713 File Offset: 0x00074913
		public void CopyTo(Array array, int index)
		{
			Array.Copy(this._data, index, array, 0, this._data.Length);
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x060021F3 RID: 8691 RVA: 0x0007672B File Offset: 0x0007492B
		public int Count
		{
			get
			{
				return this._data.Length;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x00076735 File Offset: 0x00074935
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x060021F5 RID: 8693 RVA: 0x00076738 File Offset: 0x00074938
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x1700047B RID: 1147
		public T this[int index]
		{
			get
			{
				return this.ElementAt(index);
			}
			set
			{
				this.Insert(index, value);
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0007676D File Offset: 0x0007496D
		public void Resize(int size)
		{
			if (size - this.Count > 0 && !this.IsPowerOfTwo(size))
			{
				size = this.NextPowerOf2(size);
			}
			if (size == this.Count)
			{
				return;
			}
			this.ForceSize(size);
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x0007679D File Offset: 0x0007499D
		public void ForceSize(int size)
		{
			Array.Resize<T>(ref this._data, size);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000767AB File Offset: 0x000749AB
		public IEnumerator<T> GetEnumerator()
		{
			return this._data.ExcludeNull<T>().GetEnumerator();
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000767BD File Offset: 0x000749BD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._data.ExcludeNull<T>().GetEnumerator();
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000767D0 File Offset: 0x000749D0
		public T ElementAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				return default(T);
			}
			return this._data[index];
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00076800 File Offset: 0x00074A00
		public void Insert(int index, T value)
		{
			if (index >= this.Count)
			{
				this.Resize(index + 1);
			}
			this._data[index] = value;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00076821 File Offset: 0x00074A21
		public bool TryGetValue(int index, out T value)
		{
			value = this.ElementAt(index);
			return value != null;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x0007683E File Offset: 0x00074A3E
		private bool IsPowerOfTwo(int value)
		{
			return (value & value - 1) == 0;
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00076848 File Offset: 0x00074A48
		private int NextPowerOf2(int v)
		{
			return (int)this.NextPowerOf2((uint)v);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00076851 File Offset: 0x00074A51
		private uint NextPowerOf2(uint v)
		{
			v -= 1U;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			v += 1U;
			return v;
		}

		// Token: 0x04000F04 RID: 3844
		private T[] _data;

		// Token: 0x04000F05 RID: 3845
		private const int MinimumSize = 8;

		// Token: 0x04000F06 RID: 3846
		[NonSerialized]
		private object _syncRoot;
	}
}
