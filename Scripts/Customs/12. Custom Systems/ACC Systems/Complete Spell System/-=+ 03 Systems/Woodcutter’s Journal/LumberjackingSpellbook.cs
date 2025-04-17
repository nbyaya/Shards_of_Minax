using System;
using Server.ContextMenus;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class LumberjackingSpellbook : CSpellbook
    {
        public override School School { get { return School.WoodcuttersJournal; } }

        [Constructable]
        public LumberjackingSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public LumberjackingSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public LumberjackingSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 555;
            Name = "Woodcutters Journal";
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

            // Update the content based on the LumberjackingSpells talent.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.LumberjackingSpells].Points;
            }

            from.CloseGump(typeof(LumberjackingSpellbookGump));
            from.SendGump(new LumberjackingSpellbookGump(this));
        }

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (from is PlayerMobile player)
			{
				list.Add(new LumberjackingSpellbookEntry(player, this));
			}
		}


        public LumberjackingSpellbook(Serial serial) : base(serial)
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

	// Custom Context Menu Entry
	public class LumberjackingSpellbookEntry : ContextMenuEntry
	{
		private PlayerMobile m_Player;
		private LumberjackingSpellbook m_Spellbook;

		public LumberjackingSpellbookEntry(PlayerMobile player, LumberjackingSpellbook spellbook)
			: base(1078990) // Custom Cliloc for Skill Tree
		{
			m_Player = player;
			m_Spellbook = spellbook;
		}

		public override void OnClick()
		{
			if (m_Player == null || m_Spellbook == null)
				return;

			m_Player.SendGump(new LumberjackingSkillTree(m_Player));
		}
	}	
}
