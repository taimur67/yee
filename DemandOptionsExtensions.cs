using System;

namespace LoG
{
	// Token: 0x020005D1 RID: 1489
	public static class DemandOptionsExtensions
	{
		// Token: 0x06001BFA RID: 7162 RVA: 0x00060E05 File Offset: 0x0005F005
		public static int GetNumCards(DemandOptions option)
		{
			switch (option)
			{
			case DemandOptions.TwoTribute:
				return 2;
			case DemandOptions.ThreeTribute:
				return 3;
			case DemandOptions.FourTribute:
				return 4;
			case DemandOptions.FiveTribute:
				return 5;
			case DemandOptions.SixTribute:
				return 6;
			}
			return 0;
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x00060E3A File Offset: 0x0005F03A
		public static int ToNumCards(this DemandOptions option)
		{
			return DemandOptionsExtensions.GetNumCards(option);
		}
	}
}
