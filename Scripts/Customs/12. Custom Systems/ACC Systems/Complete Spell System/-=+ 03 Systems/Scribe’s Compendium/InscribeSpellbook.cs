using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.Commands;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.InscribeMagic
{
    public class InscribeSpellbook : CSpellbook
    {
        public override School School { get { return School.ScribesCompendium; } }

        [Constructable]
        public InscribeSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public InscribeSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public InscribeSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2730;
            Name = "Scribes Compendium";
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

            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                // Set the content based on the unlocked Inscription spells.
                this.Content = (ulong)profile.Talents[TalentID.InscribeSpells].Points;
            }

            from.CloseGump(typeof(InscribeSpellbookGump));
            from.SendGump(new InscribeSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new InscribeSpellbookEntry(player, this));
            }
        }

        public InscribeSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry to open the Inscription Skill Tree.
    public class InscribeSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private InscribeSpellbook m_Spellbook;

        public InscribeSpellbookEntry(PlayerMobile player, InscribeSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new InscriptionSkillTree(m_Player));
        }
    }
}
