using System;
using Server;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;  // Add this namespace for NetState
using Server.Commands;  // Add this namespace for CommandEventArgs

namespace Server.Mobiles
{
    [CorpseName("spymaster corpse")]
    public class Spymaster : Mobile
    {
        public virtual bool IsInvulnerable { get { return true; } }

        [Constructable]
        public Spymaster()
        {
            InitStats(31, 41, 51);

            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Blessed = true;

            AddItem(new Robe(Utility.RandomNeutralHue()));
            AddItem(new Boots());
            Utility.AssignRandomHair(this);
            Direction = Direction.South;
            Name = NameList.RandomName("male");
            Title = "the spymaster";
            CantWalk = true;
        }

        public Spymaster(Serial serial) : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new SpymasterEntry(from, this));
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

        public class SpymasterEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public SpymasterEntry(Mobile from, Mobile giver) : base(6146, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;

                if (!mobile.HasGump(typeof(SpymasterGump)))
                {
                    mobile.SendGump(new SpymasterGump(mobile));
                }
            }
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            PlayerMobile mobile = from as PlayerMobile;

            if (mobile != null)
            {
                if (dropped is Gold)
                {
                    int amount = dropped.Amount;
                    Item questScroll = null;

                    switch (amount)
                    {
                        case 500:
                            questScroll = new ArchaeologyCharter();
                            break;
                        case 1000:
                            questScroll = new MurderClue();
                            break;
                        case 1500:
                            questScroll = new TradeRouteContract();
                            break;
                        case 2000:
                            questScroll = new BurglaryMission();
                            break;
                        case 2500:
                            questScroll = new LumberContract();
                            break;
                        case 3000:
                            questScroll = new OreContract();
                            break;
                        default:
                            this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "That is not the amount I am looking for.", mobile.NetState);
                            return false;
                    }

                    if (questScroll != null)
                    {
                        mobile.AddToBackpack(questScroll);
                        this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Here is your quest scroll. Good luck!", mobile.NetState);
                        dropped.Delete();
                        return true;
                    }
                }
                else
                {
                    this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "I have no need for this...", mobile.NetState);
                }
            }

            return false;
        }
    }
}

namespace Server.Gumps
{
    public class SpymasterGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register("SpymasterGump", AccessLevel.GameMaster, new CommandEventHandler(SpymasterGump_OnCommand));
        }

        private static void SpymasterGump_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new SpymasterGump(e.Mobile));
        }

        public SpymasterGump(Mobile owner) : base(50, 50)
        {
            AddPage(0);
            AddImageTiled(54, 33, 369, 400, 2624);
            AddAlphaRegion(54, 33, 369, 400);
            AddImageTiled(416, 39, 44, 389, 203);

            AddImage(97, 49, 9005);
            AddImageTiled(58, 39, 29, 390, 10460);
            AddImageTiled(412, 37, 31, 389, 10460);
            AddLabel(140, 60, 0x34, "The Spymaster");

            AddHtml(107, 140, 300, 230, " < BODY > " +
            "<BASEFONT COLOR=YELLOW>Greetings adventurer. I am the Spymaster.<BR>" +
            "<BASEFONT COLOR=YELLOW>I deal in secrets and tasks that require<BR>" +
            "<BASEFONT COLOR=YELLOW>a delicate touch. Should you be interested<BR>" +
            "<BASEFONT COLOR=YELLOW>in undertaking a covert operation, simply<BR>" +
            "<BASEFONT COLOR=YELLOW>slip some gold my way and I will give you<BR>" +
            "<BASEFONT COLOR=YELLOW>a task befitting your generosity.<BR>" +
            "<BASEFONT COLOR=YELLOW><BR>" +
            "<BASEFONT COLOR=YELLOW>500 Gold - Archaeology Charter<BR>" +
            "<BASEFONT COLOR=YELLOW>1000 Gold - Murder Clue<BR>" +
            "<BASEFONT COLOR=YELLOW>1500 Gold - Trade Route Contract<BR>" +
            "<BASEFONT COLOR=YELLOW>2000 Gold - Burglary Mission<BR>" +
            "<BASEFONT COLOR=YELLOW>2500 Gold - Lumber Contract<BR>" +
            "<BASEFONT COLOR=YELLOW>3000 Gold - Ore Contract<BR>" +
            "</BODY>", false, true);

            AddImage(430, 9, 10441);
            AddImageTiled(40, 38, 17, 391, 9263);
            AddImage(6, 25, 10421);
            AddImage(34, 12, 10420);
            AddImageTiled(94, 25, 342, 15, 10304);
            AddImageTiled(40, 427, 415, 16, 10304);
            AddImage(-10, 314, 10402);
            AddImage(56, 150, 10411);
            AddImage(155, 120, 2103);
            AddImage(136, 84, 96);
            AddButton(225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            switch (info.ButtonID)
            {
                case 0: { break; }
            }
        }
    }
}
