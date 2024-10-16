using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a venomous ettin corpse")]
    public class VenomousEttin : BaseCreature
    {
        private DateTime m_NextToxicSpit;
        private DateTime m_NextPoisonCloud;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VenomousEttin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a venomous ettin";
            Body = 18; // Ettin body
            Hue = 1557; // Unique hue
			BaseSoundID = 367;

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

        public VenomousEttin(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random interval between 5 and 20 seconds
                    m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random interval between 10 and 30 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextToxicSpit)
                {
                    ToxicSpit();
                }

                if (DateTime.UtcNow >= m_NextPoisonCloud)
                {
                    PoisonCloud();
                }
            }
        }

        private void ToxicSpit()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Ettin spits a toxic venom! *");
                target.SendMessage("You are hit by a toxic venom!");
                target.Damage(Utility.RandomMinMax(10, 15), this);
                target.ApplyPoison(this, Poison.Greater); // Apply poison

                // Update the next activation time for ToxicSpit
                Random rand = new Random();
                m_NextToxicSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40)); // Random interval between 20 and 40 seconds
            }
        }

        private void PoisonCloud()
        {
            Point3D loc = new Point3D(X, Y, Z);
            Map map = Map;

            if (map != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Venomous Ettin releases a poison cloud! *");
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 10, 30, 0x3F4, 0, 9515, 0);

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive && m.Player)
                    {
                        m.SendMessage("You are engulfed by a poisonous cloud!");
                        m.ApplyPoison(this, Poison.Greater); // Apply poison
                        m.Damage(Utility.RandomMinMax(5, 10), this);
                    }
                }

                // Update the next activation time for PoisonCloud
                Random rand = new Random();
                m_NextPoisonCloud = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 3)); // Random interval between 1 and 3 minutes
            }
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
