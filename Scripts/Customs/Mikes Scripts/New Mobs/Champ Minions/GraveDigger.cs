using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a grave digger")]
    public class GraveDigger : BaseCreature
    {
        private DateTime m_NextSummonTime;
        private TimeSpan m_SummonDelay = TimeSpan.FromSeconds(30.0);

        [Constructable]
        public GraveDigger() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Grave Digger";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Grave Digger";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomNeutralHue();
            AddItem(robe);

            Item shovel = new Shovel();
            AddItem(shovel);
            shovel.Movable = false;

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

            SetStr(300, 400);
            SetDex(100, 150);
            SetInt(200, 250);

            SetHits(400, 600);

            SetDamage(8, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.MagicResist, 85.5, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 38;

            m_NextSummonTime = DateTime.Now + m_SummonDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextSummonTime)
            {
                SummonSkeleton();
                m_NextSummonTime = DateTime.Now + m_SummonDelay;
            }
        }

        private void SummonSkeleton()
        {
            if (Map == null)
                return;

            BaseCreature skeleton = new Skeleton();
            skeleton.Team = this.Team;
            skeleton.Combatant = this.Combatant;

            Point3D loc = this.Location;
            bool validLocation = false;

            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = X + Utility.Random(3) - 1;
                int y = Y + Utility.Random(3) - 1;
                int z = Map.GetAverageZ(x, y);

                if (validLocation = Map.CanFit(x, y, Z, 16, false, false))
                    loc = new Point3D(x, y, Z);
                else if (validLocation = Map.CanFit(x, y, z, 16, false, false))
                    loc = new Point3D(x, y, z);
            }

            skeleton.MoveToWorld(loc, Map);
            skeleton.Say("Arise, my minion!");
        }

        public GraveDigger(Serial serial) : base(serial)
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
