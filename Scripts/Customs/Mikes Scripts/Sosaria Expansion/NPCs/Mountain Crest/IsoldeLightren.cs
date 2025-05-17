using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ExtinguishTheGlowQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Extinguish the Glow"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Isolde Lightren*, the Wisp Scholar of Mountain Crest, her figure delicate against the glimmering frost.\n\n" +
                    "She clutches a scroll, light dancing across its surface, reflecting fear and fascination.\n\n" +
                    "“You’ve seen them, haven’t you? The wisps... dancing lights, leading many to their doom.”\n\n" +
                    "“I’ve mapped their trails, studied their hues. They’re predictable—except for one. **The Icelight Wisp**.”\n\n" +
                    "“It shouldn’t exist, not in this form. It shifts colors in fear, lures the unwary into the chasms of the Ice Cavern. Explorers have died following its glow. This rogue must be erased. It disrupts the balance I’ve spent years understanding.”\n\n" +
                    "**Slay the Icelight Wisp**, and return peace to the frozen paths.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand. But beware—without action, more will be lost to its light.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it dances? I feel its pull in my dreams, see its glow in every shadow of ice.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The light fades... You’ve done what I could not. The patterns realign, and the caverns may rest.\n\n" +
                       "Take this *SpaceRaceCache*, a relic entrusted to me by sky-bound scholars. It holds knowledge—and perhaps something more.";
            }
        }

        public ExtinguishTheGlowQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IcelightWisp), "Icelight Wisp", 1));
            AddReward(new BaseReward(typeof(SpaceRaceCache), 1, "SpaceRaceCache"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Extinguish the Glow'!");
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

    public class IsoldeLightren : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ExtinguishTheGlowQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer());
        }

        [Constructable]
        public IsoldeLightren()
            : base("the Wisp Scholar", "Isolde Lightren")
        {
        }

        public IsoldeLightren(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 100);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Elf;

            Hue = 1153; // Pale blue skin tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1150; // Silvery white
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1152, Name = "Glacial Shroud" }); // Frosty blue hue
            AddItem(new FancyDress() { Hue = 1153, Name = "Wispweave Gown" }); // Pale iridescent fabric
            AddItem(new ElvenBoots() { Hue = 1154, Name = "Miststep Boots" }); // Ice-grey boots

            AddItem(new MagicWand() { Hue = 1157, Name = "Lightren’s Rod" }); // Pale, shimmering wand

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scholar's Satchel";
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
