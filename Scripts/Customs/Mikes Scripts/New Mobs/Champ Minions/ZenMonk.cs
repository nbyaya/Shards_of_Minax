using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a zen monk")]
    public class ZenMonk : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(10.0); // time between healing
        public DateTime m_NextHealTime;

        [Constructable]
        public ZenMonk() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Zen Monk";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Zen Monk";
            }

            Item robe = new Robe();
            robe.Hue = Utility.RandomNeutralHue();
            AddItem(robe);

            Item sandals = new Sandals();
            sandals.Hue = Utility.RandomNeutralHue();
            AddItem(sandals);

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
            SetInt(300, 400);

            SetHits(500, 700);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Healing, 80.1, 100.0);

            Fame = 6000;
            Karma = 6000;

            VirtualArmor = 50;

            m_NextHealTime = DateTime.Now + m_HealDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return true; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextHealTime)
            {
                HealAndCalm();
                m_NextHealTime = DateTime.Now + m_HealDelay;
            }
        }

        private void HealAndCalm()
        {
            foreach (Mobile m in this.GetMobilesInRange(4))
            {
                if (m is BaseCreature && ((BaseCreature)m).ControlMaster == this.ControlMaster && m.Alive && !m.Poisoned)
                {
                    if (m.Hits < m.HitsMax)
                    {
                        m.Hits += Utility.RandomMinMax(10, 20);
                        m.SendMessage("The Zen Monk heals your wounds.");
                    }

                    if (m.Paralyzed)
                    {
                        m.Paralyzed = false;
                        m.SendMessage("The Zen Monk calms your mind, removing paralysis.");
                    }
                }
            }
        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Average);
        }

        public ZenMonk(Serial serial) : base(serial)
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
