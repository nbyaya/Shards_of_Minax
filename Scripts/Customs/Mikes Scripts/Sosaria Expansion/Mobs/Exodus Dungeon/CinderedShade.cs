using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a cindered shade corpse")]
    public class CinderedShade : BaseCreature
    {
        private DateTime m_NextFlare;
        private DateTime m_NextAshPull;
        private DateTime m_NextShadeSplit;

        [Constructable]
        public CinderedShade()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Cindered Shade";
            Body = 9; // same body as FireDaemon
            BaseSoundID = 0x47D;
            Hue = 2075; // eerie ember-orange with shadow overlay

            SetStr(620, 750);
            SetDex(150, 190);
            SetInt(400, 500);

            SetHits(1600, 2000);

            SetDamage(15, 22);
            SetDamageType(ResistanceType.Fire, 60);
            SetDamageType(ResistanceType.Energy, 20);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, -20, 0);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 65;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextFlare)
                    FlareBurst();

                if (DateTime.UtcNow >= m_NextAshPull)
                    AshenGrasp();

                if (DateTime.UtcNow >= m_NextShadeSplit)
                    TrySplit();
            }
        }

        private void FlareBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x21, true, "*The Cindered Shade emits a burning flare!*");
            PlaySound(0x208); // flare sound
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x36BD, 10, 10, 0, 0, 5029, 0);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0);
                    m.SendMessage("You are scorched by the cindered flare!");
                }
            }

            m_NextFlare = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void AshenGrasp()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x21, true, "*The Cindered Shade pulls you into a grasp of ash!*");
                Effects.SendTargetParticles(target, 0x3709, 10, 30, 0, EffectLayer.Waist);
                PlaySound(0x5B3);

                AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 50, 0, 0, 0, 50);

                if (Utility.RandomDouble() < 0.5)
                {
                    target.Paralyze(TimeSpan.FromSeconds(2));
                    target.SendMessage("You are momentarily bound by burning ash!");
                }
            }

            m_NextAshPull = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void TrySplit()
        {
            if (Hits < (HitsMaxSeed * 0.5) && Utility.RandomDouble() < 0.25)
            {
                PublicOverheadMessage(MessageType.Regular, 0x21, true, "*The Cindered Shade tears itself in two!*");
                PlaySound(0x451);

                for (int i = 0; i < 2; i++)
                {
                    Mobile shade = new LesserCinderFragment();
                    shade.MoveToWorld(Location, Map);
                }

                Delete(); // original vanishes
            }

            m_NextShadeSplit = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 6);

            if (Utility.RandomDouble() < 0.005) // 0.5% chance
                PackItem(new CinderheartAmulet());
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new AshenCore());
        }

        public override bool AutoDispel => true;
        public override bool CanRummageCorpses => true;
        public override Poison PoisonImmune => Poison.Greater;
        public override int TreasureMapLevel => 5;
        public override int Meat => 0;

        public CinderedShade(Serial serial) : base(serial) { }

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

    public class LesserCinderFragment : BaseCreature
    {
        [Constructable]
        public LesserCinderFragment()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cinder fragment";
            Body = 9;
            Hue = 1109;
            BaseSoundID = 0x47D;

            SetStr(150, 180);
            SetDex(100, 120);
            SetInt(80, 100);

            SetHits(150, 200);
            SetDamage(7, 10);
            SetDamageType(ResistanceType.Fire, 100);

            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Physical, 20, 30);

            SetSkill(SkillName.Magery, 50.0);
            SetSkill(SkillName.MagicResist, 40.0);
        }

        public LesserCinderFragment(Serial serial) : base(serial) { }

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

    public class AshenCore : Item
    {
        [Constructable]
        public AshenCore() : base(0x1F0E)
        {
            Name = "an ashen core";
            Hue = 2075;
            Weight = 1.0;
        }

        public AshenCore(Serial serial) : base(serial) { }

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

    public class CinderheartAmulet : Item
    {
        [Constructable]
        public CinderheartAmulet() : base(0x4213)
        {
            Name = "Cinderheart Amulet";
            Hue = 2075;
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public CinderheartAmulet(Serial serial) : base(serial) { }

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
