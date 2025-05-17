using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WarriorWeaveQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Warrior's Weave"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Orla Windrider*, Sailmaker of Renika, hunched over a half-stitched sail.\n\n" +
                    "Her hands are quick, precise, yet her brow is furrowed in frustration.\n\n" +
                    "“These winds aren’t natural,” she mutters. “Not since that **GraniteWarrior** stirred. Its magic-bound gusts tear through my sails like knives. And yet...”\n\n" +
                    "“Its banner—woven from enchanted stone threads—could make a sail unlike any other. A masterpiece worthy of the Storm-Sail Guild.”\n\n" +
                    "“Slay the **GraniteWarrior**, bring me its banner, and I’ll craft a sail to still the storm.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware these shores. Without that banner, every voyage from Renika risks ruin.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "No luck? The winds grow wilder. My stitches fray faster than I can mend.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it! The banner… it’s more than I imagined.\n\n" +
                       "*Orla’s eyes gleam as she runs her fingers over the stony threads.*\n\n" +
                       "“With this, I’ll tame the wildest gales. And for you? These—*PlatformSneakers*, worn by the swiftest riggers of Renika. Step sure, and the sea won’t claim you.”";
            }
        }

        public WarriorWeaveQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GraniteWarrior), "GraniteWarrior", 1));
            AddReward(new BaseReward(typeof(PlatformSneakers), 1, "PlatformSneakers"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Warrior's Weave'!");
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

    public class OrlaWindrider : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WarriorWeaveQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker());
        }

        [Constructable]
        public OrlaWindrider()
            : base("the Sailmaker", "Orla Windrider")
        {
        }

        public OrlaWindrider(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1023; // Sun-kissed tan
            HairItemID = 0x203B; // Long hair
            HairHue = 1161; // Seafoam green
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Wave-Stitched Blouse" }); // Deep ocean blue
            AddItem(new LongPants() { Hue = 1367, Name = "Tide-Wrapped Slacks" }); // Washed teal
            AddItem(new HalfApron() { Hue = 2101, Name = "Storm-Sail Apron" }); // Grey-blue
            AddItem(new Sandals() { Hue = 2406, Name = "Deckrunner’s Sandals" }); // Weathered brown
            AddItem(new FeatheredHat() { Hue = 1164, Name = "Windrider’s Cap" }); // Pale sea green, with a feather resembling a sail

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Bolt of Sailcloth";
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

