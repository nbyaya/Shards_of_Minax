using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of an aquatic tamer")]
    public class AquaticTamer : BaseCreature
    {
        private TimeSpan m_SummonDelay = TimeSpan.FromSeconds(30.0); // time between summons
        public DateTime m_NextSummonTime;

        [Constructable]
        public AquaticTamer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomBlueHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Aquatic Tamer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Aquatic Tamer";
            }

            Item robe = new Robe(Utility.RandomBlueHue());
            Item sandals = new Sandals(Utility.RandomBlueHue());
            Item staff = new GnarledStaff();

            AddItem(robe);
            AddItem(sandals);
            AddItem(staff);
            staff.Movable = false;

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(600, 800);
            SetDex(150, 250);
            SetInt(400, 500);

            SetHits(500, 700);

            SetDamage(8, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 100.1, 150.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.Wrestling, 70.1, 90.0);

            Fame = 7000;
            Karma = -7000;

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
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 10))
                {
                    SummonSeaLife();
                    m_NextSummonTime = DateTime.Now + m_SummonDelay;
                }
            }

            base.OnThink();
        }

        private void SummonSeaLife()
        {
            Map map = this.Map;

            if (map == null)
                return;

            int creaturesToSummon = Utility.RandomMinMax(1, 3);

            for (int i = 0; i < creaturesToSummon; ++i)
            {
                BaseCreature creature;

                switch (Utility.Random(3))
                {
                    case 0: creature = new SeaSerpent(); break;
                    case 1: creature = new WaterElemental(); break;
                    default: creature = new DeepSeaSerpent(); break;
                }

                creature.Team = this.Team;
                creature.FightMode = FightMode.Closest;

                Point3D location = this.Location;
                bool validLocation = false;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = this.X + Utility.RandomMinMax(-3, 3);
                    int y = this.Y + Utility.RandomMinMax(-3, 3);
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                        location = new Point3D(x, y, this.Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        location = new Point3D(x, y, z);
                }

                creature.MoveToWorld(location, map);
                creature.Combatant = this.Combatant;
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.FilthyRich);
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 20)));
        }

        public AquaticTamer(Serial serial) : base(serial)
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
