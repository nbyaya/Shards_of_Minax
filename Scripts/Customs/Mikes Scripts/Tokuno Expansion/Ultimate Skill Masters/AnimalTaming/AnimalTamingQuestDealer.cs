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
    [CorpseName("A AnimalTaming Quest Dealer's corpse")]
    public class AnimalTamingQuestDealer : Mobile
    {
        private static TimeSpan QuestDelay = TimeSpan.FromMinutes(5); // 5 minutes delay
        private static DateTime LastQuestTime = DateTime.MinValue;

        public virtual bool IsInvulnerable { get { return true; } }

        [Constructable]
        public AnimalTamingQuestDealer()
        {
            Name = NameList.RandomName("male");
            Title = "The AnimalTaming Quest Dealer";
            Body = 0x190;
            CantWalk = true;
			Direction = Direction.West;
            Hue = Utility.RandomSkinHue();

            AddItem(new Cloak());
            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new ShepherdsCrook());

            Blessed = true;
        }

        public AnimalTamingQuestDealer(Serial serial) : base(serial) { }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new AnimalTamingQuestDealerEntry(from, this));
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

    public class AnimalTamingQuestDealerEntry : ContextMenuEntry
    {
        private Mobile m_Mobile;
        private AnimalTamingQuestDealer m_Giver;

        public AnimalTamingQuestDealerEntry(Mobile from, AnimalTamingQuestDealer giver) : base(6146, 3)
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
                if (!mobile.HasGump(typeof(AnimalTamingQuestDealerGump)))
                {
                    mobile.SendGump(new AnimalTamingQuestDealerGump(mobile));
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
