using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a glacial executioner corpse")]
    public class GlacialExecutioner : BaseCreature
    {
        private DateTime m_NextIceNova;
        private DateTime m_NextFrozenStare;
        private DateTime m_NextFrostShock;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GlacialExecutioner()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Glacial Executioner";
            Title = "Avatar of the Frost Queen";
            Body = 0x190; // Same humanoid base
            BaseSoundID = 0x482; // Cold wind/golem type sounds
            Hue = 1150; // Frosty, icy blue custom hue

            SetStr(750, 850);
            SetDex(125, 145);
            SetInt(300, 350);

            SetHits(1200, 1600);
            SetStam(200, 250);
            SetMana(2000, 2500);

            SetDamage(25, 35);
            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 45, 60);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 95.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 75;
        }

        public GlacialExecutioner(Serial serial) : base(serial) { }

        public override bool AutoDispel => true;
        public override bool Unprovokable => true;
        public override bool BardImmune => true;
        public override Poison HitPoison => Poison.Regular;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Gems, 6);

            if (Utility.RandomDouble() < 0.015)
                PackItem(new IceboundRelic());

        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(5, 15));
                    m_NextFrozenStare = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextFrostShock = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextIceNova)
                    IceNova();

                if (DateTime.UtcNow >= m_NextFrozenStare)
                    FrozenStare();

                if (DateTime.UtcNow >= m_NextFrostShock)
                    FrostShock();
            }
        }

        private void IceNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* releases a chilling nova *");
            PlaySound(0x10B); // Ice shatter sound
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x375A, 10, 30, Hue, 0, 5029, 0);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);

                    if (m is Mobile target)
                    {
                        target.SendMessage(0x480, "You're blasted by freezing energy!");
                        target.Freeze(TimeSpan.FromSeconds(2));
                    }
                }
            }

            m_NextIceNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 15));
        }

        private void FrozenStare()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* locks eyes with its victim *");
            PlaySound(0x1FB); // Haunting hum

            if (Combatant is Mobile target)
            {
                target.Freeze(TimeSpan.FromSeconds(3));
                target.SendMessage(0x480, "Your body freezes under the Glacial Executioner's stare!");
            }

            m_NextFrozenStare = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(12, 18));
        }

        private void FrostShock()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "* unleashes a Frost Shock *");
            PlaySound(0x64C); // Lightning/ice crack sound

            if (Combatant is Mobile target)
            {
                AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);
                target.Stam = Math.Max(0, target.Stam - 20);
                target.Mana = Math.Max(0, target.Mana - 20);
                target.SendMessage(0x480, "You are struck by an icy blast that drains your energy!");
            }

            m_NextFrostShock = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(25, 35));
        }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from != null)
            {
                damage = (int)(damage * 0.6); // 40% damage reduction
                from.SendMessage("Your weapon strikes feel dulled by the icy hide!");
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.2)
                c.DropItem(new FrozenHeartShard());
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

    public class IceboundRelic : Item
    {
        [Constructable]
        public IceboundRelic() : base(0x1F2B)
        {
            Hue = 1150;
            Name = "an Icebound Relic";
            LootType = LootType.Cursed;
        }

        public IceboundRelic(Serial serial) : base(serial) { }

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

    public class FrozenHeartShard : Item
    {
        [Constructable]
        public FrozenHeartShard() : base(0x1C10)
        {
            Hue = 1150;
            Name = "a Frozen Heart Shard";
            LootType = LootType.Blessed;
        }

        public FrozenHeartShard(Serial serial) : base(serial) { }

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
