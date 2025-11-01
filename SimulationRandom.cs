using System;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020006EC RID: 1772
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class SimulationRandom : IDeepClone<SimulationRandom>
	{
		// Token: 0x060021CB RID: 8651 RVA: 0x00076014 File Offset: 0x00074214
		public SimulationRandom() : this(Environment.TickCount)
		{
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00076021 File Offset: 0x00074221
		private SimulationRandom(SimulationRandom other)
		{
			this.SeedArray = new int[56];
			base..ctor();
			this.inext = other.inext;
			this.inextp = other.inextp;
			Array.Copy(other.SeedArray, this.SeedArray, 56);
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x00076064 File Offset: 0x00074264
		public SimulationRandom(int Seed)
		{
			this.SeedArray = new int[56];
			base..ctor();
			int num = 161803398 - ((Seed == int.MinValue) ? int.MaxValue : Math.Abs(Seed));
			this.SeedArray[55] = num;
			int num2 = 1;
			for (int i = 1; i < 55; i++)
			{
				int num3 = 21 * i % 55;
				this.SeedArray[num3] = num2;
				num2 = num - num2;
				if (num2 < 0)
				{
					num2 += int.MaxValue;
				}
				num = this.SeedArray[num3];
			}
			for (int j = 1; j < 5; j++)
			{
				for (int k = 1; k < 56; k++)
				{
					this.SeedArray[k] -= this.SeedArray[1 + (k + 30) % 55];
					if (this.SeedArray[k] < 0)
					{
						this.SeedArray[k] += int.MaxValue;
					}
				}
			}
			this.inext = 0;
			this.inextp = 21;
			Seed = 1;
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x0007615A File Offset: 0x0007435A
		public static bool RandomOneIn(int chance, TurnState playerViewOfTurnState, AIPersistentData aiPersistentData)
		{
			return chance <= 1 || aiPersistentData.SimRandom.NextDouble() <= 1.0 / (double)chance;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x0007617E File Offset: 0x0007437E
		protected virtual double Sample()
		{
			return (double)this.InternalSample() * 4.6566128752458E-10;
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00076194 File Offset: 0x00074394
		public int PeekNextSample()
		{
			int num;
			int num2;
			return this.PeekNextSample(out num, out num2);
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000761AC File Offset: 0x000743AC
		protected int PeekNextSample(out int _inext, out int _inextp)
		{
			int num = this.inext;
			int num2 = this.inextp;
			int num3;
			if ((num3 = num + 1) >= 56)
			{
				num3 = 1;
			}
			int num4;
			if ((num4 = num2 + 1) >= 56)
			{
				num4 = 1;
			}
			int num5 = this.SeedArray[num3] - this.SeedArray[num4];
			if (num5 == 2147483647)
			{
				num5--;
			}
			if (num5 < 0)
			{
				num5 += int.MaxValue;
			}
			_inext = num3;
			_inextp = num4;
			return num5;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x0007620C File Offset: 0x0007440C
		private int InternalSample()
		{
			int num = this.PeekNextSample(out this.inext, out this.inextp);
			this.SeedArray[this.inext] = num;
			return num;
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x0007623B File Offset: 0x0007443B
		public virtual int Next()
		{
			return this.InternalSample();
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x00076244 File Offset: 0x00074444
		private double GetSampleForLargeRange()
		{
			int num = this.InternalSample();
			if (this.InternalSample() % 2 == 0)
			{
				num = -num;
			}
			return ((double)num + 2147483646.0) / 4294967293.0;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x0007627C File Offset: 0x0007447C
		public virtual int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException();
			}
			long num = (long)maxValue - (long)minValue;
			if (num > 2147483647L)
			{
				return (int)((long)(this.GetSampleForLargeRange() * (double)num) + (long)minValue);
			}
			return (int)(this.Sample() * (double)num) + minValue;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000762BD File Offset: 0x000744BD
		public virtual int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (int)(this.Sample() * (double)maxValue);
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000762D3 File Offset: 0x000744D3
		public virtual double NextDouble()
		{
			return this.Sample();
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x000762DC File Offset: 0x000744DC
		public virtual void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException();
			}
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)(this.InternalSample() % 256);
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00076310 File Offset: 0x00074510
		public void DeepClone(out SimulationRandom clone)
		{
			clone = new SimulationRandom(this);
		}

		// Token: 0x04000EFF RID: 3839
		[JsonProperty]
		private int inext;

		// Token: 0x04000F00 RID: 3840
		[JsonProperty]
		private int inextp;

		// Token: 0x04000F01 RID: 3841
		[JsonProperty]
		private int[] SeedArray;
	}
}
