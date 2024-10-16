using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a clue seeker")]
    public class ClueSeeker : BaseCreature
    {
        private TimeSpan m_ClueSeekDelay = TimeSpan.FromSeconds(10.0); // time between clue seeking actions
        public DateTime m_NextClueSeekTime;

        [Constructable]
        public ClueSeeker() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Clue Seeker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Clue Seeker";
            }

            Item robe = new Robe(Utility.RandomNeutralHue());
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
            SetDex(177, 255);
            SetInt(251, 350);

            SetHits(500, 700);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.Wrestling, 50.1, 75.0);
            SetSkill(SkillName.DetectHidden, 100.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 50;

            m_NextClueSeekTime = DateTime.Now + m_ClueSeekDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextClueSeekTime)
            {
                DetectHidden();

                m_NextClueSeekTime = DateTime.Now + m_ClueSeekDelay;
            }

            base.OnThink();
        }

        private void DetectHidden()
        {
            IPooledEnumerable mobilesInRange = this.GetMobilesInRange(8);

            foreach (Mobile m in mobilesInRange)
            {
                if (m != this && m.Hidden && m.AccessLevel == AccessLevel.Player)
                {
                    m.RevealingAction();
                    m.SendMessage("I have found you!");
                }
            }

            mobilesInRange.Free();
        }

        public override void GenerateLoot()
        {
            PackGold(200, 300);
            AddLoot(LootPack.Rich);

            PackItem(new Bandage(Utility.RandomMinMax(10, 20)));
        }

        public ClueSeeker(Serial serial) : base(serial)
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
