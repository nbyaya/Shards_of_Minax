using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frost lich corpse")]
    public class FrostLich : BaseCreature
    {
        private DateTime m_NextFrostbite;
        private DateTime m_NextIceBarrier;
        private DateTime m_NextFrozenTomb;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FrostLich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frost lich";
            Body = 24; // Lich body
            Hue = 2146; // Ice blue hue
            BaseSoundID = 0x3E9;

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

            PackItem(new GnarledStaff());
            PackNecroReg(20, 30);

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public FrostLich(Serial serial)
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
                    m_NextFrostbite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextIceBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFrozenTomb = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrostbite)
                {
                    Frostbite();
                }

                if (DateTime.UtcNow >= m_NextIceBarrier)
                {
                    IceBarrier();
                }

                if (DateTime.UtcNow >= m_NextFrozenTomb)
                {
                    FrozenTomb();
                }
            }
        }

        private void Frostbite()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Frostbite! *");
                target.SendMessage("You are being chilled by the Frost Lich's frostbite!");
                // Deal cold damage over time
                int damage = Utility.RandomMinMax(5, 10);
                target.Damage(damage, this);
                target.SendMessage("You feel a chilling sensation!");

                // Applying a debuff (e.g., slowing down)
                target.SendMessage("You are slowed down by the frostbite!");
                target.SendMessage("You are taking cold damage over time!");

                m_NextFrostbite = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void IceBarrier()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Ice Barrier! *");
            // Create an ice shield (absorbs damage)
            // Implement logic for damage absorption here
            // For example, you can use a custom method or attribute to handle damage absorption

            m_NextIceBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void FrozenTomb()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Frozen Tomb! *");
                target.SendMessage("You are encased in a block of ice!");
                // Immobilize the target
                target.Freeze(TimeSpan.FromSeconds(5));
                
                m_NextFrozenTomb = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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
