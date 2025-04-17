using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.ContextMenus;
using Server.Mobiles;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class TacticsSpellbook : CSpellbook
    {
        public override School School { get { return School.StrategistsManual; } }

        [Constructable]
        public TacticsSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public TacticsSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public TacticsSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2295;
            Name = "Strategists Manual";
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

            // Update content based on the player's unlocked tactics spells.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.TacticsSpells].Points;
            }

            from.CloseGump(typeof(TacticsSpellbookGump));
            from.SendGump(new TacticsSpellbookGump(this));
        }

        public TacticsSpellbook(Serial serial) : base(serial)
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
                list.Add(new TacticsSpellbookEntry(player, this));
            }
        }
    }

    // Context menu entry to open the Tactics Skill Tree.
    public class TacticsSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private TacticsSpellbook m_Spellbook;

        public TacticsSpellbookEntry(PlayerMobile player, TacticsSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new TacticsSkillTree(m_Player));
        }
    }
}
