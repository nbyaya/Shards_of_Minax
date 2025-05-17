using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a trap setter")]
    public class TrapSetter : BaseCreature
    {
        private TimeSpan m_TrapDelay = TimeSpan.FromSeconds(15.0); // time between trap placements
        public DateTime m_NextTrapTime;

        [Constructable]
        public TrapSetter() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = Utility.RandomSkinHue();
			Team = 2;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Trap Setter";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Trap Setter";
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D));
            Item tunic = new LeatherChest();
            Item pants = new LongPants(Utility.RandomNeutralHue());
            Item boots = new ThighBoots(Utility.RandomNeutralHue());
            hair.Hue = Utility.RandomHairHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;

            AddItem(hair);
            AddItem(tunic);
            AddItem(pants);
            AddItem(boots);

            if (!this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x2041));
                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;
                AddItem(beard);
            }

            SetStr(700, 900);
            SetDex(200, 250);
            SetInt(150, 200);

            SetHits(400, 600);

            SetDamage(8, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 65.0, 80.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);
            SetSkill(SkillName.Stealth, 80.0, 100.0);
            SetSkill(SkillName.Poisoning, 80.0, 100.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            m_NextTrapTime = DateTime.Now + m_TrapDelay;
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextTrapTime)
            {
                Mobile target = this.Combatant as Mobile;

                if (target != null && target.Map == this.Map && target.InRange(this, 8))
                {
                    if (Utility.RandomBool())
                    {
                        PlaceTrap(target);
                        m_NextTrapTime = DateTime.Now + m_TrapDelay;
                    }
                }

                base.OnThink();
            }
        }

        private void PlaceTrap(Mobile target)
        {

        }

        public TrapSetter(Serial serial) : base(serial)
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
