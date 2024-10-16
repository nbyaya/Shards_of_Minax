using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an inferno stallion corpse")]
    public class InfernoStallion : BaseMount
    {
        private DateTime m_NextFlamingHooves;
        private DateTime m_NextFireballBlast;
        private DateTime m_NextInfernoAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public InfernoStallion()
            : base("an inferno stallion", 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Hue = 2091; // Fire hue
			BaseSoundID = 0xA8;

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

        public InfernoStallion(Serial serial)
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
                    m_NextFlamingHooves = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextFireballBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextInfernoAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFlamingHooves)
                {
                    FlamingHooves();
                }

                if (DateTime.UtcNow >= m_NextFireballBlast)
                {
                    FireballBlast();
                }

                if (DateTime.UtcNow >= m_NextInfernoAura)
                {
                    InfernoAura();
                }
            }
        }

        private void FlamingHooves()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Inferno Stallion’s hooves blaze with fire!*");
                PlaySound(0x227);
                FixedEffect(0x3709, 10, 16);

                // Chance to set the target on fire
                if (Utility.RandomDouble() < 0.25)
                {
                    // Cast Combatant to Mobile
                    Mobile target = Combatant as Mobile;

                    if (target != null)
                    {
                        target.SendMessage("You have been poisoned!");
                        target.Poison = Poison.Lesser; // Apply Lesser Poison, adjust if necessary
                    }
                }

                m_NextFlamingHooves = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            }
        }

        private void FireballBlast()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*A fireball erupts from the Inferno Stallion!*");
                PlaySound(0x227);
                FixedEffect(0x3709, 10, 16);

                // Fireball effect (simplified for this example)
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != this && m != Combatant)
                    {
                        int damage = Utility.RandomMinMax(20, 30);
                        m.Damage(damage, this);
                    }
                }

                m_NextFireballBlast = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void InfernoAura()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Inferno Stallion radiates a fiery aura!*");
                PlaySound(0x227);
                FixedEffect(0x3709, 10, 16);

                // Aura effect (damaging nearby enemies over time)
                foreach (Mobile m in GetMobilesInRange(2))
                {
                    if (m != this && m != Combatant)
                    {
                        int damage = Utility.RandomMinMax(5, 10);
                        m.Damage(damage, this);
                    }
                }

                m_NextInfernoAura = DateTime.UtcNow + TimeSpan.FromSeconds(45);
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
