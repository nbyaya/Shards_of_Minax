using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class DetectHiddenSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private DetectHiddenSpellbook m_Spellbook;

        public DetectHiddenSpellbookEntry(PlayerMobile player, DetectHiddenSpellbook spellbook)
            : base(1078990) // Use a custom cliloc number for your skill tree entry text.
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new DetectHiddenSkillTree(m_Player));
        }
    }
}
