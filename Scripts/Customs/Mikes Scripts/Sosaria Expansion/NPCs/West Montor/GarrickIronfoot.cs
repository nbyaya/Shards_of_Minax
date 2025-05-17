using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BuryTheBlazingBonesQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bury the Blazing Bones"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Garrick Ironfoot*, a burly apprentice with soot-stained hands, hammer slung at his hip, eyes burning with frustration.\n\n" +
                    "“Blast it! My master's anvil—it’s crackin’ and sparkin’ like a cursed forge! All ‘cause of those **BlazingBones** in the Gate of Hell!”\n\n" +
                    "“The heat of their cursed embers warps our tools. Can’t forge a decent helm, can’t mend a blade. My grandfather once salvaged fallen knights’ armor from those halls—but now the place burns with death.”\n\n" +
                    "“Go. Find them. Destroy them. Snuff out their embers before they ruin all we’ve built.”\n\n" +
                    "**Slay the BlazingBones** and bring silence back to our forge.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If you won’t help, I’ll find someone who will. But if the forge fails, we all suffer.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The forge still screams. My tools still twist. Those BlazingBones live, and we suffer for it.";
            }
        }

        public override object Complete
        {
            get
            {
                return "They’re gone? Truly? The forge... it sings again. Steel cool and true.\n\n" +
                       "You’ve done more than just kill—**you’ve saved our craft**. Here, take this: *StormforgedHelm*. May it shield you as you shielded our future.";
            }
        }

        public BuryTheBlazingBonesQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlazingBones), "BlazingBones", 1));
            AddReward(new BaseReward(typeof(StormforgedHelm), 1, "StormforgedHelm"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bury the Blazing Bones'!");
            Owner.PlaySound(CompleteSound);
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
    }

    public class GarrickIronfoot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BuryTheBlazingBonesQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public GarrickIronfoot()
            : base("the Smithy Apprentice", "Garrick Ironfoot")
        {
        }

        public GarrickIronfoot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1001; // Soot-darkened skin tone
            HairItemID = 0x203B; // Short hair
            HairHue = 1107; // Ashen black
            FacialHairItemID = 0x2041; // Full beard
            FacialHairHue = 1107; // Ashen black
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedChest() { Hue = 2406, Name = "Ember-Touched Studded Chest" });
            AddItem(new LeatherArms() { Hue = 2406, Name = "Forge-Hardened Bracers" });
            AddItem(new LeatherGloves() { Hue = 2412, Name = "Ashen Forge Mitts" });
            AddItem(new StuddedLegs() { Hue = 1811, Name = "Ironfoot’s Leggings" });
            AddItem(new LeatherCap() { Hue = 1819, Name = "Cinderproof Cap" });
            AddItem(new HalfApron() { Hue = 1824, Name = "Smith's Apprentice Apron" });
            AddItem(new Boots() { Hue = 2101, Name = "Coal-Stained Boots" });

            AddItem(new SmithSmasher() { Hue = 2408, Name = "Garrick’s Work Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Tool Pouch";
            AddItem(backpack);
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
    }
}
