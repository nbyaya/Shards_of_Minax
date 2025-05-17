using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	// ── 2) Time‐Turn Token — gives you extra time in the instance ────────
	public class TimeTurnToken : Item
	{
		[Constructable]
		public TimeTurnToken() : base(0x1F17)
		{
			Name = "Time‐Turn Token";
			Hue  = 0x4F2;
			Weight = 1.0;
		}

		public TimeTurnToken(Serial s) : base(s) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}

			from.SendMessage("Select the magic map you wish to prolong.");
			from.Target = new InternalTarget(this);
		}

		private class InternalTarget : Target
		{
			private readonly TimeTurnToken _token;
			public InternalTarget(TimeTurnToken token) : base(12, false, TargetFlags.None)
			{
				_token = token;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_token.Deleted) return;

				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					map.ExtendExpiration(TimeSpan.FromMinutes(10));
					from.SendMessage("Temporal energies weave around the parchment—extra duration granted!");
					_token.Delete();
				}
				else
				{
					from.SendMessage("That’s not a magic map in your pack.");
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			reader.ReadInt();
		}
	}


}
