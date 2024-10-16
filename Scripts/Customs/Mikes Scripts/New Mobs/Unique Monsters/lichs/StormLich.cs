using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a storm lich corpse")]
    public class StormLich : BaseCreature
    {
        private DateTime m_NextLightningBolt;
        private DateTime m_NextStormCloud;
        private DateTime m_NextThunderousRoar;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public StormLich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm lich";
            Body = 24; // Lich body
            Hue = 2133; // Blue hue for stormy appearance
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

            PackItem(new LichFormScroll());
            PackNecroReg(20, 30);

            m_AbilitiesInitialized = false;
        }

        public StormLich(Serial serial)
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
                    m_NextLightningBolt = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStormCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextThunderousRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextLightningBolt)
                {
                    LightningBolt();
                }

                if (DateTime.UtcNow >= m_NextStormCloud)
                {
                    StormCloud();
                }

                if (DateTime.UtcNow >= m_NextThunderousRoar)
                {
                    ThunderousRoar();
                }
            }
        }

        private void LightningBolt()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Storm Lich casts Lightning Bolt! *");
                // Lightning bolt effect
                Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 2023); // Lightning effect
                target.Damage(40, this); // Adjust damage as needed
                target.Freeze(TimeSpan.FromSeconds(1)); // Stun effect
                m_NextLightningBolt = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            }
        }

        private void StormCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Storm Lich summons a storm cloud! *");
            // Storm cloud effect
            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m.Alive)
                    {
                        m.SendMessage("Lightning strikes you from the storm cloud!");
                        m.Damage(20, this); // Adjust damage as needed
                    }
                }
            });
            m_NextStormCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void ThunderousRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Storm Lich lets out a Thunderous Roar! *");
            // Thunderous Roar effect
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are disoriented by the Storm Lich's roar!");
                    m.Freeze(TimeSpan.FromSeconds(2)); // Disorient effect
                }
            }
            m_NextThunderousRoar = DateTime.UtcNow + TimeSpan.FromSeconds(40);
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

            m_AbilitiesInitialized = false;
        }
    }
}
