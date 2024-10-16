using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a flame warden ettin corpse")]
    public class FlameWardenEttin : BaseCreature
    {
        private DateTime m_NextInfernoBreath;
        private DateTime m_NextFireShield;
        private DateTime m_BlazingFuryEnd;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FlameWardenEttin()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a flame warden ettin";
            Body = 18;
            BaseSoundID = 367;
            Hue = 1562; // Fiery red-orange hue

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public FlameWardenEttin(Serial serial)
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

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 4; } }


        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextInfernoBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextFireShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextInfernoBreath)
                {
                    InfernoBreath();
                }

                if (DateTime.UtcNow >= m_NextFireShield)
                {
                    FireShield();
                }

                if (GetHealthPercentage() < 50 && m_BlazingFuryEnd == DateTime.MinValue)
                {
                    BlazingFury();
                }
            }

            if (DateTime.UtcNow >= m_BlazingFuryEnd && m_BlazingFuryEnd != DateTime.MinValue)
            {
                EndBlazingFury();
            }
        }

        private int GetHealthPercentage()
        {
            if (HitsMax <= 0)
                return 0;
            return (Hits * 100) / HitsMax;
        }

        private void InfernoBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flame Warden Ettin unleashes its Inferno Breath! *");
            PlaySound(0x108);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);

                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            // Launch fireball at a random target
            if (Combatant != null && Combatant is Mobile)
            {
                Direction = GetDirectionTo(Combatant);
                MovingParticles(Combatant, 0x36D4, 7, 0, false, true, 0x496, 0, 9502, 4019, 0x160, 0);
                Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerStateCallback(FireballImpact), Combatant);
            }

            m_NextInfernoBreath = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Set cooldown after use
        }

        private void FireballImpact(object state)
        {
            if (state is Mobile target && target.Alive)
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(40, 60), 0, 100, 0, 0, 0);
                target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            }
        }

        private void FireShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flame Warden Ettin creates a Fire Shield! *");
            PlaySound(0x1DD);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.FromSeconds(10), new TimerCallback(EndFireShield));
            m_NextFireShield = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown after use
        }

        private void EndFireShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Fire Shield dissipates *");
        }

        private void BlazingFury()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flame Warden Ettin enters a Blazing Fury! *");
            PlaySound(0x15F);
            FixedParticles(0x376A, 10, 15, 5052, EffectLayer.Waist);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 75);

            double damageIncrease = 1.5;
            SetDamage((int)(DamageMin * damageIncrease), (int)(DamageMax * damageIncrease));

            m_BlazingFuryEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set duration for Blazing Fury
        }

        private void EndBlazingFury()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Flame Warden Ettin's fury subsides *");

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            double damageDecrease = 1 / 1.5;
            SetDamage((int)(DamageMin * damageDecrease), (int)(DamageMax * damageDecrease));

            m_BlazingFuryEnd = DateTime.MinValue; // Reset the end time
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_NextFireShield <= DateTime.UtcNow && from != null && from != this && from.Alive && !from.IsDeadBondedPet && 0.1 > Utility.RandomDouble())
            {
                AOS.Damage(from, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                from.SendLocalizedMessage(1008112); // The intense heat is damaging you!
                from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                from.PlaySound(0x1DD);
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

            // Reset initialization flag and cooldowns
            m_AbilitiesInitialized = false;
            m_NextInfernoBreath = DateTime.UtcNow;
            m_NextFireShield = DateTime.UtcNow;
            m_BlazingFuryEnd = DateTime.MinValue;
        }
    }
}
