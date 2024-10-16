using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a whispering pooka corpse")]
    public class WhisperingPooka : BaseCreature
    {
        private DateTime m_NextWhisperOfMadness;
        private DateTime m_NextDevouringSilence;
        private DateTime m_NextPuppeteersCharm;
        private DateTime m_NextEchoingWhispers;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public WhisperingPooka()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Whispering Pooka";
            Body = 205; // Rabbit body
            Hue = 2228; // Unique hue to stand out

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

        public WhisperingPooka(Serial serial)
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
        public override int GetAttackSound() 
        { 
            return 0xC9; 
        }

        public override int GetHurtSound() 
        { 
            return 0xCA; 
        }

        public override int GetDeathSound() 
        { 
            return 0xCB; 
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
                    m_NextWhisperOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDevouringSilence = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextPuppeteersCharm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextEchoingWhispers = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextWhisperOfMadness)
                {
                    WhisperOfMadness();
                }

                if (DateTime.UtcNow >= m_NextDevouringSilence)
                {
                    DevouringSilence();
                }

                if (DateTime.UtcNow >= m_NextPuppeteersCharm)
                {
                    PuppeteersCharm();
                }

                if (DateTime.UtcNow >= m_NextEchoingWhispers)
                {
                    EchoingWhispers();
                }
            }
        }

        private void WhisperOfMadness()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Whispering Pooka’s voice seeps into your mind!*");
            PlaySound(0x4F1); // Eerie whisper sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    if (Utility.RandomDouble() < 0.5) // 50% chance of effect
                    {
                        AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0); // Mental damage
                        m.SendMessage("You feel a surge of paranoia and hallucinations!");
                        m.FixedParticles(0x373A, 10, 30, 0, EffectLayer.Head); // Visual effect
                    }
                }
            }

            m_NextWhisperOfMadness = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for WhisperOfMadness
        }

        private void DevouringSilence()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Whispering Pooka casts Devouring Silence! *");
            PlaySound(0x4F1); // Eerie whisper sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m is PlayerMobile player)
                {
                    if (player.Spell != null) // Check if the player is casting a spell
                    {
                        player.SendMessage("You are silenced by the Whispering Pooka!");
                        // Simulate silence effect by applying a debuff or flag
                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => 
                        {
                            // Re-enable casting after 10 seconds
                            player.SendMessage("You regain control over your spells!");
                        });

                        // Knockback effect
                        Point3D newLocation = new Point3D(player.X + Utility.Random(-2, 2), player.Y + Utility.Random(-2, 2), player.Z);
                        player.Location = newLocation;
                        player.SendMessage("You are knocked back by the force of silence!");
                        player.MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 0, 0, 0x160, 0); // Knockback visual effect
                    }
                }
            }

            m_NextDevouringSilence = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DevouringSilence
        }

        private void PuppeteersCharm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Whispering Pooka uses Puppeteer’s Charm! *");
            PlaySound(0x4F1); // Eerie whisper sound

            Mobile target = Combatant as Mobile;
            if (target != null && target is PlayerMobile player)
            {
                if (Utility.RandomDouble() < 0.3) // 30% chance of effect
                {
                    player.SendMessage("You are under the Whispering Pooka's control!");
                    Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                    {
                        if (player.Alive && !player.IsDeadBondedPet)
                        {
                            player.SendMessage("You regain control of yourself!");
                        }
                    });

                    // Simulate control
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (player.Alive && !player.IsDeadBondedPet)
                        {
                            // Move player randomly within a small range
                            Point3D newLocation = new Point3D(player.X + Utility.Random(-2, 2), player.Y + Utility.Random(-2, 2), player.Z);
                            player.Location = newLocation;
                            player.SendMessage("You are controlled by the Pooka and move erratically!");
                        }
                    });

                    // Add a visual effect for control
                    player.FixedParticles(0x373A, 10, 30, 0, EffectLayer.Head);
                }
            }

            m_NextPuppeteersCharm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for PuppeteersCharm
        }

        private void EchoingWhispers()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Echoing Whispers fill the air! *");
            PlaySound(0x4F1); // Eerie whisper sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    if (Utility.RandomDouble() < 0.3) // 30% chance of effect
                    {
                        m.SendMessage("You are overwhelmed by echoing whispers and fear!");
                        m.FixedParticles(0x373A, 10, 30, 0, EffectLayer.Head); // Visual effect
                        AOS.Damage(m, this, Utility.RandomMinMax(5, 15), 0, 0, 100, 0, 0); // Fear damage
                        // Optionally, you can apply a fear debuff or other effects here
                    }
                }
            }

            m_NextEchoingWhispers = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown for EchoingWhispers
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
