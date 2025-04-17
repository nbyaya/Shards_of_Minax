using System;
using Server.ContextMenus;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
	public class StealingSpellbook : CSpellbook
	{
		public override School School{ get{ return School.RoguesCompendium; } }

		[Constructable]
		public StealingSpellbook() : this( (ulong)0, false) // Removed CSSettings.FullSpellbooks to hardcode a false here, so only this book is affected
        {
		}

		[Constructable]
		public StealingSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public StealingSpellbook( ulong content, bool full ) : base(content, 0xEFA, full)
		{
			Hue = 2230;
			Name = "Rogues Compendium";
		}

		public override void OnDoubleClick(Mobile from)
		{
            PlayerMobile player = from as PlayerMobile;

			if ( from.AccessLevel == AccessLevel.Player )
			{
				//Container pack = from.Backpack;
				//if( !(Parent == from || (pack != null && Parent == pack)) )
				//{
					//from.SendMessage( "The spellbook must be in your backpack [and not in a container within] to open." );
					//return;
				//}
				//else
				if( SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions( from, this.School ) )
				{
					return;
				}
			}

            var profile = player.AcquireTalents();
            this.Content = (ulong)profile.Talents[TalentID.StealingSpells].Points;

            InvalidateProperties(); // Ensure the UI updates if properties change

            from.CloseGump( typeof( StealingSpellbookGump ) );
			from.SendGump( new StealingSpellbookGump( this ) );
        }

        // Override GetContextMenuEntries
        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new StealingSpellbookEntry(player, this));
            }
        }

        public StealingSpellbook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

    // Custom Context Menu Entry
    public class StealingSpellbookEntry : ContextMenuEntry
    {
        private PlayerMobile m_Player;
        private StealingSpellbook m_Spellbook;

        public StealingSpellbookEntry(PlayerMobile player, StealingSpellbook spellbook)
            : base(1078990) // Custom Cliloc for Skill Tree
        {
            m_Player = player;
            m_Spellbook = spellbook;
        }

        public override void OnClick()
        {
            if (m_Player == null || m_Spellbook == null)
                return;

            m_Player.SendGump(new StealingSkillTree(m_Player));
        }
    }
}
