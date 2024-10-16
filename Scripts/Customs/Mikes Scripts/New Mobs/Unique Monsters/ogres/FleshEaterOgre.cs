using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a flesh eater ogre corpse")]
    public class FleshEaterOgre : BaseCreature
    {
        private DateTime m_NextFleshFeast;
        private DateTime m_NextDiseaseAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FleshEaterOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flesh eater ogre";
            Body = 1;
            Hue = 2174; // Unique hue for the Flesh Eater Ogre
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

            PackItem(new Club());

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FleshEaterOgre(Serial serial)
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
                    m_NextFleshFeast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random start time
                    m_NextDiseaseAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random start time
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFleshFeast)
                {
                    FleshFeast();
                }

                if (DateTime.UtcNow >= m_NextDiseaseAura)
                {
                    DiseaseAura();
                }
            }
        }

        private void FleshFeast()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(10, 15);
                target.Damage(damage, this);
                Hits = Math.Min(Hits + (int)(damage * 0.5), HitsMax);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Feasts on the flesh! *");
                m_NextFleshFeast = DateTime.UtcNow + TimeSpan.FromSeconds(10); // Cooldown for FleshFeast
            }
        }

        private void DiseaseAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A disease aura surrounds the ogre! *");
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player && m.Alive)
                {
                    m.SendMessage("You feel a disease seeping into your body!");
                    m.Damage(5, this);
                }
            }
            m_NextDiseaseAura = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Cooldown for DiseaseAura
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
