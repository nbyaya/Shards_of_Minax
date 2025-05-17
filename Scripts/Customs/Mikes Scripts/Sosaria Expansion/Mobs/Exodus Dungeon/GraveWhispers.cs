using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("the hollow remains of Grave Whispers")]
    public class GraveWhispers : BaseCreature
    {
        private DateTime m_NextEcho;
        private DateTime m_NextPossession;
        private DateTime m_NextTorment;
        private DateTime m_NextPhaseShift;
        private bool m_ShiftedPhase;

        [Constructable]
        public GraveWhispers()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Grave Whispers";
            Title = "the Bound Echo";
            Body = 722;
            Hue = 2309; // Unnatural pale glow
            BaseSoundID = 0x9E;

            SetStr(900, 1100);
            SetDex(1200, 1400);
            SetInt(1400, 1600);

            SetHits(18000, 20000);
            SetMana(8000);
            SetStam(4000);

            SetDamage(30, 42);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 25, 40);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.SpiritSpeak, 120.0);
            SetSkill(SkillName.Necromancy, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 4);
            AddLoot(LootPack.Gems, 6);
        }

        public override bool AutoDispel => true;
        public override bool BardImmune => true;
        public override bool AlwaysMurderer => true;
        public override Poison HitPoison => Poison.Deadly;
        public override int Meat => 1;

        public override int GetIdleSound() => 1609;
        public override int GetAngerSound() => 1606;
        public override int GetHurtSound() => 1608;
        public override int GetDeathSound() => 1607;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && !target.Deleted && target.Map == this.Map && InRange(target, 12))
            {
                if (DateTime.UtcNow >= m_NextEcho)
                {
                    EchoDespair(target);
                }

                if (DateTime.UtcNow >= m_NextPossession)
                {
                    AttemptPossession(target);
                }

                if (DateTime.UtcNow >= m_NextTorment)
                {
                    PsychicTorment(target);
                }

                if (DateTime.UtcNow >= m_NextPhaseShift)
                {
                    PhaseShift();
                }
            }
        }

        private void EchoDespair(Mobile target)
        {
            target.SendMessage(0x22, "*Whispers coil around your mind... your will begins to erode.*");
            target.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Head);
            AOS.Damage(target, this, Utility.RandomMinMax(20, 40), 0, 0, 0, 0, 100);

            m_NextEcho = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
        }

        private void AttemptPossession(Mobile target)
        {
            if (Utility.RandomDouble() < 0.33) // 33% chance
            {
                target.SendMessage(0x22, "*A cold presence tries to take hold of your body!*");
                target.FixedParticles(0x375A, 10, 30, 5011, EffectLayer.Waist);

                if (Utility.RandomDouble() < 0.25) // Possession succeeds
                {
                    target.Frozen = true;
                    target.SendMessage(0x20, "You are paralyzed by a ghostly force!");

                    Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                    {
                        target.Frozen = false;
                        target.SendMessage(0x22, "You shake off the possessing force.");
                    });
                }
            }

            m_NextPossession = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void PsychicTorment(Mobile target)
        {
            Effects.PlaySound(target.Location, target.Map, 0x1F7);
            target.FixedEffect(0x37C4, 10, 16);

            int mindDamage = Utility.RandomMinMax(10, 25);
            target.Mana = Math.Max(0, target.Mana - mindDamage);
            target.SendMessage(0x22, "Your thoughts scream in anguish!");

            m_NextTorment = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
        }

        private void PhaseShift()
        {
            m_ShiftedPhase = !m_ShiftedPhase;

            if (m_ShiftedPhase)
            {
                Hue = 1175; // Ghostly blue
                Hidden = true;
                Say("*Grave Whispers fades into the veil...*");
            }
            else
            {
                Hue = 2309; // Original pale hue
                Hidden = false;
                Say("*Grave Whispers reemerges with a soul-rending scream!*");
                PlaySound(0x229);
            }

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker != null && !attacker.Deleted && Utility.RandomDouble() < 0.20)
            {
                AOS.Damage(attacker, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 100);
                attacker.SendMessage(0x22, "Your soul recoils from the spectral backlash!");
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Chance to drop a rare cursed item
            if (Utility.RandomDouble() < 0.02)
                c.DropItem(new WhisperingSkull()); // Custom item
        }

        public GraveWhispers(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_ShiftedPhase = false;
        }
    }

    public class WhisperingSkull : Item
    {
        [Constructable]
        public WhisperingSkull() : base(0x1AE0)
        {
            Name = "Whispering Skull";
            Hue = 1175;
            Weight = 1.0;
        }

        public WhisperingSkull(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
