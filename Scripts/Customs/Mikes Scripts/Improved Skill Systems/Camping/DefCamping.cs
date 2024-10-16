using System;
using Server;
using System.Collections;
using Server.Multis;
using Server.Targeting;
using Server.Items;
using Server.Regions;
using Server.Multis.Deeds;
using Server.ACC.CSS.Systems.ForagersGuidebook;

namespace Server.Engines.Craft
{
    public enum CampingRecipes
    {
        ExtraPack,
        TrailJournal,
        MagicMushroom,
        AdventurersRope,
        ShipDeed,
        HouseDeed
    }

    public class DefCamping : CraftSystem
    {
        public override SkillName MainSkill
        {
            get { return SkillName.Camping; }
        }

        public override int GumpTitleNumber
        {
            get { return 1099999; } // <CENTER>CAMPING MENU</CENTER>
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefCamping();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.0; // 0%
        }

        private DefCamping() : base(1, 1, 1.25)
        {
        }

        public override int CanCraft(Mobile from, ITool tool, Type itemType)
        {
            int num = 0;

            if (tool == null || tool.Deleted || tool.UsesRemaining <= 0)
                return 1044038; // You have worn out your tool!
            else if (!tool.CheckAccessible(from, ref num))
                return num; // The tool must be on your person to use.

            return 0;
        }

        public override void PlayCraftEffect(Mobile from)
        {
            // no animation, instant sound
            //if (from.Body.Type == BodyType.Human && !from.Mounted)
            //	from.Animate(9, 5, 1, true, false, 0);
            //new InternalTimer(from).Start();

            from.PlaySound(0x2A);
        }

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (failed)
            {
                if (lostMaterial)
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                else
                    return 1044157; // You failed to create the item, but no materials were lost.
            }
            else
            {
                if (quality == 0)
                    return 502785; // You were barely able to make this item.  It's quality is below average.
                else if (makersMark && quality == 2)
                    return 1044156; // You create an exceptional quality item and affix your maker's mark.
                else if (quality == 2)
                    return 1044155; // You create an exceptional quality item.
                else
                    return 1044154; // You create the item.
            }
        }

        public override void InitCraftList()
        {
            int index = -1;

            // Gear Menu
            AddCraft(typeof(ExtraPack), "Gear", "Extra Pack", 20.0, 20.0, typeof(Leather), "Leather", 10, 1044003);
            AddCraft(typeof(TrailJournal), "Gear", "Trail Journal", 30.0, 30.0, typeof(Leather), "Leather", 15, 1044003);
            AddCraft(typeof(MagicMushroom), "Gear", "Magic Mushroom", 40.0, 40.0, typeof(Leather), "Leather", 20, 1044003);
            AddCraft(typeof(AdventurersRope), "Gear", "Adventure Rope", 50.0, 50.0, typeof(Leather), "Leather", 25, 1044003);
			AddCraft(typeof(ForagersBook), "Gear", "Foragers Guide Book", 50.0, 50.0, typeof(Leather), "Leather", 25, 1044003);
			AddCraft(typeof(CampersBackpack), "Gear", "Campers Backpack", 50.0, 50.0, typeof(Leather), "Leather", 25, 1044003);

            // Ships Menu
            index = AddCraft(typeof(SmallBoatDeed), "Ships", "SmallBoatDeed", 60.0, 60.0, typeof(Board), "Boards", 50, 1044037);
            AddRes(index, typeof(Leather), "Leather", 50, 1044253);
            index = AddCraft(typeof(SmallDragonBoatDeed), "Ships", "SmallDragonBoatDeed", 60.0, 60.0, typeof(Board), "Boards", 50, 1044037);
            AddRes(index, typeof(Leather), "Leather", 50, 1044253);
            index = AddCraft(typeof(MediumBoatDeed), "Ships", "MediumBoatDeed", 80.0, 80.0, typeof(Board), "Boards", 80, 1044037);
            AddRes(index, typeof(Leather), "Leather", 80, 1044253);
            index = AddCraft(typeof(MediumDragonBoatDeed), "Ships", "MediumDragonBoatDeed", 80.0, 80.0, typeof(Board), "Boards", 80, 1044037);
            AddRes(index, typeof(Leather), "Leather", 80, 1044253);
            index = AddCraft(typeof(LargeBoatDeed), "Ships", "LargeBoatDeed", 100.0, 100.0, typeof(Board), "Boards", 150, 1044037);
            AddRes(index, typeof(Leather), "Leather", 150, 1044253);
            index = AddCraft(typeof(LargeDragonBoatDeed), "Ships", "LargeDragonBoatDeed", 100.0, 100.0, typeof(Board), "Boards", 150, 1044037);
            AddRes(index, typeof(Leather), "Leather", 150, 1044253);

            // Houses Menu
            index = AddCraft(typeof(TentDeed), "Houses", "Tent", 60.0, 60.0, typeof(Board), "Boards", 10, 1044037);
            AddRes(index, typeof(Leather), "Leather", 10, 1044253);
			index = AddCraft(typeof(WoodHouseDeed), "Houses", "Wood House", 75.0, 75.0, typeof(Board), "Boards", 150, 1044037);
            AddRes(index, typeof(Leather), "Leather", 10, 1044253);
			index = AddCraft(typeof(LogCabinDeed), "Houses", "Log Cabin", 90.0, 90.0, typeof(Board), "Boards", 200, 1044037);
            AddRes(index, typeof(Leather), "Leather", 20, 1044253);

            // Thieves Tools
            index = AddCraft(typeof(TrapGloves), "Thieves Tools", "Trap Gloves", 60.0, 60.0, typeof(Leather), "Leather", 12, 1044037);
            AddSkill(index, SkillName.RemoveTrap, 60.0, 60.0);
            index = AddCraft(typeof(TrapLegs), "Thieves Tools", "Trap Legs", 60.0, 60.0, typeof(Leather), "Leather", 12, 1044037);
            AddSkill(index, SkillName.RemoveTrap, 60.0, 60.0);
            index = AddCraft(typeof(TrapSleeves), "Thieves Tools", "Trap Sleeves", 60.0, 60.0, typeof(Leather), "Leather", 12, 1044037);
            AddSkill(index, SkillName.RemoveTrap, 60.0, 60.0);
            index = AddCraft(typeof(TrapTunic), "Thieves Tools", "Trap Tunic", 60.0, 60.0, typeof(Leather), "Leather", 12, 1044037);
            AddSkill(index, SkillName.RemoveTrap, 60.0, 60.0);
            index = AddCraft(typeof(TrapGorget), "Thieves Tools", "Trap Gorget", 60.0, 60.0, typeof(Leather), "Leather", 12, 1044037);
            AddSkill(index, SkillName.RemoveTrap, 60.0, 60.0);
            index = AddCraft(typeof(GlovesOfTheGrandmasterThief), "Thieves Tools", "Gloves Of The Grandmaster Thief", 100.0, 100.0, typeof(Leather), "Leather", 12, 1044037);
            AddSkill(index, SkillName.Stealing, 100.0, 100.0);
        }
    }
}
