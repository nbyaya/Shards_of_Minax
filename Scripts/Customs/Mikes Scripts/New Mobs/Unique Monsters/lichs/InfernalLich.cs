using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an infernal lich corpse")]
    public class InfernalLich : BaseCreature
    {
        private DateTime m_NextHellfireBlast;
        private DateTime m_NextInfernoAura;
        private DateTime m_NextVolcanicEruption;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public InfernalLich()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an infernal lich";
            Body = 24;
            Hue = 2143; // Fiery hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public InfernalLich(Serial serial)
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
                    m_NextHellfireBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextInfernoAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextVolcanicEruption = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextHellfireBlast)
                {
                    HellfireBlast();
                }

                if (DateTime.UtcNow >= m_NextInfernoAura)
                {
                    InfernoAura();
                }

                if (DateTime.UtcNow >= m_NextVolcanicEruption)
                {
                    VolcanicEruption();
                }
            }
        }

        private void HellfireBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Unleashes Hellfire Blast! *");
            PlaySound(0x1F2);
            FixedEffect(0x3709, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(30, 50);
                    m.Damage(damage, this);
                    m.SendMessage("You are scorched by a fiery explosion!");
                }
            }

            m_NextHellfireBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
        }

        private void InfernoAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Radiates Inferno Aura! *");
            PlaySound(0x1F3);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    m.Damage(damage, this);
                    m.SendMessage("The aura of flames around the Infernal Lich burns you!");
                }
            }

            m_NextInfernoAura = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Reset cooldown
        }

        private void VolcanicEruption()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Causes a Volcanic Eruption! *");
            PlaySound(0x1F4);
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(40, 60);
                    m.Damage(damage, this);
                    m.SendMessage("The ground erupts in flames beneath you!");
                }
            }

            m_NextVolcanicEruption = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Reset cooldown
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
