using System;
using System.ComponentModel;

namespace MonsterHunterStories2
{
	class Util
	{
		public const uint PC_ADDRESS = 0x30;

		public const uint CHARACTER_ADDRESS = 0x2D2A98;
		public const uint CHARACTER_COUNT = 31;
		public const uint CHARACTER_SIZE = 596;

		public const uint MONSTER_ADDRESS = 0x2D72C8;
		public const uint MONSTER_COUNT = 500;
		public const uint MONSTER_SIZE = 412;
		public const uint MONSTER_SLAIN_ADDRESS = 0x15E64;

		public const uint EGG_ADDRESS = 0x10CAC;
		public const uint EGG_COUNT = 12;
		public const uint EGG_SIZE = 120;
		public const uint EGG_COUNT_ADDRESS = 0x10C4A;
		public const uint EGG_AllCOUNT_CommonSmell_ADDRESS = 0x140E4;
		public const uint EGG_AllCOUNT_HighSmell_ADDRESS = 0x140E8;
		public const uint EGG_AllCOUNT_CoOp_ADDRESS = 0x140EC;

		public const uint WEAPON_ADDRESS = 0x3ECC;
		public const uint WEAPON_COUNT = 700;
		public const uint WEAPON_SIZE = 36;

		public const uint ARMOR_ADDRESS = 0xA13C;
		public const uint ARMOR_COUNT = 400;
		public const uint ARMOR_SIZE = 36;

		public const uint TALISMAN_ADDRESS = 0xD97C;
		public const uint TALISMAN_COUNT = 200;
		public const uint TALISMAN_SIZE = 48;

		public const uint ITEMSETTING_ADDRESS = 0x12B68;
		public const uint MONEY_ADDRESS = 0x48;
		public const uint ITEM_ADDRESS = 0x4C;

		public const uint GUIDE_Monsterpedia_ADDRESS_1 = 0x11A68;//11A68 - 1225F
		public const uint GUIDE_Monsterpedia_ADDRESS_2 = 0x12B68;//12B68 - 12C42
		public const uint GUIDE_Monstipedia_ADDRESS = 0x12C90;//12C90 - 12D3F
		public const uint GUIDE_BookofGenes_ADDRESS = 0x12D40;//12D40 - 12E1F

		public const uint DAN_ADDRESS = 0x4037C;
		public const uint DAN_COUNT = 1024;
		public const uint DAN_SIZE = 116;

		public const uint MEDALS_ADDRESS = 0x13EE0;
		public const uint MELYNXLNC_ADDRESS = 0x12C50;  //12C50 - 12C8F
		public const uint MELYNXLNC_COUNT = 0x40;

		public const uint EggHatched_ADDRESS = 0x15666; //size 0x16A

		public const uint CoOp_COUNT1_ADDRESS = 0x141A8;
		public const uint CoOp_COUNT2_ADDRESS = 0x141C8;

		public const uint TOTAL_BATTLES_ADDRESS = 0x14138;
		public const uint TOTAL_PAIR_BATTLES_ADDRESS = 0x14148;
		public const uint TOTAL_WINS_ADDRESS = 0x141B8;
		public const uint TOTAL_PAIR_WINS_ADDRESS = 0x141C0;

		public const uint TOTAL_BATTLES_FRIENDS_ADDRESS = 0x14188;
		public const uint TOTAL_PAIR_BATTLES_FRIENDS_ADDRESS = 0x14198;
		public const uint TOTAL_WINS_FRIENDS_ADDRESS = 0x141E0;
		public const uint TOTAL_PAIR_WINS_FRIENDS_ADDRESS = 0x141E8;

		public const uint STORY_PARTNER_1 = 0x2D72C4; //13
		public const uint STORY_PARTNER_2 = 0x30C7FC; //13
		public const uint STORY_1 = 0x162C8;  //C0 9A 5E 00
		public const uint STORY_2 = 0x1640E; //36
		public const uint STORY_3 = 0x16534; //65 30 36 32
		public const uint STORY_4 = 0x19070; //0x19070 - 0x19088  -> 00
		public const uint STORY_5 = 0x1661C; //2B 52 20 67
		public const uint STORY_6 = 0x164F4; //57 51 22 1D
		public const uint STORY_7 = 0x163CC; //C8 A7 A0 86



		public static void WriteNumber(uint address, uint size, uint value, uint min, uint max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			SaveData.Instance().WriteNumber(address, size, value);
		}

		public static uint ItemIDAddress(uint id)
		{
			return ITEM_ADDRESS + id * 8;
		}

		public static String ReadHex(uint address, uint size)
		{
			byte[] bytes = SaveData.Instance().ReadValue(address, size);
			if (bytes == null) return "";

			var hexText = new System.Text.StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i % 2 == 0 && i != 0) hexText.Append(" ");
				hexText.Append(bytes[i].ToString("X2"));
			}
			return hexText.ToString();
		}

		public static void WriteHex(uint address, String hexString)
		{
			hexString = hexString.Replace(" ", "");
			if (hexString.Length % 2 != 0)
			{
				hexString += "0";
			}

			byte[] writeBytes = new byte[hexString.Length / 2];
			for (int i = 0; i < writeBytes.Length; i++)
			{
				writeBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}
			SaveData.Instance().WriteValue(address, writeBytes);
		}
		
	}
}
