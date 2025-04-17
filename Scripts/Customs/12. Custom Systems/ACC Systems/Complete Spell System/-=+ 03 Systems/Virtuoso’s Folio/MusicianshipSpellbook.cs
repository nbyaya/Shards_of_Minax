using System;
using Server.Commands;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class MusicianshipSpellbook : CSpellbook
    {
        public override School School { get { return School.VirtuososFolio; } }

        [Constructable]
        public MusicianshipSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public MusicianshipSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public MusicianshipSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1444;
            Name = "Virtuoso's Folio";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            // Update content based on MusicianshipSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.MusicianshipSpells].Points;
            }

            from.CloseGump(typeof(MusicianshipSpellbookGump));
            from.SendGump(new MusicianshipSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
                list.Add(new MusicianshipSpellbookEntry(player, this));
        }

        public MusicianshipSpellbook(Serial serial) : base(serial)
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

    // Context Menu Entry to open the Musicianship Skill Tree.
    public class MusicianshipSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private MusicianshipSpellbook m_Spellbook;

        public MusicianshipSpellbookEntry(PlayerMobile player, MusicianshipSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree (reuse or define as needed)
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new MusicianshipSkillTree(m_Player));
        }
    }
}
