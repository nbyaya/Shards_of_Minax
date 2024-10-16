using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a shadow ogre corpse")]
    public class ShadowOgre : BaseCreature
    {
        private DateTime m_NextShadowStep;
        private DateTime m_NextShadowStrike;
        private DateTime m_NextDarknessAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow ogre";
            Body = 1;
            Hue = 2171; // Dark hue for a shadowy appearance
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

        public ShadowOgre(Serial serial)
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
                    m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDarknessAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowStep)
                {
                    ShadowStep();
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextDarknessAura)
                {
                    DarknessAura();
                }
            }
        }

        private void ShadowStep()
        {
            Point3D newLocation = new Point3D(X + Utility.RandomMinMax(-5, 5), Y + Utility.RandomMinMax(-5, 5), Z);
            if (Map.CanSpawnMobile(newLocation))
            {
                MoveToWorld(newLocation, Map);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Ogre vanishes into the shadows! *");
                m_NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for ShadowStep
            }
        }

        private void ShadowStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(20, 30);
                target.Damage(damage, this);
                target.SendMessage("The Shadow Ogre strikes from the darkness!");
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Ogre strikes from the gloom! *");
                m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ShadowStrike
            }
        }

        private void DarknessAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Darkness engulfs the area! *");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The area around you darkens, reducing your visibility!");
                    // Example effect: reduce accuracy or visibility
                }
            }

            m_NextDarknessAura = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DarknessAura
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
