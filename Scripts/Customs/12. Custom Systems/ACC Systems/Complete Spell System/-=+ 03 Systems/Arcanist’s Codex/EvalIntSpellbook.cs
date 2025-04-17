using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class EvalIntSpellbook : CSpellbook
    {
        public override School School { get { return School.ArcanistsCodex; } }

        [Constructable]
        public EvalIntSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public EvalIntSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public EvalIntSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 100;
            Name = "Arcanists Codex";
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
            // Update content based on the EvalIntSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.EvalIntSpells].Points;
            }
            from.CloseGump(typeof(EvalIntSpellbookGump));
            from.SendGump(new EvalIntSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new EvalIntSpellbookEntry(player, this));
            }
        }

        public EvalIntSpellbook(Serial serial) : base(serial)
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

    // Custom context menu entry to open the EvalInt Skill Tree.
    public class EvalIntSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private EvalIntSpellbook m_Spellbook;

        public EvalIntSpellbookEntry(PlayerMobile player, EvalIntSpellbook spellbook)
            : base(1078990)
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new EvalIntSkillTree(m_Player));
        }
    }
}
