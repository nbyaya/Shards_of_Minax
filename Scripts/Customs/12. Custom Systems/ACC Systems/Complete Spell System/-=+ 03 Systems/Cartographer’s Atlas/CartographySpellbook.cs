using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class CartographySpellbook : CSpellbook
    {
        public override School School { get { return School.CartographersAtlas; } }

        [Constructable]
        public CartographySpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public CartographySpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public CartographySpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 900;
            Name = "Cartographer’s Atlas";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                {
                    return;
                }
            }

            // Update the content based on the CartographySpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.CartographySpells].Points;
            }

            from.CloseGump(typeof(CartographySpellbookGump));
            from.SendGump(new CartographySpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile player)
            {
                list.Add(new CartographySpellbookEntry(player, this));
            }
        }

        public CartographySpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Cartography Skill Tree.
    public class CartographySpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private CartographySpellbook m_Spellbook;

        public CartographySpellbookEntry(PlayerMobile player, CartographySpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new CartographySkillTree(m_Player));
        }
    }
}
