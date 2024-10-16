using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a nyx'rith corpse")]
    public class NyxRith : BaseCreature
    {
        private DateTime m_NextLifeDrain;
        private DateTime m_NextVitalitySiphon;
        private DateTime m_NextConsumeEssence;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public NyxRith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Nyx'Rith the Devourer";
            Body = 22; // ElderGazer body
            Hue = 1769; // Dark purple hue
			BaseSoundID = 377;

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

        public NyxRith(Serial serial)
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
                    m_NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextVitalitySiphon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextConsumeEssence = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 5));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLifeDrain)
                {
                    LifeDrain();
                }

                if (DateTime.UtcNow >= m_NextVitalitySiphon)
                {
                    VitalitySiphon();
                }

                if (DateTime.UtcNow >= m_NextConsumeEssence)
                {
                    ConsumeEssence();
                }
            }
        }

        private void LifeDrain()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                Hits = Math.Min(Hits + damage, HitsMax);

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nyx'Rith drains your life force! *");
                target.PlaySound(0x1F1);
                target.FixedEffect(0x376A, 10, 16);

                m_NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown after use
            }
        }

        private void VitalitySiphon()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player && m.Alive)
                {
                    int damage = Utility.RandomMinMax(5, 10);
                    m.Damage(damage, this);
                    Hits = Math.Min(Hits + damage, HitsMax);

                    PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nyx'Rith siphons vitality from its surroundings! *");
                    m.PlaySound(0x1F1);
                    m.FixedEffect(0x376A, 10, 16);
                }
            }

            m_NextVitalitySiphon = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Update cooldown after use
        }

        private void ConsumeEssence()
        {
            SetStr(Str + 20);
            SetDex(Dex + 15);
            SetInt(Int + 20);

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Nyx'Rith consumes essence and grows stronger! *");
            PlaySound(0x165);
            FixedEffect(0x37C4, 10, 36);

            m_NextConsumeEssence = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Update cooldown after use
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
