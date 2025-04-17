using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.HidingMagic
{
    public class HidingSpellbook : CSpellbook
    {
        public override School School { get { return School.ShadowsManual; } }

        [Constructable]
        public HidingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public HidingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public HidingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 2250;
            Name = "Shadows Manual";
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

            // Update the content based on HidingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (!profile.Talents.ContainsKey(TalentID.HidingSpells))
                    profile.Talents[TalentID.HidingSpells] = new Talent(TalentID.HidingSpells) { Points = 0 };

                this.Content = (ulong)profile.Talents[TalentID.HidingSpells].Points;
            }

            from.CloseGump(typeof(HidingSpellbookGump));
            from.SendGump(new HidingSpellbookGump(this));
        }

        public HidingSpellbook(Serial serial) : base(serial)
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
                list.Add(new HidingSpellbookEntry(player, this));
            }
        }
    }

    public class HidingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private HidingSpellbook m_Spellbook;

        public HidingSpellbookEntry(PlayerMobile player, HidingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new HidingSkillTree(m_Player));
        }
    }
}
