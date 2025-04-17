using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ACC.CSS.Systems.FishingMagic;
using Server.Commands;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class FishingSpellbook : CSpellbook
    {
        public override School School { get { return School.AnglersGuide; } }

        [Constructable]
        public FishingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public FishingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public FishingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1174;
            Name = "Anglers Guide";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                if (SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions(from, this.School))
                    return;
            }

            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                if (profile.Talents.ContainsKey(TalentID.FishingSpells))
                    this.Content = (ulong)profile.Talents[TalentID.FishingSpells].Points;
            }

            from.CloseGump(typeof(FishingSpellbookGump));
            from.SendGump(new FishingSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
                list.Add(new FishingSpellbookEntry(player, this));
        }

        public FishingSpellbook(Serial serial) : base(serial)
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

    public class FishingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private FishingSpellbook m_Spellbook;

        public FishingSpellbookEntry(PlayerMobile player, FishingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new FishingSkillTree(m_Player));
        }
    }
}
