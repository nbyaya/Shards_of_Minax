using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
    public class ScryingOrb : Item
    {
        [Constructable]
        public ScryingOrb() : base(0xE2E)
        {
            Name = "Scrying Orb";
            LootType = LootType.Blessed;
        }

        public ScryingOrb(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;

            if (pm == null)
                return;

            if (pm.Skills[SkillName.Snooping].Base < 200)
            {
                pm.SendMessage("You must have at least 200 snooping skill to use this.");
                return;
            }

            pm.SendMessage("Whose backpack do you wish to scry?");
            pm.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(ScryTarget));
        }

        private void ScryTarget(Mobile from, object targeted)
        {
            PlayerMobile target = targeted as PlayerMobile;
			if (target == null)
            {
                from.SendMessage("You can only scry for players.");
                return;
            }

            if (target == null || !target.Player || !target.Alive || target.Backpack == null)
            {
                from.SendMessage("You fail to scry for that individual.");
                return;
            }

            if (target.NetState == null)
            {
                from.SendMessage("That player is not online.");
                return;
            }

            from.SendMessage("You scry into the backpack of " + target.Name);
            from.SendGump(new BackpackViewerGump(target.Backpack));
        }
    }

    // Custom Gump to view backpack contents
    public class BackpackViewerGump : Gump
    {
        public BackpackViewerGump(Container backpack) : base(50, 50)
        {
            AddPage(0);
            AddBackground(0, 0, 450, 450, 5054);

            AddLabel(100, 20, 1152, "Backpack Contents");

            int y = 40;
            foreach (Item item in backpack.Items)
            {
                AddLabel(55, y, 1153, item.Name ?? "(no name)");
                y += 20;
            }
        }
    }
}
