using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters
{
    public static class QuestHandler
    {
        public static bool HandleOnDragDrop(
            Mobile from,
            Item dropped,
            SkillName skill,
            string nameSuffix,
            int hue)
        {
            PlayerMobile mobile = from as PlayerMobile;

            if (mobile != null)
            {
                // Handle Gold drop for initiating quests
                if (dropped is Gold gold)
                {
                    int level = GetQuestLevel(gold.Amount);

                    if (level > 0)
                    {
                        // Create and give a GeneralizedQuestScroll
                        var questScroll = new GeneralizedQuestScroll(level, nameSuffix, hue);
                        mobile.AddToBackpack(questScroll);

                        from.PrivateOverheadMessage(
                            MessageType.Regular,
                            1153,
                            false,
                            "Return the quest parchment to me when you are done for your reward.",
                            from.NetState);

                        gold.Delete();
                        return true;
                    }
                    else
                    {
                        from.PrivateOverheadMessage(
                            MessageType.Regular,
                            1153,
                            false,
                            "That is not the amount I seek.",
                            from.NetState);

                        return false;
                    }
                }
                // Handle quest scrolls
                else if (dropped is GeneralizedQuestScroll questScroll)
                {
                    if (questScroll.NNeed > questScroll.NGot)
                    {
                        mobile.AddToBackpack(dropped);
                        from.PrivateOverheadMessage(
                            MessageType.Regular,
                            1153,
                            false,
                            $"You have not yet completed your {questScroll.SkillName} quest.",
                            from.NetState);

                        return false;
                    }

                    // The player completed the quest
                    string completionMessage = questScroll.mNType == 1
                        ? $"Impressive work in mastering {questScroll.SkillName}. You have proven your worth. Here is your reward."
                        : $"Ah, you have found the {questScroll.Name}! As promised, here is your reward.";

                    // Process rewards using the RewardManager
                    RewardManager.HandleQuestReward(mobile, skill, questScroll.NLevel);

                    from.PrivateOverheadMessage(
                        MessageType.Regular,
                        1153,
                        false,
                        completionMessage,
                        from.NetState);

                    dropped.Delete();
                    return true;
                }
                else
                {
                    mobile.AddToBackpack(dropped);
                    from.PrivateOverheadMessage(
                        MessageType.Regular,
                        1153,
                        false,
                        "I have no use for this...",
                        from.NetState);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines the quest level based on the amount of gold.
        /// </summary>
        private static int GetQuestLevel(int goldAmount)
        {
            switch (goldAmount)
            {
                case 500: return 1;
                case 1000: return 2;
                case 1500: return 3;
                case 2000: return 4;
                case 2500: return 5;
                case 3000: return 6;
                default: return 0;
            }
        }
    }
    public class GenericQuestGiverGump : Gump
    {
        public GenericQuestGiverGump(Mobile owner, string masterTitle, string skillName) : base(50, 50)
        {
            AddPage(0);
            AddImageTiled(54, 33, 369, 400, 2624);
            AddAlphaRegion(54, 33, 369, 400);
            AddImageTiled(416, 39, 44, 389, 203);

            AddImage(97, 49, 9005);
            AddImageTiled(58, 39, 29, 390, 10460);
            AddImageTiled(412, 37, 31, 389, 10460);
            AddLabel(140, 60, 0x34, $"The {masterTitle}");

            AddHtml(107, 140, 300, 230, BuildBodyText(masterTitle, skillName), false, true);

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

        private string BuildBodyText(string masterTitle, string skillName)
        {
            return "<BODY>" +
                   $"<BASEFONT COLOR=YELLOW>Hail brave adventurer. I am the local<BR>" +
                   $"<BASEFONT COLOR=YELLOW>{masterTitle}. If anything needs to be<BR>" +
                   $"<BASEFONT COLOR=YELLOW>done around here...I am the one to see.<BR>" +
                   $"<BASEFONT COLOR=YELLOW>Although I am not supposed to hire any<BR>" +
                   $"<BASEFONT COLOR=YELLOW>citizens, you look like you can handle<BR>" +
                   $"<BASEFONT COLOR=YELLOW>yourself. Of course, I could get in<BR>" +
                   $"<BASEFONT COLOR=YELLOW>much trouble if they find out that I<BR>" +
                   $"<BASEFONT COLOR=YELLOW>let slip what needs to be done, as gold<BR>" +
                   $"<BASEFONT COLOR=YELLOW>is rare and usually the nobles want to<BR>" +
                   $"<BASEFONT COLOR=YELLOW>get the riches for themselves.<BR>" +
                   $"<BASEFONT COLOR=YELLOW><BR>" +
                   $"<BASEFONT COLOR=YELLOW>I'll tell you what, you slip a few gold<BR>" +
                   $"<BASEFONT COLOR=YELLOW>coins my way, and I will be a little<BR>" +
                   $"<BASEFONT COLOR=YELLOW>careless about what I say. The more<BR>" +
                   $"<BASEFONT COLOR=YELLOW>gold I get...the more careless I will<BR>" +
                   $"<BASEFONT COLOR=YELLOW>speak.<BR>" +
                   $"<BASEFONT COLOR=YELLOW><BR>" +
                   $"<BASEFONT COLOR=YELLOW>500 Gold - Level 1 Quest<BR>" +
                   $"<BASEFONT COLOR=YELLOW>1000 Gold - Level 2 Quest<BR>" +
                   $"<BASEFONT COLOR=YELLOW>1500 Gold - Level 3 Quest<BR>" +
                   $"<BASEFONT COLOR=YELLOW>2000 Gold - Level 4 Quest<BR>" +
                   $"<BASEFONT COLOR=YELLOW>2500 Gold - Level 5 Quest<BR>" +
                   $"<BASEFONT COLOR=YELLOW>3000 Gold - Level 6 Quest<BR>" +
                   $"<BASEFONT COLOR=YELLOW><BR>" +
                   $"<BASEFONT COLOR=YELLOW>Simply follow the quest by targeting a<BR>" +
                   $"<BASEFONT COLOR=YELLOW>corpse of a slain creature. The corpse<BR>" +
                   $"<BASEFONT COLOR=YELLOW>must be either a creature you are in a<BR>" +
                   $"<BASEFONT COLOR=YELLOW>quest to slay...or a creature in the<BR>" +
                   $"<BASEFONT COLOR=YELLOW>area you are seeking an item.<BR>" +
                   $"<BASEFONT COLOR=YELLOW><BR>" +
                   $"<BASEFONT COLOR=YELLOW>Rewards vary from gold, items,<BR>" +
                   $"<BASEFONT COLOR=YELLOW>Maxxia Scrolls, and {skillName} Power Scrolls.<BR>" +
                   "</BODY>";
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
    public class QuestGiverContextMenuEntry : ContextMenuEntry
    {
        private Mobile m_From;
        private string m_MasterTitle;
        private string m_SkillName;

        public QuestGiverContextMenuEntry(Mobile from, string masterTitle, string skillName)
            : base(6146) // Localization number, adjust if necessary
        {
            m_From = from;
            m_MasterTitle = masterTitle;
            m_SkillName = skillName;
        }

        public override void OnClick()
        {
            if (m_From != null && m_From.NetState != null)
            {
                m_From.SendGump(new GenericQuestGiverGump(m_From, m_MasterTitle, m_SkillName));
            }
        }
    }
    public class GeneralizedQuestScroll : QuestScroll
    {
        public string SkillName { get; private set; } // The skill associated with this scroll

        [Constructable]
        public GeneralizedQuestScroll(int level, string skillName, int hue) : base(level)
        {
            SkillName = skillName ?? "Unknown Skill";

            // Customize the name and hue based on the skill
            this.Name = $"{this.Name} [{SkillName}]";
            this.Hue = hue; 
        }

        public GeneralizedQuestScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
            writer.Write(SkillName); // Serialize the skill name
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            SkillName = reader.ReadString() ?? "Unknown Skill";
        }
    }
}
