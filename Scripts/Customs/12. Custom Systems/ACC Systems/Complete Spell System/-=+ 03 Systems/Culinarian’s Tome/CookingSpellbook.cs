using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CookingMagic
{
    public class CookingSpellbook : CSpellbook
    {
        public override School School { get { return School.CulinariansTome; } }

        [Constructable]
        public CookingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public CookingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public CookingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1100;
            Name = "Culinarian’s Tome";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update the content based on CookingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.CookingSpells].Points;
            }

            from.CloseGump(typeof(CookingSpellbookGump));
            from.SendGump(new CookingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
                list.Add(new CookingSpellbookEntry(player, this));
        }

        public CookingSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Cooking Skill Tree.
    public class CookingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private CookingSpellbook m_Spellbook;

        public CookingSpellbookEntry(PlayerMobile player, CookingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new CookingSkillTree(m_Player));
        }
    }
}
