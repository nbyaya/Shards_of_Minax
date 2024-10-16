using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a decoy deployer")]
    public class DecoyDeployer : BaseCreature
    {
        private TimeSpan m_DecoyDelay = TimeSpan.FromSeconds(30.0); // time between deploying decoys
        public DateTime m_NextDecoyTime;

        [Constructable]
        public DecoyDeployer() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Deployer";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Deployer";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomBrightHue();
            AddItem(robe);

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
            SetDex(150, 200);
            SetInt(500, 700);

            SetHits(400, 600);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 40;

            m_NextDecoyTime = DateTime.Now + m_DecoyDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextDecoyTime)
            {
                DeployDecoy();
                m_NextDecoyTime = DateTime.Now + m_DecoyDelay;
            }

            base.OnThink();
        }

        private void DeployDecoy()
        {
            if (Combatant != null && Combatant.Map == this.Map)
            {
                Decoy decoy = new Decoy();
                decoy.MoveToWorld(Location, Map);
                this.Say(true, "Try and hit the real me!");

                int phrase = Utility.Random(3);

                switch (phrase)
                {
                    case 0: this.Say(true, "Can you tell which one is real?"); break;
                    case 1: this.Say(true, "Good luck finding me!"); break;
                    case 2: this.Say(true, "Catch me if you can!"); break;
                }
            }
        }

        public DecoyDeployer(Serial serial) : base(serial)
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

    public class Decoy : Mobile
    {
        public Decoy()
        {
            Body = Utility.RandomList(0x191, 0x190);
            Hue = Utility.RandomSkinHue();
            Name = "a decoy";

            Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(Delete));
        }

        public Decoy(Serial serial) : base(serial)
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
