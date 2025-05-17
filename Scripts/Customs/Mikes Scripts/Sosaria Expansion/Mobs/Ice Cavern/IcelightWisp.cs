using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("an icelight wisp corpse")]
    public class IcelightWisp : BaseCreature
    {
        private DateTime m_NextBlink;
        private DateTime m_NextNova;
        private DateTime m_NextIllusion;
        private bool m_Initialized;

        [Constructable]
        public IcelightWisp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Icelight Wisp";
            Body = 165; // Same body as DarkWisp
            Hue = 1152; // Pale icy blue hue
            BaseSoundID = 466;

            SetStr(300, 400);
            SetDex(200, 300);
            SetInt(400, 500);

            SetHits(600, 800);
            SetMana(2000);

            SetDamage(20, 25);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 30, 50);
            SetResistance(ResistanceType.Fire, 10, 25);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 80.0);
            SetSkill(SkillName.SpiritSpeak, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;

            AddItem(new LightSource());
        }

        public IcelightWisp(Serial serial) : base(serial) { }

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 4);

            if (Utility.RandomDouble() < 0.01) // 1% drop chance
            {
                PackItem(new ShardOfFrozenLight());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_Initialized)
                {
                    m_NextBlink = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 15));
                    m_NextNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextIllusion = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                    m_Initialized = true;
                }

                if (DateTime.UtcNow >= m_NextBlink)
                    Blink();

                if (DateTime.UtcNow >= m_NextNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextIllusion)
                    Illusion();
            }
        }

        private void Blink()
        {
            Point3D newLocation = new Point3D(Location.X + Utility.RandomMinMax(-3, 3), Location.Y + Utility.RandomMinMax(-3, 3), Location.Z);
            if (Map.CanSpawnMobile(newLocation))
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 20, 1153, 0, 5029, 0);
                Location = newLocation;
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 20, 1153, 0, 5029, 0);
                PlaySound(0x653);
            }
            m_NextBlink = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*releases a pulse of freezing light*");

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.Hidden && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.PlaySound(0x208);
                    m.SendMessage(0x480, "You are frozen by the Icelight Wisp’s nova!");
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);
                }
            }

            m_NextNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }

        private void Illusion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*shifts into blinding duplicates*");
            PlaySound(0x1E4);

            for (int i = 0; i < 2; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), () =>
                {
                    CloneWisp illusion = new CloneWisp(this);
                    illusion.MoveToWorld(Location, Map);
                });
            }

            m_NextIllusion = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.20 && defender is Mobile target)
            {
                target.Mana -= Utility.RandomMinMax(10, 20);
                target.Stam -= Utility.RandomMinMax(10, 20);
                target.SendMessage(0x22, "The icy touch of the wisp drains your energy.");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Initialized = false;
        }
    }

    public class CloneWisp : IcelightWisp
    {
        public override bool DeleteCorpseOnDeath => true;

        public CloneWisp(IcelightWisp source)
        {
            Name = "an illusion of the Icelight Wisp";
            Hue = 1152;
            Body = source.Body;
            BaseSoundID = source.BaseSoundID;

            SetHits(10);
            SetDamage(0);
            Fame = 0;
            Karma = 0;

            Timer.DelayCall(TimeSpan.FromSeconds(8), Delete);
        }

        public CloneWisp(Serial serial) : base(serial) { }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(1, from, willKill);
            if (!Deleted)
                Delete();
        }

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

    public class ShardOfFrozenLight : Item
    {
        public ShardOfFrozenLight() : base(0x1ECD)
        {
            Name = "a Shard of Frozen Light";
            Hue = 1153;
            Weight = 1.0;
            LootType = LootType.Regular;
        }

        public ShardOfFrozenLight(Serial serial) : base(serial) { }

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
