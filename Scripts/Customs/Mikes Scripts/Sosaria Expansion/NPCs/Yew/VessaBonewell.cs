using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WingsOfWasteQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wings of Waste"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself in the shadowed alcove of *Vessa Bonewell*, Boneworker of Yew.\n\n" +
                    "Around her, bleached bones are carved into intricate talismans, hung from twisted cords of sinew. Her hands are stained with dust, yet her eyes gleam with purpose.\n\n" +
                    "“The forest grows restless, stranger. The wards I’ve made—they weaken.”\n\n" +
                    "“A piece is missing... the shard of a DecayingBalron. It holds the essence of waste, of withering death, needed to bind spirits that prowl the Catastrophe.”\n\n" +
                    "“This Balron, it rots as it flies. Wings like gravecloth, breath like ruin. It haunts the depths of the Catastrophe, and its bones are unlike any other.”\n\n" +
                    "**Slay the DecayingBalron**, and bring me a bone shard from its carcass. Without it, Yew’s defenses will fall to corruption.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let Yew’s trees fall silent, and her bones lie unguarded. But know this: the Catastrophe will not wait.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? Then decay will seep from the Catastrophe, and I will have no talisman strong enough to hold it back.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ah... you have it. The shard is slick with death, yet sings with power.\n\n" +
                       "With this, I can shape a talisman that even the rot dares not cross.\n\n" +
                       "Take these *BoneArms*, carved in the old way, strengthened by this very shard. May they ward you, as I now ward Yew.";
            }
        }

        public WingsOfWasteQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DecayingBalron), "DecayingBalron", 1));
            AddReward(new BaseReward(typeof(BoneArms), 1, "BoneArms"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wings of Waste'!");
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

    public class VessaBonewell : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WingsOfWasteQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBLeatherWorker());
        }

        [Constructable]
        public VessaBonewell()
            : base("the Boneworker", "Vessa Bonewell")
        {
        }

        public VessaBonewell(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1023; // Pale bone-white skin
            HairItemID = 0x2049; // Long Hair
            HairHue = 1150; // Silver-gray
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 2406, Name = "Boneworker's Mantle" }); // Dark bone-grey
            AddItem(new BoneArms() { Hue = 2401, Name = "Carved Bone Sleeves" });
            AddItem(new BoneChest() { Hue = 2105, Name = "Etched Bone Bodice" });
            AddItem(new BoneGloves() { Hue = 2101, Name = "Talisman Crafter's Gloves" });
            AddItem(new Skirt() { Hue = 2301, Name = "Dust-Wrapped Skirt" });
            AddItem(new Sandals() { Hue = 2403, Name = "Worn Bone-Strap Sandals" });

            AddItem(new Cleaver() { Hue = 2419, Name = "Bone Shaper's Knife" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2412;
            backpack.Name = "Bundle of Bone Shards";
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
