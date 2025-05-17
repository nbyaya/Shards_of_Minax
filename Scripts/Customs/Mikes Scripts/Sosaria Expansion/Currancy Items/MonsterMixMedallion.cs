using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	// ── 3) Monster‐Mix Medallion — shuffles your monster combo ────────────
	public class MonsterMixMedallion : Item
	{
		[Constructable]
		public MonsterMixMedallion() : base(0x1F18)
		{
			Name = "Monster‐Mix Medallion";
			Hue  = 0x56D;
			Weight = 1.0;
		}

		public MonsterMixMedallion(Serial s) : base(s) { }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}

			from.SendMessage("Select the magic map you wish to remix.");
			from.Target = new InternalTarget(this);
		}

		private class InternalTarget : Target
		{
			private readonly MonsterMixMedallion _medal;
			public InternalTarget(MonsterMixMedallion medal) : base(12, false, TargetFlags.None)
			{
				_medal = medal;
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				if (_medal.Deleted) return;

				if (targ is MagicMapBase map && map.IsChildOf(from.Backpack))
				{
					map.ShuffleMonsterCombo();
					from.SendMessage("Beasts howl in anticipation—the monster lineup has changed!");
					_medal.Delete();
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
