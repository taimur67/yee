using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LoG
{
	// Token: 0x020005C4 RID: 1476
	[JsonObject(MemberSerialization.OptIn)]
	[Serializable]
	public class TargetContext : IDeepClone<TargetContext>
	{
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x0005F932 File Offset: 0x0005DB32
		public int PlayerId
		{
			get
			{
				return this._playerId;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001B87 RID: 7047 RVA: 0x0005F93A File Offset: 0x0005DB3A
		public Identifier ItemId
		{
			get
			{
				return this._itemId;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0005F942 File Offset: 0x0005DB42
		public HexCoord Location
		{
			get
			{
				return this._location;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0005F94A File Offset: 0x0005DB4A
		[JsonIgnore]
		public bool IsAnyTargetSet
		{
			get
			{
				return this._playerId != int.MinValue || this._itemId != Identifier.Invalid || this._location != HexCoord.Invalid;
			}
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x0005F974 File Offset: 0x0005DB74
		[JsonConstructor]
		public TargetContext()
		{
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0005F999 File Offset: 0x0005DB99
		public TargetContext(Identifier itemId)
		{
			this.SetTargetGameItem(itemId);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0005F9C5 File Offset: 0x0005DBC5
		public bool Equals(TargetContext other)
		{
			return other != null && this._playerId == other._playerId && this._itemId == other._itemId && this._location == other._location;
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0005F9F9 File Offset: 0x0005DBF9
		public bool GetTargetItem(out Identifier id)
		{
			id = this._itemId;
			return id != Identifier.Invalid;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0005FA0B File Offset: 0x0005DC0B
		public bool GetTargetPlayer(out PlayerIndex playerId)
		{
			playerId = (PlayerIndex)this._playerId;
			return playerId != PlayerIndex.Invalid;
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0005FA21 File Offset: 0x0005DC21
		public bool GetTargetHex(out HexCoord coord)
		{
			coord = this._location;
			return coord != HexCoord.Invalid;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0005FA3F File Offset: 0x0005DC3F
		public void SetTargetGamePiece(GamePiece gamePiece)
		{
			this._itemId = gamePiece.Id;
			this._playerId = gamePiece.ControllingPlayerId;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x0005FA59 File Offset: 0x0005DC59
		public void SetTargetGameItem(Identifier gameItem)
		{
			this._itemId = gameItem;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x0005FA62 File Offset: 0x0005DC62
		public void SetTargetGameItem(GameItem gameItem, int affectedPlayerId)
		{
			this.SetTargetGameItem(gameItem.Id, affectedPlayerId);
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0005FA71 File Offset: 0x0005DC71
		public void SetTargetGameItem(Identifier gameItemId, int affectedPlayerId)
		{
			this.SetTargetGameItem(gameItemId);
			this._playerId = affectedPlayerId;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0005FA81 File Offset: 0x0005DC81
		public void SetTargetHex(HexCoord hex, int affectedPlayerId)
		{
			this._location = hex;
			this._playerId = affectedPlayerId;
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0005FA91 File Offset: 0x0005DC91
		public void SetTargetHex(HexCoord hex)
		{
			this._location = hex;
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0005FA9A File Offset: 0x0005DC9A
		public void SetTargetPlayer(int playerId)
		{
			this._playerId = playerId;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0005FAA3 File Offset: 0x0005DCA3
		public void CopyFrom(TargetContext otherContext)
		{
			if (otherContext != null)
			{
				this._playerId = otherContext.PlayerId;
				this._itemId = otherContext._itemId;
				this._location = otherContext._location;
			}
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x0005FACC File Offset: 0x0005DCCC
		public string GetDebugValue()
		{
			if (this._playerId != -2147483648)
			{
				return this._playerId.ToString();
			}
			if (this._itemId != Identifier.Invalid)
			{
				return this._itemId.ToString();
			}
			if (this._location != HexCoord.Invalid)
			{
				return this._location.ToString();
			}
			return "";
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x0005FB36 File Offset: 0x0005DD36
		public void DeepClone(out TargetContext clone)
		{
			clone = new TargetContext
			{
				_playerId = this._playerId,
				_itemId = this._itemId,
				_location = this._location
			};
		}

		// Token: 0x04000C62 RID: 3170
		[JsonProperty]
		[DefaultValue(-2147483648)]
		private int _playerId = int.MinValue;

		// Token: 0x04000C63 RID: 3171
		[JsonProperty]
		[DefaultValue(Identifier.Invalid)]
		private Identifier _itemId = Identifier.Invalid;

		// Token: 0x04000C64 RID: 3172
		[JsonProperty]
		[DefaultValue(2147483647)]
		private HexCoord _location = HexCoord.Invalid;
	}
}
