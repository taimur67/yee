using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x02000308 RID: 776
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class NemesisChangeMessage : CannedMessage
	{
		// Token: 0x06000F2A RID: 3882 RVA: 0x0003C25A File Offset: 0x0003A45A
		public static int CountReceived(PlayerState receivingPlayer)
		{
			return receivingPlayer.MessageLog.GetAllReceivedMessages().Count;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0003C26C File Offset: 0x0003A46C
		public static int CountReceivedRecently(PlayerState receivingPlayer, TurnContext context, int recencyTurnCount = 3)
		{
			return receivingPlayer.MessageLog.GetAllReceivedMessages().Count((Message m) => m.TurnId >= context.CurrentTurn.TurnValue - recencyTurnCount);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0003C2AC File Offset: 0x0003A4AC
		public static int CountSentRecently(PlayerState sendingPlayer, TurnContext context, int recencyTurnCount = 3)
		{
			return sendingPlayer.MessageLog.GetAllSentMessages(sendingPlayer.Id).Count((Message m) => m.TurnId >= context.CurrentTurn.TurnValue - recencyTurnCount);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0003C2F0 File Offset: 0x0003A4F0
		public static int CountReceivedFromPlayer(PlayerState receivingPlayer, PlayerState sendingPlayer)
		{
			return receivingPlayer.MessageLog.GetAllReceivedMessages().Count((Message m) => m.FromPlayerId == sendingPlayer.Id);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0003C328 File Offset: 0x0003A528
		public static int CountRecentlyReceivedFromPlayer(PlayerState receivingPlayer, PlayerState sendingPlayer, TurnContext context, int recencyTurnCount = 3)
		{
			return (from m in receivingPlayer.MessageLog.GetAllReceivedMessages()
			where m.TurnId >= context.CurrentTurn.TurnValue - recencyTurnCount
			select m).Count((Message m) => m.FromPlayerId == sendingPlayer.Id);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0003C380 File Offset: 0x0003A580
		public static void TriggerForValidRecipients(TurnContext context, PlayerState sendingPlayer)
		{
			TurnState currentTurn = context.CurrentTurn;
			if (!sendingPlayer.Animosity.HasRecentlyChangedNemesis(currentTurn, 5))
			{
				return;
			}
			PlayerState playerState;
			float num;
			if (!currentTurn.TryGetNemesis(sendingPlayer, out playerState, out num))
			{
				return;
			}
			if (num < 0.3f)
			{
				return;
			}
			float num2;
			if (sendingPlayer.Animosity.GetPlayerIDWithMostAnimosity(out num2) != playerState.Id)
			{
				return;
			}
			IEnumerable<PlayerState> enumerable = (from p in context.CurrentTurn.EnumeratePlayerStates(false, false)
			where p.Id != sendingPlayer.Id
			select p).OrderByDescending(delegate(PlayerState p)
			{
				int num4 = context.Random.Next() % 20;
				num4 -= NemesisChangeMessage.CountReceived(p);
				if (p.Role == PlayerRole.Human)
				{
					num4 += 100;
				}
				return num4;
			});
			int num3 = 0;
			foreach (PlayerState playerState2 in enumerable)
			{
				if (num3 <= 1 && NemesisChangeMessage.CountSentRecently(sendingPlayer, context, 5) < 3 && NemesisChangeMessage.CountReceivedRecently(playerState2, context, 3) < 2 && NemesisChangeMessage.CountRecentlyReceivedFromPlayer(playerState2, sendingPlayer, context, 7) < 1)
				{
					NemesisChangeMessage nemesisChangeMessage = new NemesisChangeMessage(context, sendingPlayer, playerState2, playerState);
					if (!nemesisChangeMessage.WasAlreadySentTo(playerState2))
					{
						playerState2.MessageLog.AddIncomingMessage(nemesisChangeMessage);
						num3++;
						context.CurrentTurn.AddGameEvent<MessageReceivedEvent>(new MessageReceivedEvent(nemesisChangeMessage));
					}
				}
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0003C504 File Offset: 0x0003A704
		[JsonConstructor]
		public NemesisChangeMessage()
		{
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0003C518 File Offset: 0x0003A718
		public NemesisChangeMessage(TurnContext context, PlayerState sender, PlayerState receiver, PlayerState nemesis) : base(context, sender, receiver)
		{
			this.NewNemesisId = nemesis.Id;
			if (nemesis.Id == receiver.Id)
			{
				this.change = NemesisChangeMessage.NemesisChangeType.Recipient;
			}
			else if (receiver.Id == sender.Animosity.PreviousNemesisId)
			{
				this.change = NemesisChangeMessage.NemesisChangeType.Replaced;
			}
			else
			{
				this.change = NemesisChangeMessage.NemesisChangeType.Witness;
			}
			this.LocalizedStringId = "Canned." + sender.ArchfiendId + ".NewNemesis." + Enum.GetName(typeof(NemesisChangeMessage.NemesisChangeType), this.change);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0003C5B8 File Offset: 0x0003A7B8
		public override void DeepClone(out Message message)
		{
			NemesisChangeMessage nemesisChangeMessage;
			this.DeepClone(out nemesisChangeMessage);
			message = nemesisChangeMessage;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0003C5D0 File Offset: 0x0003A7D0
		private void DeepClone(out NemesisChangeMessage clone)
		{
			clone = new NemesisChangeMessage
			{
				change = this.change,
				NewNemesisId = this.NewNemesisId,
				LocalizedStringId = this.LocalizedStringId.DeepClone()
			};
			base.DeepCloneMessageParts(clone);
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0003C60C File Offset: 0x0003A80C
		public bool IsDuplicateOf(Message other)
		{
			NemesisChangeMessage nemesisChangeMessage = other as NemesisChangeMessage;
			return nemesisChangeMessage == null || (this.FromPlayerId == nemesisChangeMessage.FromPlayerId && this.ToPlayerId == nemesisChangeMessage.ToPlayerId && this.change == nemesisChangeMessage.change);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0003C656 File Offset: 0x0003A856
		public bool WasAlreadySentTo(PlayerState playerState)
		{
			return playerState.MessageLog.GetAllReceivedMessages().Any(new Func<Message, bool>(this.IsDuplicateOf));
		}

		// Token: 0x040006ED RID: 1773
		private const int PriorityMaxRandomBonus = 20;

		// Token: 0x040006EE RID: 1774
		private const int PriorityBonusForHumans = 100;

		// Token: 0x040006EF RID: 1775
		private const int PriorityPenaltyPerMessageReceived = 1;

		// Token: 0x040006F0 RID: 1776
		private const int ReceiverTotalMessagesMaxPerWindow = 2;

		// Token: 0x040006F1 RID: 1777
		private const int ReceiverTotalMessagesWindowTurnDuration = 3;

		// Token: 0x040006F2 RID: 1778
		private const int ReceiverFromSenderMaxPerWindow = 1;

		// Token: 0x040006F3 RID: 1779
		private const int ReceiverFromSenderWindowTurnDuration = 7;

		// Token: 0x040006F4 RID: 1780
		private const int SenderMaxTurnsToWaitToSend = 5;

		// Token: 0x040006F5 RID: 1781
		private const int SenderMaxMessagesPerTurn = 1;

		// Token: 0x040006F6 RID: 1782
		private const int SenderMaxMessagesPerWindow = 3;

		// Token: 0x040006F7 RID: 1783
		private const int SenderWindowTurnDuration = 5;

		// Token: 0x040006F8 RID: 1784
		private const float SenderMinAnimosityToSend = 0.3f;

		// Token: 0x040006F9 RID: 1785
		[BindableValue("archfiend_name", BindingOption.IntPlayerId)]
		[JsonProperty]
		[DefaultValue(-2147483648)]
		public int NewNemesisId = int.MinValue;

		// Token: 0x040006FA RID: 1786
		[JsonProperty]
		public NemesisChangeMessage.NemesisChangeType change;

		// Token: 0x020008F4 RID: 2292
		public enum NemesisChangeType
		{
			// Token: 0x04001415 RID: 5141
			Recipient,
			// Token: 0x04001416 RID: 5142
			Replaced,
			// Token: 0x04001417 RID: 5143
			Witness
		}
	}
}
