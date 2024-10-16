using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a toxic ogre corpse")]
    public class ToxicOgre : BaseCreature
    {
        private DateTime m_NextPoisonousBreath;
        private DateTime m_NextToxicSlap;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ToxicOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a toxic ogre";
            Body = 1; // Ogre body
            Hue = 2168; // Greenish hue for toxicity
			BaseSoundID = 427;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public ToxicOgre(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextPoisonousBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextToxicSlap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPoisonousBreath)
                {
                    PoisonousBreath();
                }

                if (DateTime.UtcNow >= m_NextToxicSlap)
                {
                    ToxicSlap();
                }
            }
        }

        private void PoisonousBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Ogre breathes out a noxious cloud! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.Player) // Ignore players for this example
                {
                    if (Utility.RandomDouble() < 0.5) // 50% chance to apply poison
                    {
                        m.SendMessage("You are engulfed by toxic fumes!");
                        m.ApplyPoison(this, Poison.Lethal); // Apply lethal poison
                        m.Damage(10, this); // Additional damage from the breath
                    }
                }
            }

            m_NextPoisonousBreath = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ToxicSlap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Ogre slaps with a toxic touch! *");
            FixedEffect(0x376A, 10, 16);

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(15, 20);
                target.Damage(damage, this);
                target.ApplyPoison(this, Poison.Deadly); // Apply deadly poison
                target.SendMessage("The Toxic Ogre's slap burns you with poison!");
            }

            m_NextToxicSlap = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (willKill)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Toxic Ogre's venomous skin strikes back! *");
                if (from != null && from.Alive)
                {
                    from.ApplyPoison(this, Poison.Deadly); // Apply deadly poison on hit
                    from.SendMessage("You feel a burning sensation as you strike the Toxic Ogre!");
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
