using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shattered corpse")]
    public class ShatteredCorpse : BaseCreature
    {
        private DateTime m_NextShardBurst;
        private DateTime m_NextMirrorSpawn;
        private DateTime m_NextTorment;

        private bool m_InitializedAbilities;

        [Constructable]
        public ShatteredCorpse()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Shattered Corpse";
            Body = 8;
            BaseSoundID = 684;
            Hue = 2647; // ghostly green glow with violet shimmer

            SetStr(600, 750);
            SetDex(100, 130);
            SetInt(500, 650);

            SetHits(800, 1200);
            SetMana(1000);

            SetDamage(20, 28);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 30);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 40, 55);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 65);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 55, 70);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Tactics, 80.0, 95.0);
            SetSkill(SkillName.Wrestling, 70.0, 85.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            m_InitializedAbilities = false;
        }

        public override bool AutoDispel => true;
        public override bool Unprovokable => true;
        public override Poison PoisonImmune => Poison.Greater;
        public override bool BleedImmune => true;
        public override bool ReacquireOnMovement => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 4);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null)
                return;

            if (!m_InitializedAbilities)
            {
                Random rand = new Random();
                m_NextShardBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(5, 12));
                m_NextMirrorSpawn = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 30));
                m_NextTorment = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 25));
                m_InitializedAbilities = true;
            }

            if (DateTime.UtcNow >= m_NextShardBurst)
                ShardBurst();

            if (DateTime.UtcNow >= m_NextMirrorSpawn)
                MirrorSpawn();

            if (DateTime.UtcNow >= m_NextTorment)
                PsychicTorment();
        }

        private void ShardBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "*The Shattered Corpse explodes with razor-shards!*");
            PlaySound(0x654);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);

                    if (m is Mobile mobile)
                        mobile.SendMessage(38, "You are slashed by flying shards!");
                }
            }

            Effects.SendLocationEffect(Location, Map, 0x3709, 30);
            m_NextShardBurst = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void MirrorSpawn()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "*A mirrored husk tears itself from the corpse!*");
            PlaySound(0x1FB);

            Mobile mirror = new MirrorFragment(this);
            mirror.MoveToWorld(Location, Map);

            m_NextMirrorSpawn = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void PsychicTorment()
        {
            if (Combatant is Mobile target)
            {
                target.Mana -= Utility.RandomMinMax(10, 25);
                target.Stam -= Utility.RandomMinMax(15, 30);
                target.SendMessage(38, "Your mind reels with unbearable pain!");

                Effects.SendLocationEffect(target.Location, target.Map, 0x374A, 20, 10);
                PlaySound(0x1ED);

                m_NextTorment = DateTime.UtcNow + TimeSpan.FromSeconds(25);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.01)
            {
                c.DropItem(new ShardHeart());
            }
        }

        public ShatteredCorpse(Serial serial) : base(serial)
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
            m_InitializedAbilities = false;
        }
    }

    public class MirrorFragment : BaseCreature
    {
        [Constructable]
        public MirrorFragment(Mobile master)
            : base(AIType.AI_Melee, FightMode.Closest, 6, 1, 0.2, 0.4)
        {
            Name = "a mirror fragment";
            Body = 8;
            Hue = 1150; // shimmering gray
            BaseSoundID = 684;

            SetStr(100);
            SetDex(50);
            SetInt(10);

            SetHits(100);
            SetDamage(10, 15);

            SetResistance(ResistanceType.Physical, 30);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Wrestling, 60.0);
            SetSkill(SkillName.Tactics, 60.0);

            Fame = 0;
            Karma = 0;
            VirtualArmor = 18;

            Timer.DelayCall(TimeSpan.FromSeconds(15), Delete);
        }

        public MirrorFragment(Serial serial) : base(serial)
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

    public class ShardHeart : Item
    {
        [Constructable]
        public ShardHeart() : base(0x1CF0)
        {
            Name = "a crystalline shard heart";
            Hue = 1289;
            Weight = 1.0;
        }

        public ShardHeart(Serial serial) : base(serial)
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
