using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a peppermint puff corpse")]
    public class PeppermintPuff : BaseCreature
    {
        private DateTime m_NextPeppermintMist;
        private DateTime m_NextMintyChill;
        private DateTime m_NextPeppermintBlast;
        private DateTime m_NextPeppermintSwirl;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public PeppermintPuff()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Peppermint Puff";
            Body = 0xCF; // Sheep body
            Hue = 2344; // Unique minty hue
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

        public PeppermintPuff(Serial serial)
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
                    m_NextPeppermintMist = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextMintyChill = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextPeppermintBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 35));
                    m_NextPeppermintSwirl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextPeppermintMist)
                {
                    PeppermintMist();
                }

                if (DateTime.UtcNow >= m_NextMintyChill)
                {
                    MintyChill();
                }

                if (DateTime.UtcNow >= m_NextPeppermintBlast)
                {
                    PeppermintBlast();
                }

                if (DateTime.UtcNow >= m_NextPeppermintSwirl)
                {
                    PeppermintSwirl();
                }
            }
        }

        private void PeppermintMist()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Peppermint Puff confuses its foes with a cloud of minty mist!*");
            PlaySound(0x20E); // Mist sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.SendMessage("You are surrounded by a confusing cloud of peppermint mist!");

                    // Cause targets to randomly move
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => StartConfusion(m));
                }
            }

            m_NextPeppermintMist = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for PeppermintMist
        }

        private void StartConfusion(Mobile target)
        {
            if (target == null || !target.Alive || !CanBeHarmful(target))
                return;

            Timer.DelayCall(TimeSpan.Zero, () =>
            {
                if (target.Alive)
                {
                    target.SendMessage("The peppermint mist causes you to move erratically!");

                    Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                    {
                        if (target.Alive)
                            target.MoveToWorld(new Point3D(target.X + Utility.Random(-2, 5), target.Y + Utility.Random(-2, 5), target.Z), target.Map);
                    });
                }
            });
        }

        private void MintyChill()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Peppermint Puff releases a chilling blast!*");
            PlaySound(0x20F); // Chill sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.SendMessage("You are struck by a chilling blast!");

                    // Reduce movement speed by 50%
                    m.SendMessage("You feel your movement slow down!");
                    m.Send(new SpeedChangePacket(m, 0.5)); // Assume this method changes movement speed
                }
            }

            m_NextMintyChill = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for MintyChill
        }

        private void PeppermintBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Peppermint Puff unleashes a powerful peppermint blast!*");
            PlaySound(0x20D); // Blast sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.SendMessage("You are hit by a blast of peppermint energy!");

                    // Apply damage
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 100, 0, 0, 0);
                }
            }

            m_NextPeppermintBlast = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for PeppermintBlast
        }

        private void PeppermintSwirl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Peppermint Puff creates a swirling vortex of peppermint!*");
            PlaySound(0x20E); // Swirl sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.SendMessage("A swirling vortex of peppermint surrounds you!");

                    // Apply a knockback effect
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 0);
                    m.MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0); // Swirling particles effect
                }
            }

            m_NextPeppermintSwirl = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Cooldown for PeppermintSwirl
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

    public class SpeedChangePacket : Packet
    {
        public SpeedChangePacket(Mobile mobile, double speedMultiplier) : base(0xBF)
        {
            EnsureCapacity(22);
            m_Stream.Write((int)mobile.Serial.Value);
            m_Stream.Write((short)0x00); // Unknown
            m_Stream.Write((short)0x00); // Unknown
            m_Stream.Write((byte)(speedMultiplier * 100)); // Speed multiplier
        }
    }
}
