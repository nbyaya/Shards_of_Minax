using System;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server;
using System.Linq;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a storm serpent corpse")]
    public class StormSerpent : BaseCreature
    {
        private Mobile _summoner;

        [Constructable]
        public StormSerpent()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm serpent";
            Body = 0x15; // Giant serpent body
            Hue = 0x480; // Electric blue
            BaseSoundID = 219;



            double skillFactor = 0;

            SetStr((int)(200 + (50 * skillFactor)));
            SetDex((int)(80 + (30 * skillFactor)));
            SetInt((int)(100 + (50 * skillFactor)));

            SetHits((int)(200 + (100 * skillFactor)));
            SetMana((int)(100 + (100 * skillFactor)));

            SetDamage(12, 22);

            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 30 + (int)(10 * skillFactor));
            SetResistance(ResistanceType.Fire, 20 + (int)(10 * skillFactor));
            SetResistance(ResistanceType.Cold, 30);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 60 + (int)(20 * skillFactor));

            SetSkill(SkillName.MagicResist, 80.0 + (20 * skillFactor));
            SetSkill(SkillName.Tactics, 75.0 + (20 * skillFactor));
            SetSkill(SkillName.Wrestling, 70.0 + (20 * skillFactor));
            SetSkill(SkillName.EvalInt, 50.0 + (20 * skillFactor));
            SetSkill(SkillName.Magery, 60.0 + (20 * skillFactor));

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            Timer.DelayCall(TimeSpan.FromSeconds(2), DoChainLightning);
        }

        public override bool Commandable => true;
        public override bool BardImmune => true;
        public override Poison PoisonImmune => Poison.Greater;

        public override void GenerateLoot()
        {
            // No loot – summon
        }

        public StormSerpent(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

		public void DoChainLightning()
		{
			if (Deleted || !Alive)
				return;

			Timer.DelayCall(TimeSpan.FromSeconds(6.0), DoChainLightning);

			if (_summoner == null || !_summoner.Alive)
				return;

			Mobile target = Combatant as Mobile;

			if (target != null && CanBeHarmful(target) && Utility.RandomDouble() <= 0.6)
			{
				PublicOverheadMessage(MessageType.Emote, 0x9, false, "*crackles with electric fury*");

				Effects.SendLocationEffect(Location, Map, 0x37B9, 10, 10, 0x481, 0);
				FixedParticles(0x37B9, 10, 30, 5052, EffectLayer.Head);

				List<Mobile> targets = GetNearbyEnemies(this, 4);
				foreach (Mobile m in targets)
				{
					m.BoltEffect(0);
					AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 0, 0, 0, 100); // 100% energy
				}
			}
		}


		private static List<Mobile> GetNearbyEnemies(Mobile from, int range)
		{
			List<Mobile> targets = new List<Mobile>();

			foreach (object obj in from.GetMobilesInRange(range))
			{
				if (obj is Mobile m && m != from && m.Alive && from.CanBeHarmful(m) && from.InLOS(m))
				{
					targets.Add(m);
				}
			}

			return targets;
		}

    }
}
