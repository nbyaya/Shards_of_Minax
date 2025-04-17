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
    [CorpseName("The remains of the Ultimate Fletching Master")]
    public class UltimateFletchingMaster : Mobile
    {
        private static TimeSpan QuestDelay = TimeSpan.FromMinutes(5); // 5 minutes delay
        private static DateTime LastQuestTime = DateTime.MinValue;

        public virtual bool IsInvulnerable { get { return true; } }

        [Constructable]
        public UltimateFletchingMaster()
        {
            Name = NameList.RandomName("male");
            Title = "the Ultimate Fletching Master";
            Body = 0x190;
            CantWalk = true;
            Direction = Direction.West;
            Hue = Utility.RandomSkinHue();

            // Outfit (adjust as desired)
            AddItem(new Robe(Utility.RandomMinMax(1, 3000)));
            AddItem(new Sandals(1));
            AddItem(new WizardsHat(1109));

            // Possibly give them a Staff
            AddItem(new GnarledStaff { Movable = false, Hue = 1153 });

            Blessed = true;
        }

        public UltimateFletchingMaster(Serial serial) : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new UltimateFletchingMasterEntry(from, this));
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

    public class UltimateFletchingMasterEntry : ContextMenuEntry
    {
        private Mobile m_Mobile;
        private UltimateFletchingMaster m_Giver;

        public UltimateFletchingMasterEntry(Mobile from, UltimateFletchingMaster giver) : base(6146, 3)
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
                if (!mobile.HasGump(typeof(UltimateFletchingMasterGump)))
                {
                    mobile.SendGump(new UltimateFletchingMasterGump(mobile));
                    mobile.AddToBackpack(new UltimateFletchingMasterContract(mobile));
                }
                m_Giver.UpdateLastQuestTime();
            }
            else
            {
                mobile.SendMessage("You must wait before requesting another Fletching challenge.");
            }
        }
    }
}
