using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class TasteIDSpellbook : CSpellbook
    {
        public override School School { get { return School.ConnoisseursTome; } }

        [Constructable]
        public TasteIDSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public TasteIDSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public TasteIDSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1000;
            Name = "Connoisseur’s Tome";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the content based on the TasteIDSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.TasteIDSpells].Points;
            }

            from.CloseGump(typeof(TasteIDSpellbookGump));
            from.SendGump(new TasteIDSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
                list.Add(new TasteIDSpellbookEntry(player, this));
        }

        public TasteIDSpellbook(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Custom Context Menu Entry for TasteID Spellbook
    public class TasteIDSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private TasteIDSpellbook m_Spellbook;

        public TasteIDSpellbookEntry(PlayerMobile player, TasteIDSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree (you can change the number if needed)
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new TasteIDSkillTree(m_Player));
        }
    }
}
