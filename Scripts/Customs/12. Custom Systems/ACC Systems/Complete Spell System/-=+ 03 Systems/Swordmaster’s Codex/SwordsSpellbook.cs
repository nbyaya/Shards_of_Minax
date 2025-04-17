using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.SwordsMagic
{
    public class SwordsSpellbook : CSpellbook
    {
        public override School School { get { return School.SwordmastersCodex; } }

        [Constructable]
        public SwordsSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public SwordsSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public SwordsSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2570;
            Name = "Swordmasters Codex";
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

            // Update the content based on the unlocked sword spells from the skill tree.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.SwordsSpellbookSpells].Points;
            }

            from.CloseGump(typeof(SwordsSpellbookGump));
            from.SendGump(new SwordsSpellbookGump(this));
        }

        public SwordsSpellbook(Serial serial) : base(serial)
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

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new SwordsSpellbookEntry(player, this));
            }
        }
    }

    // Custom Context Menu Entry for the Sword Spellbook
    public class SwordsSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private SwordsSpellbook m_Spellbook;

        public SwordsSpellbookEntry(PlayerMobile player, SwordsSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new SwordsSkillTree(m_Player));
        }
    }
}
