using System;

namespace LoG
{
	// Token: 0x02000456 RID: 1110
	public static class TurnTimerExtensions
	{
		// Token: 0x06001501 RID: 5377 RVA: 0x0004FAEC File Offset: 0x0004DCEC
		public static TimeSpan ToTimeSpan(this TurnTimer_Data timerData)
		{
			switch (timerData.TimeSpanType)
			{
			case TimeSpanType.Minutes:
				return new TimeSpan(0, 0, timerData.TimeValue, 0);
			case TimeSpanType.Hours:
				return new TimeSpan(0, timerData.TimeValue, 0, 0);
			case TimeSpanType.Days:
				return new TimeSpan(timerData.TimeValue, 0, 0, 0);
			case TimeSpanType.Weeks:
				return new TimeSpan(timerData.TimeValue * 7, 0, 0, 0);
			}
			return default(TimeSpan);
		}
	}
}
