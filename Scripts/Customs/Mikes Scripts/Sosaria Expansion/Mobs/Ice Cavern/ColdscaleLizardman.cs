using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a coldscale lizardman corpse")]
    public class ColdscaleLizardman : BaseCreature
    {
        private DateTime m_NextIceSpike;
        private DateTime m_NextFrozenGaze;
        private DateTime m_NextFrostShield;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public ColdscaleLizardman()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Coldscale Lizardman";
            Body = Utility.RandomList(35, 36);
            BaseSoundID = 417;
            Hue = 1152; // Icy blue

            SetStr(180, 220);
            SetDex(120, 150);
            SetInt(100, 120);

            SetHits(400, 500);

            SetDamage(12, 18);
            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 85.0, 110.0);
            SetSkill(SkillName.Wrestling, 85.0, 105.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 50;

            m_AbilitiesInitialized = false;
        }

        public override bool AutoDispel => true;
        public override int TreasureMapLevel => 3;
        public override bool CanRummageCorpses => true;

        public override int Meat => 1;
        public override int Hides => 16;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, 2);

            if (Utility.RandomDouble() < 0.015) // 1.5% rare drop
            {
                this.PackItem(new ColdscaleHeart());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    var rand = new Random();
                    m_NextIceSpike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(3, 10));
                    m_NextFrozenGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(8, 15));
                    m_NextFrostShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextIceSpike)
                    CastIceSpike();

                if (DateTime.UtcNow >= m_NextFrozenGaze)
                    FrozenGaze();

                if (DateTime.UtcNow >= m_NextFrostShield)
                    ActivateFrostShield();
            }
        }

        private void CastIceSpike()
        {
            if (Combatant is Mobile target && !target.Deleted && target.Alive)
            {
                PublicOverheadMessage(MessageType.Emote, 0x480, true, "*hurls icy spikes from its claws*");
                target.SendMessage(0x480, "You are struck by icy shards!");
                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 0, 0, 0, 100); // Cold damage
                target.Frozen = true;
                Timer.DelayCall(TimeSpan.FromSeconds(2), () => target.Frozen = false);
                m_NextIceSpike = DateTime.UtcNow + TimeSpan.FromSeconds(10 + Utility.Random(5));
            }
        }

        private void FrozenGaze()
        {
            if (Combatant is Mobile target && !target.Deleted && target.Alive)
            {
                PublicOverheadMessage(MessageType.Emote, 0x481, true, "*locks eyes with chilling intent*");
                target.SendMessage(0x481, "You are frozen in fear!");
                target.Freeze(TimeSpan.FromSeconds(2));
                m_NextFrozenGaze = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.Random(10));
            }
        }

        private void ActivateFrostShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x481, true, "*a frost shield shimmers over its scales*");

            VirtualArmor += 40;
            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                VirtualArmor -= 40;
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*the frost shield dissipates*");
            });

            m_NextFrostShield = DateTime.UtcNow + TimeSpan.FromSeconds(45 + Utility.Random(15));
        }

        public ColdscaleLizardman(Serial serial) : base(serial)
        {
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

    public class ColdscaleHeart : Item
    {
        [Constructable]
        public ColdscaleHeart() : base(0x1CF1)
        {
            Name = "Coldscale Heart";
            Hue = 1152;
            Weight = 1.0;
        }

        public ColdscaleHeart(Serial serial) : base(serial)
        {
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
        }
    }
}
