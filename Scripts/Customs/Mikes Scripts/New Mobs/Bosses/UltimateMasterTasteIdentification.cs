using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Julia Child")]
    public class UltimateMasterTasteIdentification : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterTasteIdentification()
            : base(AIType.AI_Mage)
        {
            Name = "Julia Child";
            Title = "The Ultimate Master of Taste Identification";
            Body = 0x191;
            Hue = 0x83F;

            SetStr(250, 350);
            SetDex(100, 150);
            SetInt(600, 750);

            SetHits(10000);
            SetMana(3000);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.TasteID, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22000;
            Karma = -22000;

            VirtualArmor = 60;

            AddItem(new ChefToque());
            AddItem(new PlainDress());
            AddItem(new Sandals());

            HairItemID = 0x203C; // Long Hair
            HairHue = 0x44E;
        }

        public UltimateMasterTasteIdentification(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(MasterChefsKnife), typeof(FlavorExtract) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ParacelsusTome), typeof(PotionKeg) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(ElixirOfLife), typeof(MysticalAlembic) }; }
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

            c.DropItem(new PowerScroll(SkillName.TasteID, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new MasterChefsKnife());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new FlavorExtract());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: TasteTest(); break;
                    case 1: CulinaryDelight(); break;
                    case 2: FlavorBurst(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void TasteTest()
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

                int damage = Utility.RandomMinMax(30, 50);
                AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                m.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                m.PlaySound(0x1F7);

                m.SendMessage("You feel your strength sapped by a horrible taste!");
            }
        }

        public void CulinaryDelight()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && !this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                m.FixedParticles(0x376A, 9, 32, 5037, EffectLayer.Waist);
                m.PlaySound(0x1F2);

                m.SendMessage("You feel invigorated by a delicious culinary delight!");
            }
        }

        public void FlavorBurst()
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

                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x307);
                m.Paralyze(TimeSpan.FromSeconds(2.0));
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
