using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.TrackingMagic
{
    public class TrackingSpellbook : CSpellbook
    {
        public override School School { get { return School.TrackersGuide; } }

        [Constructable]
        public TrackingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public TrackingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public TrackingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2319;
            Name = "Trackers Guide";
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

            // Update the content based on the TrackingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.TrackingSpells].Points;
            }

            from.CloseGump(typeof(TrackingSpellbookGump));
            from.SendGump(new TrackingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new TrackingSpellbookEntry(player, this));
            }
        }

        public TrackingSpellbook(Serial serial) : base(serial)
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

    // Custom Context Menu Entry for opening the Tracking Skill Tree.
    public class TrackingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private TrackingSpellbook m_Spellbook;

        public TrackingSpellbookEntry(PlayerMobile player, TrackingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new TrackingSkillTree(m_Player));
        }
    }
}
