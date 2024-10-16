using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a battle dressmaker")]
    public class BattleDressmaker : BaseCreature
    {
        private TimeSpan m_EnhanceDelay = TimeSpan.FromSeconds(15.0); // time between enhancements
        public DateTime m_NextEnhanceTime;

        [Constructable]
        public BattleDressmaker() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Dressmaker";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Dressmaker";
            }

            Item dress = new FancyDress(Utility.RandomNeutralHue());
            AddItem(dress);

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);

            SetStr(600, 800);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(500, 700);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 50;

            m_NextEnhanceTime = DateTime.Now + m_EnhanceDelay;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextEnhanceTime)
            {
                Mobile combatant = this.Combatant as Mobile;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 8))
                {
                    EnhanceArmor();
                    m_NextEnhanceTime = DateTime.Now + m_EnhanceDelay;
                }

                base.OnThink();
            }
        }

        public void EnhanceArmor()
        {

        }

        public override void GenerateLoot()
        {
            PackGem();
            PackGold(200, 250);
            AddLoot(LootPack.Rich);
        }

        public BattleDressmaker(Serial serial) : base(serial)
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
