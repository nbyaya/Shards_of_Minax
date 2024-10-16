using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a fungal toad corpse")]
    public class FungalToad : BaseCreature
    {
        private DateTime m_NextSporeBurst;
        private DateTime m_NextFungalGrowth;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FungalToad()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fungal toad";
            Body = 80; // Giant toad body
            Hue = 2449; // Unique hue for the Fungal Toad
            BaseSoundID = 0x26B;
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

        public FungalToad(Serial serial)
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
                    m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFungalGrowth = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSporeBurst)
                {
                    SporeBurst();
                }

                if (DateTime.UtcNow >= m_NextFungalGrowth)
                {
                    FungalGrowth();
                }
            }
        }

        private void SporeBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a burst of toxic spores! *");
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are engulfed in a cloud of toxic spores!");
                    int damage = Utility.RandomMinMax(10, 20);
                    m.Damage(damage, this);

                    if (Utility.RandomDouble() < 0.3)
                    {
                        // Chance to inflict sleep effect
                        m.SendMessage("You feel drowsy from the toxic spores!");
                        m.Freeze(TimeSpan.FromSeconds(2));
                    }
                }
            }

            m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Update cooldown after use
        }

        private void FungalGrowth()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Summons toxic mushrooms around it! *");
            FixedEffect(0x373A, 10, 16);

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
                if (Map.CanSpawnMobile(loc))
                {
                    ToxicMushroom mushroom = new ToxicMushroom();
                    mushroom.MoveToWorld(loc, Map);
                }
            }

            m_NextFungalGrowth = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown after use
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

    public class ToxicMushroom : Item
    {
        private class ToxicMushroomTimer : Timer
        {
            private ToxicMushroom m_Mushroom;

            public ToxicMushroomTimer(ToxicMushroom mushroom)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_Mushroom = mushroom;
            }

            protected override void OnTick()
            {
                if (m_Mushroom == null || m_Mushroom.Deleted)
                    return;

                foreach (Mobile m in m_Mushroom.GetMobilesInRange(1))
                {
                    if (m != null && m.Alive)
                    {
                        m.SendMessage("The toxic mushroom emits a poisonous cloud!");
                        int damage = Utility.RandomMinMax(5, 10);
                        m.Damage(damage);
                    }
                }

                // Delete the mushroom after applying damage
                m_Mushroom.Delete();
            }
        }

        [Constructable]
        public ToxicMushroom()
            : base(0x1D8B) // Mushroom graphic
        {
            Name = "a toxic mushroom";
            Hue = 0x9C4; // Matching hue to Fungal Toad

            // Start the timer to handle periodic damage and self-destruction
            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Delete)); // Remove after 5 seconds to prevent endless presence
            new ToxicMushroomTimer(this).Start();
        }

        public ToxicMushroom(Serial serial)
            : base(serial)
        {
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
        }
    }
}
