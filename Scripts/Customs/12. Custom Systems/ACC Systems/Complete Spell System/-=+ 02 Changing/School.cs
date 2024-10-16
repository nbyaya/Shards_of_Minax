using System;
using System.Collections;

namespace Server.ACC.CSS
{
	[Flags]
	public enum School
	{
		Invalid  = 0x00000000,
		Magery   = 0x00000001,
		Necro    = 0x00000002,
		Chivalry = 0x00000004,
		CookingMagic = 0x00000005,
		ForagersGuidebook = 0x00000006,
		MartialManual = 0x00000007,
		Ninja    = 0x00000008,
		Pastoralicon = 0x00000009,
		Samurai  = 0x00000010,
		Druid    = 0x00000020,
		Avatar   = 0x00000040,
		Bard     = 0x00000080,
		Cleric   = 0x00000100,
		Ranger   = 0x00000200,
		Rogue    = 0x00000400,
		Undead   = 0x00000800,
        Ancient  = 0x00001000,
	}
}