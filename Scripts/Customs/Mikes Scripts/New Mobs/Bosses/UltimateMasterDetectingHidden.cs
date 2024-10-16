using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Sherlock Holmes")]
    public class UltimateMasterDetectingHidden : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterDetectingHidden()
            : base(AIType.AI_Mage)
        {
            Name = "Sherlock Holmes";
            Title = "The Master Detective";
            Body = 0x190;
            Hue = 0x83F2;

            SetStr(305, 425);
            SetDex(172, 250);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.DetectHidden, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = 22500;

            VirtualArmor = 70;
			
            AddItem(new FancyShirt(1157));
            AddItem(new LongPants(1175));
            AddItem(new Cloak(1175));
            AddItem(new Boots(1175));

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x455;

            AddItem(new DeerstalkerHat());
            AddItem(new MagnifyingGlass());
        }

        public UltimateMasterDetectingHidden(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(MagnifyingGlass), typeof(DeerstalkerHat) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(MagnifyingGlass), typeof(InvisibilityPotion) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(MagnifyingGlass), typeof(MagnifyingGlass) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.DetectHidden, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MagnifyingGlass());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new DeerstalkerHat());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: ElementaryStrike(defender); break;
                    case 1: Detect(); break;
                    case 2: Deduction(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void ElementaryStrike(Mobile defender)
        {
            if (defender != null)
            {
                int damage = Utility.RandomMinMax(50, 65);
                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);
                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.PlaySound(0x307);
            }
        }

        public void Detect()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(12))
            {
                if (m != this && m.Hidden && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                m.RevealingAction();
                m.FixedParticles(0x375A, 9, 20, 5027, EffectLayer.Waist);
                m.PlaySound(0x1FD);
            }
        }

        public void Deduction(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                defender.PlaySound(0x1E1);

                StatMod mod;
                mod = defender.GetStatMod("[Deduction] Str");
                if (mod != null)
                    defender.RemoveStatMod("[Deduction] Str");
                defender.AddStatMod(new StatMod(StatType.Str, "[Deduction] Str", -20, TimeSpan.FromSeconds(60)));

                mod = defender.GetStatMod("[Deduction] Dex");
                if (mod != null)
                    defender.RemoveStatMod("[Deduction] Dex");
                defender.AddStatMod(new StatMod(StatType.Dex, "[Deduction] Dex", -20, TimeSpan.FromSeconds(60)));

                mod = defender.GetStatMod("[Deduction] Int");
                if (mod != null)
                    defender.RemoveStatMod("[Deduction] Int");
                defender.AddStatMod(new StatMod(StatType.Int, "[Deduction] Int", -20, TimeSpan.FromSeconds(60)));

                defender.SendLocalizedMessage(1070845); // The creature's intellect is reducing your defenses!
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
        }
    }

    public class MagnifyingGlass : BaseBashing
    {
        [Constructable]
        public MagnifyingGlass() : base(0xFC2)
        {
            Weight = 1.0;
            Name = "Magnifying Glass";
            Hue = 2213;
        }

        public MagnifyingGlass(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060451, "10"); // detection range ~1_val~ increase
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

    public class DeerstalkerHat : BaseHat
    {
        [Constructable]
        public DeerstalkerHat() : base(0x171B)
        {
            Weight = 1.0;
            Name = "Deerstalker Hat";
            Hue = 2213;
        }

        public DeerstalkerHat(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060439, "10"); // intelligence bonus ~1_val~
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