using System;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("corpse of a combat medic")]
    public class CombatMedic : BaseCreature
    {
        private TimeSpan m_HealDelay = TimeSpan.FromSeconds(10.0); // time between healing actions
        public DateTime m_NextHealTime;

        [Constructable]
        public CombatMedic() : base(AIType.AI_Healer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = " the Combat Medic";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = " the Combat Medic";
            }

            Item robe = new Robe();
            Item boots = new Boots();
            robe.Hue = 1150; // white color for medic
            boots.Hue = Utility.RandomNeutralHue();

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(robe);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(400, 600);
            SetDex(150, 200);
            SetInt(200, 300);

            SetHits(300, 500);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.Healing, 90.1, 100.0);
            SetSkill(SkillName.Magery, 80.5, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 95.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 2000;
            Karma = 2000;

            VirtualArmor = 40;

            m_NextHealTime = DateTime.Now + m_HealDelay;
        }

        public override bool AlwaysMurderer { get { return false; } }
        public override bool CanRummageCorpses { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool ClickTitle { get { return true; } }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextHealTime)
            {
                HealAllies();
                m_NextHealTime = DateTime.Now + m_HealDelay;
            }

            base.OnThink();
        }

        private void HealAllies()
        {
            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m is BaseCreature && ((BaseCreature)m).Controlled && m.Hits < m.HitsMax)
                {
                    int healAmount = Utility.RandomMinMax(10, 20);
                    m.Heal(healAmount);

                    m.SendMessage("The Combat Medic heals your wounds!");

                    break;
                }
            }
        }

        public CombatMedic(Serial serial) : base(serial)
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
