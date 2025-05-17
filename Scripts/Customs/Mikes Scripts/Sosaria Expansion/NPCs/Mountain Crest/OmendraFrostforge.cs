using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SmeltTheScourgeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Smelt the Scourge"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Omendra Frostforge*, the renowned blacksmith of Mountain Crest.\n\n" +
                    "Her hammer clangs against a glowing blade, sending sparks dancing in the frigid air. She doesn’t look up, but speaks with a voice like iron on stone.\n\n" +
                    "“Another cart, gone. My shipments of ore, crushed beneath frozen fists.”\n\n" +
                    "“A *Glacial Construct*, they say. Automaton born of ice and malice, forged in the Cavern’s heart.”\n\n" +
                    "“I can't shape steel without ore. And my forges don't burn bright without that lifeblood. I need someone to melt that beast down, piece by piece.”\n\n" +
                    "“Ordinary blades won’t do. You’ll have to aim for the joints—split the seams before the frost claims you too.”\n\n" +
                    "**Slay the Glacial Construct** blocking my ore supply, and the forges will sing your name.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Fine. Let the construct feast on my shipments until the mountain swallows us all.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it prowls? My forge cools with every moment. My hammer grows cold.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So... the frost shatters at last. You've saved more than just ore—you’ve rekindled Mountain Crest’s flame.\n\n" +
                       "Take this: *UmbraWarAxe.* Forged in shadow, quenched in the blood of frost. May it cleave your path true.";
            }
        }

        public SmeltTheScourgeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialConstruct), "Glacial Construct", 1));
            AddReward(new BaseReward(typeof(UmbraWarAxe), 1, "UmbraWarAxe"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Smelt the Scourge'!");
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

    public class OmendraFrostforge : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SmeltTheScourgeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public OmendraFrostforge()
            : base("the Master Blacksmith", "Omendra Frostforge")
        {
        }

        public OmendraFrostforge(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 90, 85);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1150; // Frosted steel color
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2401, Name = "Frostforge Plate" }); // Frosty-blue steel
            AddItem(new PlateLegs() { Hue = 2401, Name = "Forge-Tempered Greaves" });
            AddItem(new PlateGloves() { Hue = 1152, Name = "Anvil-Touched Gauntlets" }); // Dull metallic hue
            AddItem(new PlateHelm() { Hue = 1154, Name = "Omendra’s Smithing Helm" });
            AddItem(new HalfApron() { Hue = 1153, Name = "Coal-Stained Apron" }); // Blacksmith vibe
            AddItem(new Boots() { Hue = 1175, Name = "Ember-Stompers" });

            AddItem(new SmithSmasher() { Hue = 1150, Name = "Frostforge Hammer" }); // Her signature smithing weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Smith's Satchel";
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
