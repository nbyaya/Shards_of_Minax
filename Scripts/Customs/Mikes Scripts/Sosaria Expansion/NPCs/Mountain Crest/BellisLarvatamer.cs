using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class LarvaeOfIceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Larvae of Ice"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Bellis Larvatamer*, her coat speckled with frost, her eyes gleaming with a strange fervor.\n\n" +
                    "She clutches a crystalline jar, inside which a peculiar mushroom glows faintly.\n\n" +
                    "“You see this? This is *Cryoshroom Vivaria*, one of the rarest fungal specimens known to Mountain Crest!”\n\n" +
                    "“But something’s gone wrong. Deep in the Ice Cavern, a monstrous larva—**the Icebound Larva**—has begun devouring my fungal crops.”\n\n" +
                    "“Its acidic spit... it’s corrosive enough to melt leather, ruin tools. But worse, it’s mutating my samples. I can’t allow it. The balance here is delicate.”\n\n" +
                    "“If you’re willing, brave the cold, avoid its spit, and **slay the Icebound Larva** before my research is lost forever.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then leave me to mourn my spores... May the Icebound Larva feast until it freezes over.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive, that slimy menace? My samples won't survive another day with it loose!";
            }
        }

        public override object Complete
        {
            get
            {
                return "It's done? The Larva is dead?\n\n" +
                       "Oh, my spores thank you! My research can now thrive again, safe from its acidic menace.\n\n" +
                       "Take this: **SOLDIERSMight**. It’s a token of gratitude, as rare as the Cryoshrooms themselves.";
            }
        }

        public LarvaeOfIceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IceboundLarva), "Icebound Larva", 1));
            AddReward(new BaseReward(typeof(SOLDIERSMight), 1, "SOLDIERSMight"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Larvae of Ice'!");
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

    public class BellisLarvatamer : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(LarvaeOfIceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBJewel());
        }

        [Constructable]
        public BellisLarvatamer()
            : base("the Entomologist", "Bellis Larvatamer")
        {
        }

        public BellisLarvatamer(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale, frost-bitten skin tone
            HairItemID = 0x2049; // Long Hair
            HairHue = 1153; // Frosty white-blue
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1152, Name = "Frostwoven Shroud" }); // Pale icy blue
            AddItem(new LeatherGloves() { Hue = 2101, Name = "Chillbite Gloves" }); // Dark navy
            AddItem(new ElvenBoots() { Hue = 2401, Name = "Tread of the Deep Ice" }); // Slate gray
            AddItem(new Skirt() { Hue = 2210, Name = "Mossthread Skirt" }); // Soft green
            AddItem(new FormalShirt() { Hue = 1120, Name = "Spore-Warden's Tunic" }); // Light green

            AddItem(new GnarledStaff() { Hue = 1153, Name = "Cryoshroom Crook" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Spore Pack";
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
