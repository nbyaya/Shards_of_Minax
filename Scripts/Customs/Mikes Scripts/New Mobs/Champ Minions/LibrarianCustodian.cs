using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a librarian custodian")]
    public class LibrarianCustodian : BaseCreature
    {
        private TimeSpan m_SummonDelay = TimeSpan.FromSeconds(30.0); // time between summons
        public DateTime m_NextSummonTime;

        [Constructable]
        public LibrarianCustodian() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x190;
            Name = NameList.RandomName("male");
            Title = " the Librarian Custodian";
			Team = 2;

            Hue = Utility.RandomSkinHue();
            Item robe = new Robe(Utility.RandomNeutralHue());
            Item shoes = new Shoes(Utility.RandomNeutralHue());
            Item glasses = new Item(0x1FBA); // Glasses
            glasses.Layer = Layer.Helm;

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(robe);
            AddItem(shoes);
            AddItem(glasses);
            AddItem(hair);

            SetStr(600, 800);
            SetDex(100, 150);
            SetInt(500, 600);

            SetHits(400, 600);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 70.0);
            SetSkill(SkillName.MagicResist, 85.5, 100.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 40.1, 60.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 40;

            m_NextSummonTime = DateTime.Now + m_SummonDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSummonTime)
            {
                SummonPaperConstruct();
                m_NextSummonTime = DateTime.Now + m_SummonDelay;
            }
        }

        public void SummonPaperConstruct()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
            {
                this.Say(true, "Defend the library, my paper constructs!");

                for (int i = 0; i < 3; i++)
                {
                    PaperConstruct construct = new PaperConstruct();
                    construct.MoveToWorld(combatant.Location, combatant.Map);
                    construct.Combatant = combatant;
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGold(150, 200);
            AddLoot(LootPack.Rich);
        }

        public LibrarianCustodian(Serial serial) : base(serial)
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

    public class PaperConstruct : BaseCreature
    {
        [Constructable]
        public PaperConstruct() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xF0; // Paper doll graphic
            Name = "a paper construct";

            SetStr(100, 150);
            SetDex(150, 200);
            SetInt(10, 20);

            SetHits(50, 75);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 0);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 25.0, 50.0);
            SetSkill(SkillName.Tactics, 25.0, 50.0);
            SetSkill(SkillName.Wrestling, 25.0, 50.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 10;

            Tamable = false;
        }

        public PaperConstruct(Serial serial) : base(serial)
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
