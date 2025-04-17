using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class ParrySpellbook : CSpellbook
    {
        public override School School { get { return School.GuardiansCodex; } }

        [Constructable]
        public ParrySpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public ParrySpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public ParrySpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1600;
            Name = "Guardians Codex";
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

            // Update content based on ParrySpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.ParrySpells].Points;
            }

            from.CloseGump(typeof(ParrySpellbookGump));
            from.SendGump(new ParrySpellbookGump(this));
        }

        public ParrySpellbook(Serial serial) : base(serial)
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

        // Add a context menu entry to open the Parry Skill Tree.
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new ParrySpellbookEntry(player, this));
            }
        }
    }

    // Custom Context Menu Entry for the Parry Spellbook.
    public class ParrySpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private ParrySpellbook m_Spellbook;

        public ParrySpellbookEntry(PlayerMobile player, ParrySpellbook spellbook)
            : base(1078990) // Use a custom cliloc number or identifier.
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new ParrySkillTree(m_Player));
        }
    }
}
