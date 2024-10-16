using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a nightmare leaper corpse")]
    public class NightmareLeaper : BaseCreature
    {
        private DateTime m_NextShadowLeap;
        private DateTime m_NextNightmareVision;
        private DateTime m_NextDreamEater;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public NightmareLeaper()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Nightmare Leaper";
            Body = 205; // Rabbit body
            Hue = 2229; // Dark aura hue

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

        public NightmareLeaper(Serial serial)
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
                    Random rand = new Random();
                    m_NextShadowLeap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextNightmareVision = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDreamEater = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowLeap)
                {
                    ShadowLeap();
                }

                if (DateTime.UtcNow >= m_NextNightmareVision)
                {
                    NightmareVision();
                }

                if (DateTime.UtcNow >= m_NextDreamEater)
                {
                    DreamEater();
                }
            }
        }

        private void ShadowLeap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Nightmare Leaper vanishes into shadows and strikes with a surprise attack! *");
            PlaySound(0x207); // Shadow sound

            // Create a teleport effect
            Effects.SendLocationEffect(Location, Map, 0x376A, 10, 1);
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                // Teleport to a new location within 5 tiles
                Point3D targetLocation = new Point3D(
                    Location.X + Utility.RandomMinMax(-5, 5),
                    Location.Y + Utility.RandomMinMax(-5, 5),
                    Location.Z
                );

                MoveToWorld(targetLocation, Map);
                Effects.SendLocationEffect(Location, Map, 0x376A, 10, 1);
                if (Combatant != null && Utility.RandomDouble() < 0.75) // 75% chance to attack after teleport
                {
                    Attack(Combatant);
                }
            });

            m_NextShadowLeap = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for ShadowLeap
        }

		private void NightmareVision()
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Nightmare Leaper unleashes a terrifying vision! *");
			PlaySound(0x22F); // Fear sound

			foreach (Mobile m in GetMobilesInRange(5))
			{
				if (m != this && m.Alive && CanBeHarmful(m))
				{
					DoHarmful(m);
					m.Damage(Utility.RandomMinMax(10, 20)); // Fear damage
					m.SendMessage("You are overwhelmed by a nightmarish vision!");

					// Apply debuff
					Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
					{
						m.SendMessage("The effects of the nightmare vision wear off.");
					});

					m.SendMessage("Your attack and defense are reduced!");
					// Apply custom debuff logic
					// For example, reduce the mobile's skills
					m.RawDex -= 10; // Reduce dexterity
					m.RawStr -= 10; // Reduce strength
				}
			}

			m_NextNightmareVision = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for NightmareVision
		}


        private void DreamEater()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Nightmare Leaper consumes the dreams of nearby beings! *");
            PlaySound(0x1A9); // Energy drain sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int manaDrained = Utility.RandomMinMax(10, 20);
                    m.Mana -= manaDrained; // Drain mana
                    m.SendMessage($"You feel your energy being drained for {manaDrained} points!");
                    
                    // Chance to confuse or disorient
                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.SendMessage("You feel disoriented by the dream drain!");
                        m.SendLocalizedMessage(1049464); // You are disoriented!
                        m.Paralyze(TimeSpan.FromSeconds(5)); // Paralyze for 5 seconds
                    }
                }
            }

            m_NextDreamEater = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for DreamEater
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
