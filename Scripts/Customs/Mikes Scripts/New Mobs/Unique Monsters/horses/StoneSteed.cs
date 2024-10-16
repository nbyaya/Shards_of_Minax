using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a stone steed corpse")]
    public class StoneSteed : BaseMount
    {
        private DateTime m_NextEarthquake;
        private DateTime m_NextStoneSkin;
        private DateTime m_NextRockThrow;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        private int _armorBonus;
        private bool _immunityToKnockbacks;

        [Constructable]
        public StoneSteed()
            : base("a stone steed", 0xE2, 0x3EA0, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Hue = 2086; // Earthy brown hue
			BaseSoundID = 0xA8;

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

        public StoneSteed(Serial serial)
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
                    m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextRockThrow = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEarthquake)
                {
                    Earthquake();
                }

                if (DateTime.UtcNow >= m_NextStoneSkin)
                {
                    StoneSkin();
                }

                if (DateTime.UtcNow >= m_NextRockThrow)
                {
                    RockThrow();
                }
            }
        }

        private void Earthquake()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Stone Steed causes the ground to tremble! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are stunned by the quake!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.Damage(20, this);
                }
            }

            m_NextEarthquake = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void StoneSkin()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Stone Steed’s skin hardens like rock! *");
            FixedEffect(0x37C4, 10, 36);

            _armorBonus = 15;
            _immunityToKnockbacks = true;

            Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
            {
                _armorBonus = 0;
                _immunityToKnockbacks = false;
            });

            m_NextStoneSkin = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void RockThrow()
        {
            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Stone Steed hurls a massive rock! *");
                FixedEffect(0x374A, 10, 16);

                int damage = Utility.RandomMinMax(30, 50);
                target.Damage(damage, this);
                target.Freeze(TimeSpan.FromSeconds(2));

                m_NextRockThrow = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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
