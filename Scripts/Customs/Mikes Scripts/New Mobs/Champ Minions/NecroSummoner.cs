using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a necro summoner")]
    public class NecroSummoner : BaseCreature
    {
        private TimeSpan m_SummonDelay = TimeSpan.FromSeconds(30.0); // time between summons
        public DateTime m_NextSummonTime;

        [Constructable]
        public NecroSummoner() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Name = NameList.RandomName("male");
            Title = " the Necro Summoner";
			Team = 2;

            Item robe = new Robe();
            robe.Hue = 0x455;
            AddItem(robe);

            Item hood = new HoodedShroudOfShadows();
            AddItem(hood);

            Item boots = new ThighBoots();
            AddItem(boots);

            Item staff = new GnarledStaff();
            AddItem(staff);
            staff.Movable = false;

            SetStr(500, 700);
            SetDex(100, 150);
            SetInt(300, 400);

            SetHits(400, 600);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.5, 100.0);
            SetSkill(SkillName.Meditation, 60.1, 70.0);
            SetSkill(SkillName.MagicResist, 85.5, 95.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.Wrestling, 70.1, 80.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            m_NextSummonTime = DateTime.Now + m_SummonDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextSummonTime)
            {
                SummonUndead();
                m_NextSummonTime = DateTime.Now + m_SummonDelay;
            }

            base.OnThink();
        }

        public void SummonUndead()
        {
            Mobile combatant = this.Combatant as Mobile;

            if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
            {
                BaseCreature summon = null;
                switch (Utility.Random(3))
                {
                    case 0: summon = new Skeleton(); break;
                    case 1: summon = new Zombie(); break;
                    case 2: summon = new Wraith(); break;
                }

                if (summon != null)
                {
                    summon.Team = this.Team;
                    summon.Map = this.Map;
                    summon.Location = this.Location;
                    summon.Combatant = combatant;
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);

            PackItem(new MandrakeRoot(Utility.RandomMinMax(10, 20)));
        }

        public NecroSummoner(Serial serial) : base(serial)
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
