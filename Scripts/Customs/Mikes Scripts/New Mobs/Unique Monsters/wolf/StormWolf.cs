using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a storm wolf corpse")]
    public class StormWolf : BaseCreature
    {
        private DateTime m_NextLightningBite;
        private DateTime m_NextThunderClap;
        private DateTime m_NextStormAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm wolf";
            Body = 23; // DireWolf body
            Hue = 2591; // Stormy hue
			BaseSoundID = 0xE5;
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

        public StormWolf(Serial serial)
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
                    m_NextLightningBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextThunderClap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLightningBite)
                {
                    LightningBite();
                }

                if (DateTime.UtcNow >= m_NextThunderClap)
                {
                    ThunderClap();
                }

                if (DateTime.UtcNow >= m_NextStormAura)
                {
                    StormAura();
                }
            }
        }

        private void LightningBite()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Wolf's bite crackles with lightning! *");
                Mobile target = Combatant as Mobile;
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                target.FixedEffect(0x29A, 10, 16);
                target.PlaySound(0x29C); // Lightning sound
                m_NextLightningBite = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void ThunderClap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Wolf roars, releasing a thunderous clap! *");
            FixedEffect(0x376A, 10, 16);
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant)
                {
                    m.SendMessage("You are stunned by the Thunder Clap!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.Damage(10, this);
                }
            }
            m_NextThunderClap = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        private void StormAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Wolf is surrounded by a swirling storm aura! *");
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant)
                {
                    m.SendMessage("You are hurt by the Storm Wolf's aura!");
                    m.Damage(5, this);
                }
            }
            m_NextStormAura = DateTime.UtcNow + TimeSpan.FromSeconds(10);
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
