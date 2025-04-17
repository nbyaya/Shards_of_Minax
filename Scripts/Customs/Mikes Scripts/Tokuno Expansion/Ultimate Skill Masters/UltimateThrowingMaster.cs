using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;
using Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters;

namespace Server.Mobiles
{
    [CorpseName("an Throwing master's corpse")]
    public class UltimateThrowingMaster : QuestGiver
    {
        public override bool IsInvulnerable { get { return true; } }
        [Constructable]
        public UltimateThrowingMaster()
        {
            Hue = Utility.RandomSkinHue();
            Body = 0x190;
            Blessed = true;

            AddItem(new Cloak(Utility.RandomMinMax(1, 3000))); // Perhaps greenish
            AddItem(new Bow());
            AddItem(new Robe(Utility.RandomMinMax(1, 3000)));
            AddItem(new Boots(Utility.RandomMinMax(1, 3000)));
            Utility.AssignRandomHair(this);
            Direction = Direction.East;
            Name = NameList.RandomName("male");
            Title = "the ultimate Throwing master";
            CantWalk = true;
        }

        public UltimateThrowingMaster(Serial serial) : base(serial)
        {
        }

        // We override OnDragDrop to change which QuestScroll is given. We will give an BushidoQuestScroll instead.
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            // Delegate the drag-drop handling to the QuestHandler
            return QuestHandler.HandleOnDragDrop(from, dropped, SkillName.Throwing, "Throwing", 0x4B5); // Bushido hue
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new QuestGiverContextMenuEntry(from, "Throwing Master", "Throwing"));
        }
    }
}
