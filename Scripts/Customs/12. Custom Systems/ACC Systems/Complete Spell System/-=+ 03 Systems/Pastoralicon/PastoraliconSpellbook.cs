using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Pastoralicon
{
    public class PastoraliconBook : CSpellbook
    {
        public override School School { get { return School.Pastoralicon; } }

        [Constructable]
        public PastoraliconBook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public PastoraliconBook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public PastoraliconBook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2186;
            Name = "Pastoralicon";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update content based on PastoraliconSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.PastoraliconSpells].Points;
            }

            from.CloseGump(typeof(PastoraliconGump));
            from.SendGump(new PastoraliconGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new PastoraliconSpellbookEntry(player, this));
            }
        }

        public PastoraliconBook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Pastoralicon Skill Tree.
    public class PastoraliconSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private PastoraliconBook m_Spellbook;

        public PastoraliconSpellbookEntry(PlayerMobile player, PastoraliconBook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new Server.ACC.CSS.Systems.PastoraliconMagic.PastoraliconSkillTree(m_Player));
        }
    }
}
