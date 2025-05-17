using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class QuellTheInferno : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Quell the InfernoElemental"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Korrin Stonebrew*, Master Brewer of West Montor’s famed Flame & Foam tavern.\n\n" +
                    "His face, ruddy from both sun and spirits, is contorted in frustration as he wipes sweat from his brow with a scorched rag.\n\n" +
                    "“Blast it! Me casks are burstin’, the ale’s boiling in the vats, and the brew’s turnin’ sour!”\n\n" +
                    "“I’ve traced the trouble to that cursed **Gate of Hell**. Some blasted **InfernoElemental** has been stokin' flames that creep all the way into me cellars! It’s upset the balance—me fermentation depends on cool air, y’see. Even the hops from Dawn’s cool fields can’t fix what’s boilin’ from below.”\n\n" +
                    "**Slay the InfernoElemental** that’s heating the earth beneath me tavern. Else, I’ll be servin’ boiled barley and tears for ale!”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "No? Then mayhaps you’ve a taste for burnt brew, friend. I’ll be drownin’ in smoke and ruin while that elemental dances.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You’ve yet to douse that flame? Me barrels are bucklin’ under the heat. Get ye gone, and be swift!";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ye did it! The flames’ve cooled, and me brew can breathe again!\n\n" +
                       "Take this, friend: *Beastmaster’s Tunic*. It won’t cool yer drink, but it’ll keep you fierce in any heat.\n\n" +
                       "Now, let’s toast to yer bravery—and to a fresh batch of ale!";
            }
        }

        public QuellTheInferno() : base()
        {
            AddObjective(new SlayObjective(typeof(InfernoElemental), "InfernoElemental", 1));
            AddReward(new BaseReward(typeof(BeastmastersTunic), 1, "Beastmaster’s Tunic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Quell the InfernoElemental'!");
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

    public class KorrinStonebrew : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(QuellTheInferno) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker()); // Brewer closest to Baker for vendor role
        }

        [Constructable]
        public KorrinStonebrew()
            : base("the Brewer", "Korrin Stonebrew")
        {
        }

        public KorrinStonebrew(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 75, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2410; // Sun-burnt tone
            HairItemID = 8251; // Medium long hair
            HairHue = 1358; // Rich brown
            FacialHairItemID = 8267; // Braided beard
            FacialHairHue = 1358;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1153, Name = "Emberweave Shirt" }); // Deep red hue
            AddItem(new HalfApron() { Hue = 2511, Name = "Scorch-Stained Apron" }); // Blackened leather
            AddItem(new ShortPants() { Hue = 2309, Name = "Brewmaster's Breeches" }); // Warm brown
            AddItem(new ThighBoots() { Hue = 1813, Name = "Ashwalker Boots" }); // Soot-black
            AddItem(new LeatherGloves() { Hue = 1825, Name = "Cask-Grip Gloves" }); // Charcoal hue
            AddItem(new WideBrimHat() { Hue = 2425, Name = "Foam-Crowned Hat" }); // Cream-tan
            AddItem(new GnarledStaff() { Hue = 2101, Name = "Brew Stirrer" }); // A rustic brewing staff

            Backpack backpack = new Backpack();
            backpack.Hue = 0;
            backpack.Name = "Brewer’s Pack";
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
