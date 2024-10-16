using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Nemesis Unit corpse")]
    public class NemesisUnit : BaseCreature
    {
        private DateTime m_NextVengefulStrike;
        private DateTime m_NextInfernoMissiles;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public NemesisUnit()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Nemesis Unit";
            Body = 0x2F5; // ExodusMinion body
            BaseSoundID = 0x2F8;
            Hue = 2273; // Dark red hue

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

        public NemesisUnit(Serial serial)
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

        public override bool BardImmune { get { return !Core.AOS; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }


        public override int GetIdleSound()
        {
            return 0x2CC;
        }

        public override int GetAttackSound()
        {
            return 0x2C8;
        }

        public override int GetDeathSound()
        {
            return 0x2C9;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextVengefulStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextInfernoMissiles = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVengefulStrike)
                {
                    DoVengefulStrike();
                }

                if (DateTime.UtcNow >= m_NextInfernoMissiles)
                {
                    DoInfernoMissiles();
                }
            }
        }

        private void DoVengefulStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive && target.InRange(this, 1))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Vengeful Strike *");
                PlaySound(0x1E7);

                int damage = Utility.RandomMinMax(30, 40);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                target.FixedParticles(0x37B9, 10, 15, 5012, EffectLayer.Waist);

                if (Utility.RandomDouble() < 0.3)
                {
                    target.SendLocalizedMessage(1060167); // You have been hit by a crushing blow!
                    target.PlaySound(0x1E1);
                    target.Freeze(TimeSpan.FromSeconds(2));
                    BleedAttack.BeginBleed(target, this, true); // Use the correct boolean value as needed
                }

                Random rand = new Random();
                m_NextVengefulStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40)); // Randomized cooldown
            }
        }

        private void DoInfernoMissiles()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Inferno Missiles *");
            PlaySound(0x64E);

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), FireMissile);
            }

            Random rand = new Random();
            m_NextInfernoMissiles = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60)); // Randomized cooldown
        }

        private void FireMissile()
        {
            if (Combatant == null)
                return;

            DoHarmful(Combatant);
            MovingParticles(Combatant, 0x36D4, 7, 0, false, true, 0, 0, 9502, 4019, 0x160, 0);
            PlaySound(0x15E);

            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (Combatant != null && Combatant.Alive)
                {
                    AOS.Damage(Combatant, this, Utility.RandomMinMax(25, 35), 0, 100, 0, 0, 0);
                    Combatant.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    Combatant.PlaySound(0x208);
                    ApplyBurningEffect(Combatant as Mobile);
                }
            });
        }

        private void ApplyBurningEffect(Mobile target)
        {
            if (target == null)
                return;

            Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), 5, () =>
            {
                if (target.Alive)
                {
                    target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                }
            });
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
            // Randomize the ability start times again
            Random rand = new Random();
            m_NextVengefulStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 20));
            m_NextInfernoMissiles = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 30));
        }
    }
}
