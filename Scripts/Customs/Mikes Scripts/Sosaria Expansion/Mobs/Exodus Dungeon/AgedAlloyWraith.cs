using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an aged alloy wraith corpse")]
    public class AgedAlloyWraith : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextPulseNova;
        private DateTime m_NextEchoCurse;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public AgedAlloyWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Aged Alloy Wraith";
            Body = 109; // Copper Elemental body
            BaseSoundID = 268;
            Hue = 2049; // Dark platinum shimmer

            SetStr(650, 800);
            SetDex(150, 200);
            SetInt(300, 400);

            SetHits(1000, 1300);
            SetStam(200, 250);
            SetMana(1200, 1500);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 70, 90);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 110.0);

            Fame = 26000;
            Karma = -26000;

            VirtualArmor = 70;

            m_AbilitiesInitialized = false;
        }

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override bool Unprovokable => true;
        public override int TreasureMapLevel => 5;
        public override Poison HitPoison => Poison.Lethal;
        public override bool AlwaysMurderer => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);
            if (Utility.RandomDouble() < 0.015)
                PackItem(new ClockworkSoulstone());
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Combatant.Deleted || !Combatant.Alive)
                return;

            if (!m_AbilitiesInitialized)
            {
                m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 25));
                m_NextPulseNova = DateTime.UtcNow + TimeSpan.FromSeconds(15);
                m_NextEchoCurse = DateTime.UtcNow + TimeSpan.FromSeconds(25);
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextPhaseShift)
                PhaseShift();

            if (DateTime.UtcNow >= m_NextPulseNova)
                PulseNova();

            if (DateTime.UtcNow >= m_NextEchoCurse)
                EchoCurse();
        }

        private void PhaseShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Aged Alloy Wraith flickers between dimensions! *");
            PlaySound(0x20C);
            Hidden = true;
            Blessed = true;

            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                Hidden = false;
                Blessed = false;
                PlaySound(0x208);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The wraith returns, partially phased! *");
            });

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 50));
        }

        private void PulseNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Aged Alloy Wraith emits an energy pulse! *");
            Effects.PlaySound(Location, Map, 0x5CE);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && !m.Hidden)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);
                    m.SendMessage("An arcane shockwave surges through your body!");
                }
            }

            m_NextPulseNova = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void EchoCurse()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Wraith echoes your own power against you! *");
                target.FixedParticles(0x375A, 1, 30, 9966, 3, 0, EffectLayer.CenterFeet);

                // Reflect damage based on Mana
                int reflected = target.Mana / 2;
                AOS.Damage(target, this, reflected, 0, 0, 100, 0, 0);
                target.SendMessage("You feel your magical energy being turned against you!");

                m_NextEchoCurse = DateTime.UtcNow + TimeSpan.FromSeconds(60);
            }
        }

        public AgedAlloyWraith(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class ClockworkSoulstone : Item
    {
        [Constructable]
        public ClockworkSoulstone() : base(0x1BFB)
        {
            Name = "a Clockwork Soulstone";
            Hue = 1153;
            Weight = 1.0;
            LootType = LootType.Cursed;
        }

        public ClockworkSoulstone(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
