using System;
using System.Collections.Generic;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class FletchingSpellbook : CSpellbook
    {
        public override School School { get { return School.FletchersFolio; } }

        [Constructable]
        public FletchingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public FletchingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public FletchingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2980;
            Name = "Fletchers Folio";
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

            // Update the content based on the FletchingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.FletchingSpells].Points;
            }

            from.CloseGump(typeof(FletchingSpellbookGump));
            from.SendGump(new FletchingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new FletchingSpellbookEntry(player, this));
            }
        }

        public FletchingSpellbook(Serial serial) : base(serial)
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

    public class FletchingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private FletchingSpellbook m_Spellbook;

        public FletchingSpellbookEntry(PlayerMobile player, FletchingSpellbook spellbook)
            : base(1078990) // Use your custom cliloc number here if desired.
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new FletchingSkillTree(m_Player));
        }
    }
}
