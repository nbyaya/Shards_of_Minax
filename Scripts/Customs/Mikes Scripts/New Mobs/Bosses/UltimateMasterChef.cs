using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Gordon Ramsay")]
    public class UltimateMasterChef : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterChef()
            : base(AIType.AI_Melee)
        {
            Name = "Gordon Ramsay";
            Title = "The Ultimate Chef";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(305, 425);
            SetDex(72, 150);
            SetInt(505, 750);

            SetHits(12000);
            SetMana(2500);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Swords, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Cooking, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;

            AddItem(new Bonnet());
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            HairItemID = 0x2048; // Short hair
            HairHue = 0x47E;
        }

        public UltimateMasterChef(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(ChefsKnife), typeof(RamsaysRecipeBook) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ExquisiteCookingTools), typeof(MasterworkCookingApron) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(DecorativePlatter), typeof(GoldenChefHat) }; }
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

            c.DropItem(new PowerScroll(SkillName.Cooking, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ChefsKnife());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new RamsaysRecipeBook());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: Flambe(defender); break;
                    case 1: KitchenFury(); break;
                    case 2: HealingFeast(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void Flambe(Mobile defender)
        {
            if (defender != null)
            {
                defender.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                defender.PlaySound(0x208);

                AOS.Damage(defender, this, Utility.RandomMinMax(60, 80), 0, 100, 0, 0, 0);
            }
        }

        public void KitchenFury()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                m.SendLocalizedMessage(1070755); // You are struck by Gordon Ramsay's fury!
                m.FixedParticles(0x3728, 1, 13, 0x26B4, 0x3F, 0x7, EffectLayer.Head);
                m.PlaySound(0x209);
            }
        }

        public void HealingFeast()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                m.PlaySound(0x1F2);

                m.Hits += Utility.RandomMinMax(20, 40);
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

    public class ChefsKnife : Item
    {
        [Constructable]
        public ChefsKnife() : base(0x10E5)
        {
            Name = "Chef's Knife";
            Hue = 0x47E;
            Weight = 1.0;
        }

        public ChefsKnife(Serial serial) : base(serial)
        {
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

    public class RamsaysRecipeBook : Item
    {
        [Constructable]
        public RamsaysRecipeBook() : base(0x22C5)
        {
            Name = "Ramsay's Recipe Book";
            Hue = 0x47E;
            Weight = 1.0;
        }

        public RamsaysRecipeBook(Serial serial) : base(serial)
        {
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
}
