using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RottingEssenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Rotting Essence"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Selara Mirefoot*, a reclusive herbalist whose shop clings to the edge of Death Glutch’s scorched cliffs.\n\n" +
                    "The air inside her shop is thick with incense and the sickly-sweet aroma of exotic blooms, but her eyes are sharp, filled with fear.\n\n" +
                    "“There’s a blight festering in the Malidor Witches Academy greenhouse. My finest blooms—flowers I’ve tended since girlhood—are being *corroded*, their lifeblood drained by a vile slug. **An Arcane Slug**, they say, a creature drawn to magic like flies to rot.”\n\n" +
                    "“It oozes through the greenhouse, trailing filth and death. If my blossoms wither, I’m ruined. And worse... I fear the slug’s poison will seep into the roots of the land.”\n\n" +
                    "“Please. Slay it. End this decay before it spreads.”\n\n" +
                    "**Kill the Arcane Slug** that infests the Academy’s greenhouse.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the roots shrivel and Death Glutch bear witness to another life undone by neglect.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it slithers? My blooms cry out in silence. If you do not act, I must watch them rot.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have done it... the stench has lifted. Already, my plants drink deep of clean earth.\n\n" +
                       "Take this: a *Colored Lamppost*, to brighten the darkest corners, as you have brightened my fading hope.";
            }
        }

        public RottingEssenceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ArcaneSlug), "Arcane Slug", 1));
            AddReward(new BaseReward(typeof(ColoredLamppost), 1, "Colored Lamppost"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Rotting Essence'!");
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

    public class SelaraMirefoot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RottingEssenceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist());
        }

        [Constructable]
        public SelaraMirefoot()
            : base("the Herbalist", "Selara Mirefoot")
        {
        }

        public SelaraMirefoot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Pale skin tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1109; // Moss green
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1425, Name = "Bogroot Robe" }); // Swampy green
            AddItem(new Cloak() { Hue = 2101, Name = "Sporeshade Cloak" }); // Deep purple
            AddItem(new FeatheredHat() { Hue = 2212, Name = "Mycelium Cap" }); // Fungal gray
            AddItem(new Sandals() { Hue = 2412, Name = "Mirefoot Sandals" }); // Muddy brown
            AddItem(new HalfApron() { Hue = 1822, Name = "Spore-Dusted Apron" }); // Pale cream

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herbal Satchel";
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
