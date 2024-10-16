using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a storm ogre corpse")]
    public class StormOgre : BaseCreature
    {
        private DateTime m_NextLightningBolt;
        private DateTime m_NextThunderstrike;
        private DateTime m_NextStormAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm ogre";
            Body = 1;
            Hue = 2170; // Lightning-themed hue
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

            PackItem(new Club());
        }

        public StormOgre(Serial serial)
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
                    m_NextLightningBolt = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextThunderstrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLightningBolt)
                {
                    LightningBolt();
                }

                if (DateTime.UtcNow >= m_NextThunderstrike)
                {
                    Thunderstrike();
                }

                if (DateTime.UtcNow >= m_NextStormAura)
                {
                    StormAura();
                }
            }
        }

        private void LightningBolt()
        {
            if (Combatant != null && Combatant.Alive)
            {
                Mobile target = Combatant as Mobile;
                int damage = Utility.RandomMinMax(15, 25);

                target.Damage(damage, this);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Lightning Bolt! *");
                target.FixedEffect(0x36D4, 10, 16);
                target.SendMessage("You are struck by a bolt of lightning!");

                m_NextLightningBolt = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown
            }
        }

        private void Thunderstrike()
        {
            if (Combatant != null)
            {
                Point3D loc = Location;
                int range = 2;

                foreach (Mobile m in GetMobilesInRange(range))
                {
                    if (m != this && m != Combatant && m.Alive)
                    {
                        int damage = Utility.RandomMinMax(20, 30);

                        m.Damage(damage, this);
                        m.SendMessage("You are blasted by a shockwave!");
                        m.FixedEffect(0x3B2, 10, 16);
                    }
                }

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thunderstrike! *");
                FixedEffect(0x376A, 10, 16);

                m_NextThunderstrike = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Set cooldown
            }
        }

        private void StormAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Storm Aura! *");
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(5, 10);

                    m.Damage(damage, this);
                    m.SendMessage("You are struck by a burst of lightning from the storm!");
                }
            }

            m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextLightningBolt = DateTime.UtcNow; // Reset to now
            m_NextThunderstrike = DateTime.UtcNow; // Reset to now
            m_NextStormAura = DateTime.UtcNow; // Reset to now
            m_AbilitiesInitialized = false; // Reset flag
        }
    }
}
