using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FlameprowlQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Flameprowl"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Kieran Oakenshield*, the Woodcarver of Fawn, hunched over a scorched log, his tools laid out like fallen soldiers.\n\n" +
                    "His eyes meet yours, heavy with smoke and sorrow.\n\n" +
                    "“The Pyrrfelis…” he mutters. “A beast of flame and fang, it stalks our young oaks, turning saplings to ash before they take root.”\n\n" +
                    "“My carvings blacken overnight when it passes, and the forest mourns with me.”\n\n" +
                    "“I’ve crafted bracers against its fire, but I cannot wield them. I shape wood, not wield blades. You must hunt it. Drive the Pyrrfelis from these woods—or see them burn.”\n\n" +
                    "**Hunt the Pyrrfelis** before it claims Fawn’s heartwood.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the forest will burn... and I shall carve only charcoal.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still prowls? Each night I hear the crackle of flames in my dreams...";
            }
        }

        public override object Complete
        {
            get
            {
                return "The Pyrrfelis is no more? Then let the woods breathe again.\n\n" +
                       "Take this—*ShardCrest*. A token for one who has turned flame to ember, and shielded the saplings yet to grow.";
            }
        }

        public FlameprowlQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Pyrrfelis), "Pyrrfelis", 1));
            AddReward(new BaseReward(typeof(ShardCrest), 1, "ShardCrest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Flameprowl'!");
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

    public class KieranOakenshield : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FlameprowlQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public KieranOakenshield()
            : base("the Woodcarver", "Kieran Oakenshield")
        {
        }

        public KieranOakenshield(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 20);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1153; // Ashen tan
            HairItemID = 0x2049; // Short hair
            HairHue = 1107; // Soot-black
            FacialHairItemID = 0x203F; // Full beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 2421, Name = "Emberweave Shirt" }); // Deep red
            AddItem(new LeatherLegs() { Hue = 2105, Name = "Oak-Hardened Leggings" }); // Oak brown
            AddItem(new HalfApron() { Hue = 1811, Name = "Char-Marked Apron" }); // Charcoal gray
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Ash-Bound Gloves" }); // Pale gray
            AddItem(new Boots() { Hue = 1812, Name = "Forest-Treader Boots" }); // Forest green
            AddItem(new FeatheredHat() { Hue = 1824, Name = "Fireplume Hat" }); // Burnt orange

            Backpack backpack = new Backpack();
            backpack.Hue = 0;
            backpack.Name = "Woodcarver's Pack";
            AddItem(backpack);

            AddItem(new CarpentersHammer() { Hue = 2105, Name = "Oakenshield's Mallet" });
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
