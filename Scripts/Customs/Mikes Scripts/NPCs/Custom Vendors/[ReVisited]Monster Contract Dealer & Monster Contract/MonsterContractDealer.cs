using System;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("Good Luck Getting Your Contract!")]
    public class MonsterContractDealer : Mobile
    {
        private static TimeSpan QuestDelay = TimeSpan.FromMinutes(5); // 5 minutes delay
        private static DateTime LastQuestTime = DateTime.MinValue;

        public virtual bool IsInvulnerable { get { return true; } }

        [Constructable]
        public MonsterContractDealer()
        {
            Name = "-Grim-";
            Title = "The Monster Slayer";
            Body = 0x190;
            CantWalk = true;
            Hue = Utility.RandomSkinHue();

            AddItem(ItemSet(new Cloak()));
            AddItem(ItemSet(new Robe()));
            AddItem(ItemSet1(new ShepherdsCrook()));

            Item Boots = new Boots { Hue = 2112, Movable = false };
            AddItem(Boots);

            Item FancyShirt = new FancyShirt { Hue = 1267, Movable = false };
            AddItem(FancyShirt);

            Item LongPants = new LongPants { Hue = 847, Movable = false };
            AddItem(LongPants);

            int hairHue = 1814;
            switch (Utility.Random(2))
            {
                case 0: AddItem(new PonyTail(hairHue)); break;
                case 1: AddItem(new Goatee(hairHue)); break;
            }

            Blessed = true;
        }

        public static Item ItemSet(Item item)
        {
            item.Movable = false;
            item.Hue = 1109;
            return item;
        }

        public static Item ItemSet1(Item item)
        {
            item.Movable = false;
            item.Hue = 1153;
            return item;
        }

        public MonsterContractDealer(Serial serial) : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new MonsterContractDealerEntry(from, this));
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

        public bool CanGiveContract()
        {
            return DateTime.Now - LastQuestTime >= QuestDelay;
        }

        public void UpdateLastQuestTime()
        {
            LastQuestTime = DateTime.Now;
        }
    }

    public class MonsterContractDealerEntry : ContextMenuEntry
    {
        private Mobile m_Mobile;
        private MonsterContractDealer m_Giver;

        public MonsterContractDealerEntry(Mobile from, MonsterContractDealer giver) : base(6146, 3)
        {
            m_Mobile = from;
            m_Giver = giver;
        }

        public override void OnClick()
        {
            if (!(m_Mobile is PlayerMobile))
                return;

            PlayerMobile mobile = (PlayerMobile)m_Mobile;

            if (m_Giver.CanGiveContract())
            {
                if (!mobile.HasGump(typeof(MonsterContractDealerGump)))
                {
                    mobile.SendGump(new MonsterContractDealerGump(mobile));
                    mobile.AddToBackpack(new MonsterContract());
                }
                m_Giver.UpdateLastQuestTime();
            }
            else
            {
                mobile.SendMessage("You must wait before requesting another contract.");
            }
        }
    }
}
