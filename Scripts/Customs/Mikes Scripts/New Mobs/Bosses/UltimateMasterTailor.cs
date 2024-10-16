using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Coco Chanel")]
    public class UltimateMasterTailor : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterTailor()
            : base(AIType.AI_Mage)
        {
            Name = "Coco Chanel";
            Title = "The Ultimate Tailor";
            Body = 0x191;
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

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Tailoring, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70;
			
			AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Cloak(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x94;
        }

        public UltimateMasterTailor(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(DesignersScissors), typeof(CoutureFabric) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(EmbroideredShirt), typeof(SewingKit) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(FashionMannequin), typeof(FineCloth) }; }
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

            c.DropItem(new PowerScroll(SkillName.Tailoring, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new DesignersScissors());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new CoutureFabric());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: FashionStrike(defender); break;
                    case 1: SeamstressSkill(); break;
                    case 2: Repair(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void FashionStrike(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                int damage = Utility.RandomMinMax(60, 80);
                AOS.Damage(defender, this, damage, 100, 0, 0, 0, 0);

                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.PlaySound(0x207);
            }
        }

        public void SeamstressSkill()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.VirtualArmorMod += 20;

                m.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist);
                m.PlaySound(0x1EA);
            }
        }

        public void Repair()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.Hits += Utility.RandomMinMax(50, 100);
                m.VirtualArmorMod += 20;

                m.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                m.PlaySound(0x1FA);
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
}
