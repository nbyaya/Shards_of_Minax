using System;
using System.Collections.Generic;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.ACC.CSS.Systems.DetectHiddenMagic
{
    public class DetectHiddenSpellbook : CSpellbook
    {
        public override School School { get { return School.InquisitorsManual; } }

        [Constructable]
        public DetectHiddenSpellbook() : this((ulong)0, CSSettings.FullSpellbooks)
        {
        }

        [Constructable]
        public DetectHiddenSpellbook(bool full) : this((ulong)0, full)
        {
        }

        [Constructable]
        public DetectHiddenSpellbook(ulong content, bool full) : base(content, 0xEFA, full)
        {
            Hue = 1900;
            Name = "Inquisitor's Manual";
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

            // Update the spellbook content based on activated detect hidden spells.
            if (from is PlayerMobile player)
            {
                var profile = player.AcquireTalents();
                this.Content = (ulong)profile.Talents[TalentID.DetectHiddenSpells].Points;
            }

            from.CloseGump(typeof(DetectHiddenSpellbookGump));
            from.SendGump(new DetectHiddenSpellbookGump(this));
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from is PlayerMobile player)
            {
                list.Add(new DetectHiddenSpellbookEntry(player, this));
            }
        }

        public DetectHiddenSpellbook(Serial serial) : base(serial)
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
}
