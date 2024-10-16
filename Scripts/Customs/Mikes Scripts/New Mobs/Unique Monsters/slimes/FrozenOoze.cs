using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a frozen ooze corpse")]
    public class FrozenOoze : BaseCreature
    {
        private DateTime m_NextGlacialWave;
        private DateTime m_NextIceArmor;
        private DateTime m_NextFrostShards;
        private DateTime m_NextCryogenicBurst;
        private bool m_IceArmorActive;

        [Constructable]
        public FrozenOoze()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frozen ooze";
            Body = 51; // Slime body
            Hue = 2385; // Unique ice hue
			BaseSoundID = 456;

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
            m_IceArmorActive = false;
        }

        public FrozenOoze(Serial serial)
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
                // Ability cooldowns
                if (DateTime.UtcNow >= m_NextGlacialWave)
                {
                    GlacialWave();
                }

                if (DateTime.UtcNow >= m_NextIceArmor && !m_IceArmorActive)
                {
                    IceArmor();
                }

                if (DateTime.UtcNow >= m_NextFrostShards)
                {
                    FrostShards();
                }

                if (DateTime.UtcNow >= m_NextCryogenicBurst)
                {
                    CryogenicBurst();
                }

                if (m_IceArmorActive && DateTime.UtcNow >= m_NextIceArmor.AddSeconds(10))
                {
                    DeactivateIceArmor();
                }
            }

            // Update cooldowns
            if (DateTime.UtcNow >= m_NextGlacialWave)
            {
                m_NextGlacialWave = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }

            if (DateTime.UtcNow >= m_NextIceArmor)
            {
                m_NextIceArmor = DateTime.UtcNow + TimeSpan.FromSeconds(60);
            }

            if (DateTime.UtcNow >= m_NextFrostShards)
            {
                m_NextFrostShards = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }

            if (DateTime.UtcNow >= m_NextCryogenicBurst)
            {
                m_NextCryogenicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(60);
            }
        }

        private void GlacialWave()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frozen Ooze unleashes a powerful wave of cold! *");
            PlaySound(0x0C4); // Cold sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 0, 100, 0, 0); // Cold damage
                    m.SendMessage("You are hit by a freezing wave!");
                    m.Frozen = true; // Freeze the target
                    Timer.DelayCall(TimeSpan.FromSeconds(3), () => m.Frozen = false); // Unfreeze after 3 seconds
                    m.SendMessage("You feel the chill of the cold wave slowing your movements!");
                    m.SendLocalizedMessage(1114727); // The burst of bubbles knocks you back!
                    m.MovingParticles(this, 0x373A, 10, 0, false, true, 0x1F4, 0, 3006, 4006, 0x160, 0); // Wave particles
                }
            }
        }

        private void IceArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frozen Ooze encases itself in a protective layer of ice! *");
            PlaySound(0x1D8); // Ice shield sound

            FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            VirtualArmor += 30; // Increase virtual armor
            m_IceArmorActive = true;


            m_NextIceArmor = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Duration
        }

        private void DeactivateIceArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The ice armor around the Frozen Ooze fades away! *");
            VirtualArmor -= 30; // Reset virtual armor
            m_IceArmorActive = false;

            // Restore movement speed
            ActiveSpeed = 1.0; // Normal active speed
            PassiveSpeed = 1.0; // Normal passive speed
        }

        private void FrostShards()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frozen Ooze sends shards of ice flying in all directions! *");
            PlaySound(0x1C8); // Ice shard sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 15), 0, 0, 100, 0, 0); // Cold damage
                    m.SendMessage("You are struck by icy shards!");
                    m.Frozen = true; // Slow the target
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => m.Frozen = false); // Unfreeze after 2 seconds
                    m.SendLocalizedMessage(1114728); // Your attack is deflected by the bubble shield!
                }
            }
        }

        private void CryogenicBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frozen Ooze releases a devastating cryogenic burst! *");
            PlaySound(0x0C5); // Burst sound

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 40), 0, 0, 100, 0, 0); // Massive cold damage
                    m.SendMessage("You are engulfed by a cryogenic burst!");
                    m.SendLocalizedMessage(1114729); // The cryogenic burst has left you chilled to the bone!
                }
            }

            // After the burst, reduce damage output and apply a cooldown
            this.SetDamage(10, 15); // Reduced damage output
            m_NextCryogenicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(120); // Cooldown
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            // Freezing Touch
            if (Utility.RandomDouble() < 0.2) // 20% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Frozen Ooze's touch freezes you in place! *");
                from.Frozen = true; // Freeze the attacker
                Timer.DelayCall(TimeSpan.FromSeconds(3), () => from.Frozen = false); // Unfreeze after 3 seconds
                Effects.SendTargetEffect(from, 0x379A, 10); // Frost effect
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
        }
    }
}
