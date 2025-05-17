using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DisarmFlamebotQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Disarm the Flamebot"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Cedric Cogwright*, a wiry tinkerer with soot-smudged goggles and hands that twitch with restless energy.\n\n" +
                    "His workshop buzzes with gears, springs, and erratic sparks. He doesn't look up from a whirring contraption as he speaks:\n\n" +
                    "\"It's not just any bot. **It's *my* bot!** Well... a prototype gone rogue. The Flamebot... it’s sabotaging my clockwork traps in the Gate of Hell. **If it ruptures one more power coil, the whole forge could explode!**\"\n\n" +
                    "\"The infernal energy down there—it scrambles the circuits. I've scavenged what I could from East Montor, but *this* mess... is beyond gears and grease.\"\n\n" +
                    "**Destroy the Flamebot** before it renders Cedric’s inventions into molten scrap.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the sparks fly, stranger. But mind this—if you smell brimstone from the town square, it's already too late.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still ticking, is it? Every moment that thing lives, my gears scream in pain. Get back down there!";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ha! That *buzzard bot* finally got what it deserved? You’ve saved more than my shop—you've spared West Montor from a fiery end.\n\n" +
                       "Here. This cape was infused with protection from necromantic energies... seemed fitting, given how close we danced with death. **Take it, and may your circuits always run clean.**";
            }
        }

        public DisarmFlamebotQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Flamebot), "Flamebot", 1));
            AddReward(new BaseReward(typeof(NecromancersCape), 1, "Necromancer's Cape"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Disarm the Flamebot'!");
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

    public class CedricCogwright : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DisarmFlamebotQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTinker(this));
        }

        [Constructable]
        public CedricCogwright()
            : base("the Clockwork Tinkerer", "Cedric Cogwright")
        {
        }

        public CedricCogwright(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 85, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2410; // Soot-tanned skin
            HairItemID = 0x2047; // Messy hair
            HairHue = 1108; // Greasy black
            FacialHairItemID = 0x203E; // Bushy beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1825, Name = "Cog-etched Shirt" }); // Brassy brown
            AddItem(new StuddedLegs() { Hue = 2207, Name = "Grease-Stained Trousers" }); // Oily black
            AddItem(new HalfApron() { Hue = 2501, Name = "Tinker's Apron" }); // Metallic grey
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Spark-Worn Gloves" });
            AddItem(new LeatherCap() { Hue = 1109, Name = "Goggles of Invention" }); // Soot-grey
            AddItem(new Boots() { Hue = 1812, Name = "Stompers of Precision" });

            AddItem(new TinkerTools() { Name = "Cogwright's Multitool", Movable = false });

            Backpack backpack = new Backpack();
            backpack.Hue = 1154;
            backpack.Name = "Clockwork Satchel";
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
