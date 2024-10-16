using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a muck golem corpse")]
    public class SummonedMuckGolem : BaseCreature
    {
        private DateTime m_NextMudSling;
        private DateTime m_NextQuicksandTrap;
        private DateTime m_NextSwampMeld;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SummonedMuckGolem()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a muck golem";
            Body = 752; // Golem body
            Hue = 1929; // Muddy brown hue
			BaseSoundID = 357;

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
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SummonedMuckGolem(Serial serial)
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
                    m_NextMudSling = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextQuicksandTrap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSwampMeld = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMudSling)
                {
                    MudSling();
                }

                if (DateTime.UtcNow >= m_NextQuicksandTrap)
                {
                    QuicksandTrap();
                }

                if (DateTime.UtcNow >= m_NextSwampMeld)
                {
                    SwampMeld();
                }
            }
        }

        private void MudSling()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mud Golem slings a glob of mud! *");

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                Effects.SendTargetEffect(target, 0x36D4, 10);
                target.SendMessage("You are hit by a glob of mud, reducing your accuracy!");

                target.SendMessage("You feel your movements slow down and your accuracy decrease!");
                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 0);
                target.Damage(Utility.RandomMinMax(5, 15));

                m_NextMudSling = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown
            }
        }

        private void QuicksandTrap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mud Golem creates a quicksand trap! *");

            Point3D loc = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
            Effects.SendLocationEffect(loc, Map, 0x3709, 10, 20, 0x2D1, 3);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("You are caught in quicksand and feel yourself being pulled down!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 0);

                    // Slow down and immobilize
                    m.Freeze(TimeSpan.FromSeconds(5));
                }
            }

            m_NextQuicksandTrap = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Update cooldown
        }

        private void SwampMeld()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mud Golem melds with the swamp! *");

            Effects.SendLocationEffect(Location, Map, 0x3709, 10, 10, 0x2D1, 3);
            this.FixedParticles(0x376A, 10, 16, 0x3F, EffectLayer.Waist);

            this.Hue = 0x3F4; // Blend with swamp
            this.VirtualArmor = 75;

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(EndSwampMeld));

            m_NextSwampMeld = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Update cooldown
        }

        private void EndSwampMeld()
        {
            this.Hue = 1736; // Return to original hue
            this.VirtualArmor = 50;
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
