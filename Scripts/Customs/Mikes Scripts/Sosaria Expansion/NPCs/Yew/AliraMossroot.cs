using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DimTheVeinsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dim the Glowing Veins"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Alira Mossroot*, a wiry woman with eyes like the forest’s edge—green, sharp, and watchful.\n\n" +
                    "Dried herbs dangle from her belt, and her hands are stained with soil and sap. She speaks low, as if the woods themselves might overhear.\n\n" +
                    "“The Catastrophe stirs again. Its light—unnatural, wrong—creeps into the roots. My garden wilts under its glow.”\n\n" +
                    "“A fungus. Not like any other. It glows, and where it touches, life warps. I’ve seen bees spiral madly, birds fall from air, and herbs twist to poison.”\n\n" +
                    "“I need you to stop it. Find this **Bioluminescent Fungus**, deep in Catastrophe, and burn it from the world. Before its spores claim Yew.”\n\n" +
                    "**Slay the BioluminescentFungus** before its corruption spreads.”\n\n";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the forest will suffer, and the garden will darken. But I will not blame you. Only the light.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The fungus lives still? I feel its rot creeping closer—every breath now smells of decay.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The glow has dimmed? Bless the roots. You’ve done what I could not. My garden will live—and so may the forest.\n\n" +
                       "Take this: *HellspikeOfGlutch*. It’s not from me, but from a friend lost to Catastrophe. He’d want it wielded well.";
            }
        }

        public DimTheVeinsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BioluminescentFungus), "BioluminescentFungus", 1));
            AddReward(new BaseReward(typeof(HellspikeOfGlutch), 1, "HellspikeOfGlutch"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Dim the Glowing Veins'!");
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

    public class AliraMossroot : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DimTheVeinsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBHerbalist()); 
        }        
		
		[Constructable]
        public AliraMossroot()
            : base("the Mossroot Herbalist", "Alira Mossroot")
        {
        }

        public AliraMossroot(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 75, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Earthy tone
            HairItemID = 0x203C; // Long hair
            HairHue = 1326; // Mossy green
        }

        public override void InitOutfit()
        {
            AddItem(new PlainDress() { Hue = 1424, Name = "Rootwoven Dress" }); // Muted forest green
            AddItem(new Cloak() { Hue = 2212, Name = "Verdant Cloak" }); // Deep moss green
            AddItem(new Sandals() { Hue = 2302, Name = "Earthen Sandals" });
            AddItem(new FlowerGarland() { Hue = 1157, Name = "Herbalist's Garland" });
            AddItem(new HalfApron() { Hue = 2128, Name = "Pouch of Spores" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Gatherer's Satchel";
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
