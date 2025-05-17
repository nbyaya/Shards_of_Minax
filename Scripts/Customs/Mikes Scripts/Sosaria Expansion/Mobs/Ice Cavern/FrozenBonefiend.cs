using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a frozen bonefiend corpse")]
    public class FrozenBonefiend : BaseCreature
    {
        private DateTime m_NextIceNova;
        private DateTime m_NextScream;
        private DateTime m_NextSpikes;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrozenBonefiend()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Frozen Bonefiend";
            Body = 308;
            Hue = 1150; // Icy blue hue
            BaseSoundID = 0x48D;


            SetStr(1200, 1400);
            SetDex(180, 220);
            SetInt(250, 300);

            SetHits(5000, 6500);
            SetStam(500);
            SetMana(3000);

            SetDamage(40, 45);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 70);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 50);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 90);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Anatomy, 100.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 80;

            m_AbilitiesInitialized = false;
        }

        public override bool AutoDispel => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 2);
            AddLoot(LootPack.Rich, 3);

            if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                PackItem(new IcyBoneShard()); // Custom item for lore/quest
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_AbilitiesInitialized && Combatant != null)
            {
                m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 30));
                m_NextScream = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 40));
                m_NextSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 35));
                m_AbilitiesInitialized = true;
            }

            if (DateTime.UtcNow >= m_NextIceNova)
                IceNova();

            if (DateTime.UtcNow >= m_NextScream)
                BoneScream();

            if (DateTime.UtcNow >= m_NextSpikes)
                BoneSpikes();
        }

        private void IceNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*The Frozen Bonefiend unleashes a chilling nova!*");
            PlaySound(0x10B); // Ice blast

            Effects.PlaySound(Location, Map, 0x64B);
            Effects.SendLocationEffect(Location, Map, 0x376A, 20);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != null && m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(3));

                    m.SendMessage(0x22, "You are frozen by a chilling wave!");
                }
            }

            m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 40));
        }

        private void BoneScream()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*The Bonefiend releases a soul-piercing scream!*");
            PlaySound(0x1FE); // Horrific scream

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != null && m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.FixedParticles(0x374A, 10, 15, 5013, 0x480, 2, EffectLayer.Head);
                    m.SendMessage(0x22, "A wave of despair weakens your soul!");
                    m.Stam -= Utility.RandomMinMax(10, 30);
                    m.Mana -= Utility.RandomMinMax(20, 40);
                }
            }

            m_NextScream = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 60));
        }

        private void BoneSpikes()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, false, "*Bone spikes erupt from the ground!*");
            PlaySound(0x5C3);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != null && m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 100, 0, 0, 0, 0);
                    if (Utility.RandomDouble() < 0.3)
                    {
                        m.Freeze(TimeSpan.FromSeconds(2));
                        m.SendMessage(0x22, "You are pierced and slowed by jagged frostbone!");
                    }
                }
            }

            m_NextSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 40));
        }

        public FrozenBonefiend(Serial serial) : base(serial) { }

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

    public class IcyBoneShard : Item
    {
        [Constructable]
        public IcyBoneShard() : base(0x1CF0)
        {
            Name = "an icy bone shard";
            Hue = 1150;
            Weight = 1.0;
            Movable = true;
        }

        public IcyBoneShard(Serial serial) : base(serial) { }

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
