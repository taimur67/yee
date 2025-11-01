using System;

namespace LoG
{
	// Token: 0x02000304 RID: 772
	public class SimLogger
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0003BF70 File Offset: 0x0003A170
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0003BF77 File Offset: 0x0003A177
		public static SimLogger Logger { get; private set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0003BF7F File Offset: 0x0003A17F
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0003BF86 File Offset: 0x0003A186
		private static SimLogger LoggerInstance { get; set; }

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003BF8E File Offset: 0x0003A18E
		public static void CreateLogger(SimLogger.SimLogDelegate simLogDelegate)
		{
			if (SimLogger.LoggerInstance == null)
			{
				SimLogger.LoggerInstance = new SimLogger(simLogDelegate);
			}
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0003BFA4 File Offset: 0x0003A1A4
		public static void EnableLogger()
		{
			SimLogger.Logger = SimLogger.LoggerInstance;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0003BFB0 File Offset: 0x0003A1B0
		public static void DisableLogger()
		{
			SimLogger.Logger = null;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0003BFB8 File Offset: 0x0003A1B8
		private SimLogger(SimLogger.SimLogDelegate simLogDelegate)
		{
			this.SimLogFunction = (simLogDelegate ?? new SimLogger.SimLogDelegate(SimLogger.FallbackLogFunction));
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0003BFD7 File Offset: 0x0003A1D7
		private static void FallbackLogFunction(string message, SimLogType simLogType)
		{
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0003BFD9 File Offset: 0x0003A1D9
		public void Trace(string message)
		{
			this.SimLogFunction(message, SimLogType.Trace);
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0003BFE8 File Offset: 0x0003A1E8
		public void Warn(string message)
		{
			this.SimLogFunction(message, SimLogType.Warning);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0003BFF7 File Offset: 0x0003A1F7
		public void Error(string message)
		{
			this.SimLogFunction(message, SimLogType.Error);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0003C006 File Offset: 0x0003A206
		public void WarnIf(bool condition, string message)
		{
			if (condition)
			{
				this.Warn(message);
			}
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0003C012 File Offset: 0x0003A212
		public void ErrorIf(bool condition, string message)
		{
			if (condition)
			{
				this.Error(message);
			}
		}

		// Token: 0x040006DF RID: 1759
		private readonly SimLogger.SimLogDelegate SimLogFunction;

		// Token: 0x020008F3 RID: 2291
		// (Invoke) Token: 0x060029EC RID: 10732
		public delegate void SimLogDelegate(string message, SimLogType simLogType);
	}
}
