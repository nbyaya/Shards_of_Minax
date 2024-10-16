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
    public class ItemCollectionDealer : Mobile
    {
        private static TimeSpan QuestDelay = TimeSpan.FromMinutes(5); // 5 minutes delay
        private static DateTime LastQuestTime = DateTime.MinValue;

        public virtual bool IsInvulnerable { get { return true; } }

        [Constructable]
        public ItemCollectionDealer()
        {
            Name = "Dux";
            Title = "The Item Collector";
            Body = 0x190;
            CantWalk = true;
            Hue = Utility.RandomSkinHue();

            // Add items to the NPC
            AddItem(new Cloak { Movable = false, Hue = 1109 });
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new ShepherdsCrook { Movable = false, Hue = 1153 });

            // Additional items and appearance setup
            AddItem(new Boots { Hue = 2112, Movable = false });
            AddItem(new FancyShirt { Hue = 1267, Movable = false });
            AddItem(new LongPants { Hue = 847, Movable = false });

            Blessed = true;
        }

        public ItemCollectionDealer(Serial serial) : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new ItemCollectionDealerEntry(from, this));
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

        public bool CanGiveQuest()
        {
            if (DateTime.Now - LastQuestTime < QuestDelay)
            {
                return false;
            }
            return true;
        }

        public void UpdateLastQuestTime()
        {
            LastQuestTime = DateTime.Now;
        }
    }

    public class ItemCollectionDealerEntry : ContextMenuEntry
    {
        private Mobile m_Mobile;
        private ItemCollectionDealer m_Giver;

        public ItemCollectionDealerEntry(Mobile from, ItemCollectionDealer giver) : base(6146, 3)
        {
            m_Mobile = from;
            m_Giver = giver;
        }

        public override void OnClick()
        {
            if (!(m_Mobile is PlayerMobile))
                return;

            PlayerMobile mobile = (PlayerMobile)m_Mobile;

            if (m_Giver.CanGiveQuest())
            {
                if (!mobile.HasGump(typeof(ItemCollectionDealerGump)))
                {
                    mobile.SendGump(new ItemCollectionDealerGump(mobile));
                    mobile.AddToBackpack(new ItemCollectionContract());
                }
                m_Giver.UpdateLastQuestTime();
            }
            else
            {
                mobile.SendMessage("You must wait before requesting another quest.");
            }
        }
    }
}
