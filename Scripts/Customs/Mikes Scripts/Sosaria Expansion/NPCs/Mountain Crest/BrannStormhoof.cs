using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PaleSteedsCurseQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Pale Steed's Curse"; } }

        public override object Description
        {
            get
            {
                return
                    "Brann Stormhoof, master breeder of Mountain Crest, stands with furrowed brow beside his frost-bitten stables.\n\n" +
                    "“I’ve raised horses my whole life—Stormhoofs, bred strong for mountain wind and winter chill.”\n\n" +
                    "“But there’s something out there, in the Ice Cavern. **A beast that freezes the spirit of every steed it faces.** They say it’s a steed itself, but cursed—mane like ice crystals, breath like death.”\n\n" +
                    "“My best stallion now shivers like a foal, its eyes white with fear.”\n\n" +
                    "**Slay the Frostbitten Steed**. Break the curse. I can’t lose another.”\n\n" +
                    "Brann tightens a worn leather strap on his saddlebag. “I’ve got a chest—crafted of Locksley leather, strong and sure. It’s yours, if you help me.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the wind freeze us all. I’ll find another way—or bury the last of my Stormhoofs under snow.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The beast still rides? My steeds are restless... they sense it draws near.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So... it’s done. The chill’s lifted. My stallions breathe easy again.\n\n" +
                       "You’ve done more than slay a beast. You’ve saved a bloodline born for the heights.\n\n" +
                       "Take this *LocksleyLeatherChest*. Let it shield you, as my steeds now race free again.";
            }
        }

        public PaleSteedsCurseQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostbittenSteed), "Frostbitten Steed", 1));
            AddReward(new BaseReward(typeof(LocksleyLeatherChest), 1, "LocksleyLeatherChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Pale Steed's Curse'!");
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

    public class BrannStormhoof : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PaleSteedsCurseQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public BrannStormhoof()
            : base("the Horse Breeder", "Brann Stormhoof")
        {
        }

        public BrannStormhoof(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2411; // Weathered skin tone
            HairItemID = 0x203C; // Short Hair
            HairHue = 1150; // Frosted White
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2413, Name = "Stormhoof Jerkin" }); // Deep brown leather
            AddItem(new StuddedLegs() { Hue = 1150, Name = "Frostbitten Breeches" }); // Ice-gray
            AddItem(new LeatherGloves() { Hue = 1109, Name = "Stablemaster’s Grips" }); // Weathered leather
            AddItem(new Cloak() { Hue = 1153, Name = "Windcloak of Crest" }); // Light blue cloak
            AddItem(new TallStrawHat() { Hue = 2101, Name = "Breeder’s Brim" }); // Faded straw
            AddItem(new Boots() { Hue = 1812, Name = "Hoof-Trail Boots" }); // Sturdy black boots
            AddItem(new ShepherdsCrook() { Hue = 1109, Name = "Stormhoof Staff" }); // For guiding steeds

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Breeder's Pack";
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
