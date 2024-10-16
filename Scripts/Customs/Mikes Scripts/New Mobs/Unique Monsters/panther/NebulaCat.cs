using System;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a nebula cat corpse")]
    public class NebulaCat : BaseCreature
    {
        private DateTime m_NextNebulaBurst;
        private DateTime m_NextGravityWell;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public NebulaCat()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a nebula cat";
            Body = 0xD6; // Using panther body
            Hue = 2182; // Cosmic hue
			BaseSoundID = 0x462; // Panther sound

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

        public NebulaCat(Serial serial)
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
                    m_NextNebulaBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextGravityWell = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextNebulaBurst)
                {
                    NebulaBurst();
                }

                if (DateTime.UtcNow >= m_NextGravityWell)
                {
                    GravityWell();
                }
            }
        }

        private void NebulaBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nebula Burst! *");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Nebula Cat's cosmic dust blinds and harms you!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.Damage(Utility.RandomMinMax(10, 20), this);
                }
            }

            m_NextNebulaBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset with fixed cooldown
        }

        private void GravityWell()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Gravity Well! *");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A gravity well pulls you towards the Nebula Cat!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.MoveToWorld(this.Location, this.Map);
                }
            }

            m_NextGravityWell = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Reset with fixed cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // Add serialized fields if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag to reinitialize on load
        }
    }
}
