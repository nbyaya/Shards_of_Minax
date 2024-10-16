using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a sombrero de sol llama corpse")]
    public class SombreroDeSolLlama : BaseCreature
    {
        private DateTime m_NextSolarFlare;
        private DateTime m_NextSunbeam;
        private DateTime m_NextDesertHeat;
        private DateTime m_NextSunburst;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SombreroDeSolLlama()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a Sombrero de Sol Llama";
            Body = 0xDC; // Llama body
            Hue = 2153; // Unique hue for the llama with a radiant appearance
			this.BaseSoundID = 0x3F3;

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

            m_AbilitiesInitialized = false;
        }

        public SombreroDeSolLlama(Serial serial)
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
                    m_NextSolarFlare = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSunbeam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextDesertHeat = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_NextSunburst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextSolarFlare)
                {
                    SolarFlare();
                }

                if (DateTime.UtcNow >= m_NextSunbeam)
                {
                    Sunbeam();
                }

                if (DateTime.UtcNow >= m_NextDesertHeat)
                {
                    DesertHeat();
                }

                if (DateTime.UtcNow >= m_NextSunburst)
                {
                    Sunburst();
                }
            }
        }

        private void SolarFlare()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero de Sol Llama unleashes a dazzling Solar Flare! *");
            PlaySound(0x225); // Blinding flash sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("You are blinded by the Solar Flare and struggle to see!");
                    // Temporary accuracy reduction effect here if applicable
                    m.SendMessage("Your accuracy is reduced!");
                }
            }

            m_NextSolarFlare = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for Solar Flare
        }

        private void Sunbeam()
        {
            if (Combatant != null)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero de Sol Llama fires a scorching Sunbeam! *");
                PlaySound(0x225); // Sunbeam sound

                int damage = Utility.RandomMinMax(40, 60);
                Combatant.Damage(damage, this); // Using Combatant.Damage(damage, attacker)

                Combatant.PlaySound(0x208); // Fire sound

                m_NextSunbeam = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for Sunbeam
            }
        }

        private void DesertHeat()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero de Sol Llama radiates intense Desert Heat! *");
            PlaySound(0x225); // Desert heat sound

            // Increase fire resistance
            this.SetResistance(ResistanceType.Fire, 80, 100);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    m.Damage(damage, this); // Using m.Damage(damage, attacker)

                    m.SendMessage("You are scorched by the intense heat!");
                    m.PlaySound(0x208); // Fire sound
                }
            }

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                // Reset fire resistance after 10 seconds
                this.SetResistance(ResistanceType.Fire, 60, 70);
            });

            m_NextDesertHeat = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Cooldown for Desert Heat
        }

        private void Sunburst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sombrero de Sol Llama conjures a radiant Sunburst! *");
            PlaySound(0x225); // Radiant burst sound

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    m.Damage(damage, this); // Using m.Damage(damage, attacker)

                    m.SendMessage("You are engulfed by a brilliant Sunburst!");
                    m.MovingParticles(this, 0x36BD, 10, 0, false, true, 0x1F4, 0, 0, 0, 0x160, 0); // Particle effect
                }
            }

            m_NextSunburst = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for Sunburst
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
