using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TornFleshQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Torn Flesh"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Coralia Tidewhisper*, a Water Diviner of Moon’s aqueducts.\n\n" +
                    "Her voice is steady, but concern clouds her eyes. \"The conduits bleed, stranger. A fiend known as the *Mummy Render* tears at our lifeblood—Moon’s sacred waters.\n\n" +
                    "If it poisons our supply, all of Moon could wither in thirst and madness. Will you *slay the Mummy Render* before its blades shatter our hope?\"";
            }
        }

        public override object Refuse { get { return "Then the waters remain at risk. May the sands not claim us first."; } }

        public override object Uncomplete { get { return "The conduits still run red? The Mummy Render must fall, or we shall all feel the thirst."; } }

        public override object Complete { get { return "You’ve done it—the waters flow pure once more. Accept these *Shattermarch Greaves*, forged in gratitude, and reinforced against corruption."; } }

        public TornFleshQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MummyRender), "MummyRender", 1));

            AddReward(new BaseReward(typeof(ShattermarchGreaves), 1, "ShattermarchGreaves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Torn Flesh'!");
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

    public class CoraliaTidewhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TornFleshQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGardener()); 
        }

        [Constructable]
        public CoraliaTidewhisper()
            : base("the Water Diviner", "Coralia Tidewhisper")
        {
        }

        public CoraliaTidewhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female Human Body
            Race = Race.Human;

            Hue = 1002; // Pale tone reflecting Moon's desert light
            HairItemID = 0x2049; // Long Hair
            HairHue = 1150; // Icy silver-blue
        }

        public override void InitOutfit()
        {
            // Unique Outfit with Water-Themed Hues
            AddItem(new MonkRobe() { Hue = 1260, Name = "Tideweaver’s Robe" }); // Deep sea blue
            AddItem(new Cloak() { Hue = 1267, Name = "Aquifer's Shroud" }); // Turquoise shimmer
            AddItem(new Sandals() { Hue = 1153, Name = "Miststep Sandals" }); // Soft grey-blue
            AddItem(new FlowerGarland() { Hue = 1152, Name = "Reedwoven Circlet" }); // Green-tinted crown
            AddItem(new Scepter() { Hue = 1261, Name = "Flowbinder Rod" }); // Pale aquamarine
            Backpack backpack = new Backpack();
            backpack.Hue = 2407; // Pale crystal hue
            backpack.Name = "Diviner's Satchel";
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
