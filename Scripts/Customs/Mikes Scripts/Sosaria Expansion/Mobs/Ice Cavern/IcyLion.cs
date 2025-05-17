using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("an icy lion corpse")]
    public class IcyLion : BaseCreature
    {
        private DateTime m_NextFrostRoar;
        private DateTime m_NextIcePulse;
        private DateTime m_NextCrystalArmor;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public IcyLion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Icy Lion";
            Body = 251;
            Hue = 1152; // Pale blue frost hue
            BaseSoundID = 0x518;

            SetStr(600, 700);
            SetDex(180, 200);
            SetInt(250, 300);

            SetHits(1000, 1200);

            SetDamage(28, 38);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 50, 65);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 22000;
            Karma = -22000;

            VirtualArmor = 80;

            m_AbilitiesInitialized = false;
        }

        public IcyLion(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override int GetAngerSound() => 0x518;
        public override int GetIdleSound() => 0x517;
        public override int GetAttackSound() => 0x516;
        public override int GetHurtSound() => 0x519;
        public override int GetDeathSound() => 0x515;

        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 2);

            if (Utility.RandomDouble() < 0.005) // 1 in 200
                PackItem(new IcyLionFang()); // A unique icy loot item
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextFrostRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 25));
                    m_NextIcePulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 30));
                    m_NextCrystalArmor = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(60, 90));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostRoar)
                    FrostRoar();

                if (DateTime.UtcNow >= m_NextIcePulse)
                    IcePulse();

                if (DateTime.UtcNow >= m_NextCrystalArmor)
                    CrystalArmor();
            }
        }

        private void FrostRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Icy Lion lets out a deafening frost-roar!*");
            PlaySound(0x518);

            if (Combatant is Mobile target)
            {
                target.Freeze(TimeSpan.FromSeconds(2));
                target.SendMessage(0x480, "Your limbs stiffen as freezing air rushes over you!");
                AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
            }

            m_NextFrostRoar = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 35));
        }

        private void IcePulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*A chilling pulse radiates from the Icy Lion's core!*");
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x3709, 10, 30, Hue, 0, 5044, 0);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    m.SendMessage(0x480, "Frost pulses through your veins!");
                }
            }

            m_NextIcePulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 40));
        }

        private void CrystalArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x47F, true, "*The Icy Lion's body crystallizes, hardening its defenses!*");
            VirtualArmor += 40;

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                PublicOverheadMessage(MessageType.Regular, 0x47F, true, "*The crystalline armor shatters!*");
                VirtualArmor -= 40;
            });

            m_NextCrystalArmor = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(60, 90));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class IcyLionFang : Item
    {
        [Constructable]
        public IcyLionFang() : base(0x1BD1) // A unique item appearance
        {
            Name = "Fang of the Icy Lion";
            Hue = 1152;
            Weight = 1.0;
        }

        public IcyLionFang(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
