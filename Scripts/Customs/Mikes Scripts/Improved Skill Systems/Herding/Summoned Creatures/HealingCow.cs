using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a healing cow corpse")]
    public class HealingCow : BaseCreature
    {
        private DateTime m_MilkedOn;
        private int m_Milk;

        [Constructable]
        public HealingCow()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a healing cow";
            Body = Utility.RandomList(0xD8, 0xE7);
            BaseSoundID = 0x78;
            Hue = 1153; // Light blue hue

            SetStr(30);
            SetDex(15);
            SetInt(5);

            SetHits(30);
            SetMana(0);

            SetDamage(1, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 15);

            SetSkill(SkillName.MagicResist, 5.5);
            SetSkill(SkillName.Tactics, 5.5);
            SetSkill(SkillName.Wrestling, 5.5);

            Fame = 300;
            Karma = 1000;

            VirtualArmor = 10;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_MilkedOn = DateTime.MinValue;
            m_Milk = 5;
        }

        public HealingCow(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime MilkedOn
        {
            get { return m_MilkedOn; }
            set { m_MilkedOn = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Milk
        {
            get { return m_Milk; }
            set { m_Milk = value; }
        }

        public override int Meat { get { return 8; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            int random = Utility.Random(100);

            if (random < 5)
                Tip();
            else if (random < 20)
                PlaySound(120);
            else if (random < 40)
                PlaySound(121);
        }

        public void Tip()
        {
            PlaySound(121);
            Animate(8, 0, 3, true, false, 0);
        }

        public bool TryMilk(Mobile from)
        {
            if (!from.InLOS(this) || !from.InRange(Location, 2))
            {
                from.SendLocalizedMessage(1080400); // You can not milk the cow from this location.
                return false;
            }

            if (Controlled && ControlMaster != from)
            {
                from.SendLocalizedMessage(1071182); // The cow nimbly escapes your attempts to milk it.
                return false;
            }

            if (m_Milk == 0 && m_MilkedOn + TimeSpan.FromHours(1) > DateTime.UtcNow)
            {
                from.SendMessage("This cow cannot be milked now. Please wait for some time.");
                return false;
            }

            if (m_Milk == 0)
                m_Milk = 5;

            m_MilkedOn = DateTime.UtcNow;
            m_Milk--;

            HealingMilk milk = new HealingMilk();
            if (from.PlaceInBackpack(milk))
            {
                from.SendLocalizedMessage(1080397); // You place the milk in your backpack.
                PlaySound(0x4D1);
                return true;
            }
            else
            {
                milk.Delete();
                from.SendLocalizedMessage(1080398); // You do not have room for the milk in your backpack.
                return false;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((DateTime)m_MilkedOn);
            writer.Write((int)m_Milk);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_MilkedOn = reader.ReadDateTime();
            m_Milk = reader.ReadInt();
        }
    }

    public class HealingMilk : Item
    {
        [Constructable]
        public HealingMilk() : base(0x9AD)
        {
            Name = "Milk of Life";
            Hue = 1153; // Light blue hue
            Weight = 1.0;
        }

        public HealingMilk(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (from.Hits >= from.HitsMax)
            {
                from.SendMessage("You are already at full health.");
                return;
            }

            from.Heal(Utility.RandomMinMax(15, 25));
            from.PlaySound(0x31);
            from.FixedEffect(0x373A, 10, 16);
            from.SendMessage("You drink the Milk of Life and feel refreshed!");
            this.Delete();
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