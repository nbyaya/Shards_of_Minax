using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a jellybean jester corpse")]
    public class JellybeanJester : BaseCreature
    {
        private DateTime m_NextJellyJuggle;
        private DateTime m_NextColorfulConfusion;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public JellybeanJester()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Jellybean Jester";
            Body = 0xCF; // Sheep body
            Hue = 2349; // Unique hue for Jellybean Jester
			BaseSoundID = 0xD6;

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

        public JellybeanJester(Serial serial)
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
                    m_NextJellyJuggle = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextColorfulConfusion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextJellyJuggle)
                {
                    JellyJuggle();
                }

                if (DateTime.UtcNow >= m_NextColorfulConfusion)
                {
                    ColorfulConfusion();
                }
            }
        }

        private void JellyJuggle()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Jellybean Jester makes a mess with bouncing jellybeans!*");
            PlaySound(0x24F); // Play sound effect for jellybean throw

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant && m.Alive && CanBeHarmful(m))
                {
                    // Damage each target
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 0, 100, 0, 0);
                    
                    // Create a splash effect
                    Effects.SendLocationEffect(m.Location, m.Map, 0x36D4, 10, 1); // Jellybean splash
                    m.SendMessage("You are hit by a bouncing jellybean explosion!");

                    // Additional jellybean explosion sound
                    PlaySound(0x24F);
                }
            }

            m_NextJellyJuggle = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for JellyJuggle
        }

        private void ColorfulConfusion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Jellybean Jester throws colorful jellybeans causing confusion!*");
            PlaySound(0x24F); // Play sound effect for jellybean throw

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("You are confused by the colorful jellybeans!");

                    // Confuse the target to attack random creatures
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        if (m.Alive)
                        {

                            // Chance of paralysis effect
                            if (Utility.RandomDouble() < 0.25) // 25% chance
                            {
                                m.Freeze(TimeSpan.FromSeconds(5)); // Freeze the target for 5 seconds
                                m.SendMessage("You are frozen by the colorful jellybeans!");
                            }
                        }
                    });
                }
            }

            m_NextColorfulConfusion = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ColorfulConfusion
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
