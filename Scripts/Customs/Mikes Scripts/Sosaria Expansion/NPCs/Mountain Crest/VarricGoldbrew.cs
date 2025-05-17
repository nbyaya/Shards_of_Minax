using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BrokenVialQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Broken Vial";

        public override object Description => 
            "*Varric Goldbrew*’s eyes burn with fury as he clutches a cracked potion bottle in one clawed hand.\n\n" +
            "“You see this? My best seller. The warming brew. Gone. All of it. *Stolen!* By some frost-cursed goblin freak holed up in the Cavern.”\n\n" +
            "He slams a tiny frostbomb on the ground—it fizzles cold. “He’s alchemizing with *my* stock! Freezing my reagents. Ruining them!”\n\n" +
            "“He calls himself the *Icy Alchemist.* Hah! Just a snow-snorting thief playing with powders he don’t understand.”\n\n" +
            "**Find the Icy Alchemist in the Ice Cavern and end him.** Bring back peace to my potions... and vengeance for my broken vials.”";

        public override object Refuse => 
            "“Fine! Freeze your fingers off, then! I’ll find some other warmblood with a spine!”";

        public override object Uncomplete => 
            "“Still alive, is he? That icy freak... still boiling my brews into frostbombs? *Get back in there!*”";

        public override object Complete => 
            "*Varric nearly tears the bladedancer arms from his bag in excitement.*\n\n" +
            "“You did it! That alchemist won’t be chilling any more potions!”\n\n" +
            "“These arms—crafted for bladesingers in the old frost courts. You’ll find them sturdy... and stylish.”\n\n" +
            "**Thanks for unfreezing my trade. Next time you’re in town, the first brew’s on me.”";

        public BrokenVialQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IcyAlchemistGoblin), "Icy Alchemist Goblin", 1));
            AddReward(new BaseReward(typeof(BladedancersPlateArms), 1, "Bladedancer's Plate Arms"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Broken Vial'!");
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

    public class VarricGoldbrew : MondainQuester
    {
        public override Type[] Quests => new Type[] { typeof(BrokenVialQuest) };
        public override bool IsActiveVendor => true;
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public VarricGoldbrew()
            : base("the Goblin Trader", "Varric Goldbrew")
        {
        }

        public VarricGoldbrew(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 90, 80);
            Body = 0x190;
            Female = false;
            Hue = 1025; // Pale green goblin skin
            HairItemID = 0x203B; // Wild spiky
            HairHue = 2205; // White-blue frost tone
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1109, Name = "Glacierweave Shirt" });
            AddItem(new ShortPants() { Hue = 1153, Name = "Potion-Pocketed Shorts" });
            AddItem(new HalfApron() { Hue = 2101, Name = "Alchemist’s Stained Apron" });
            AddItem(new Sandals() { Hue = 1172, Name = "Brew-Burnt Soles" });
            AddItem(new SkullCap() { Hue = 1289, Name = "Frostcap of Reagent Scenting" });
            AddItem(new GnarledStaff() { Hue = 2309, Name = "Vial-Tipped Rod" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Reagent Satchel";
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
