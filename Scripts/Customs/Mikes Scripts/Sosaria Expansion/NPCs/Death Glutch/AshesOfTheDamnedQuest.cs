using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class AshesOfTheDamnedQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Ashes of the Damned";

        public override object Description => 
            "The flames speak, friend. Not in words, but in warnings.\n\n" +
            "When the Gate last opened, they came—bone-charred, flame-cloaked, laughing even in death. " +
            "The Burned Bone Fragments they leave behind are more than remnants. They are wards. Shields. Keys.\n\n" +
            "Four will suffice. Four to hold the firestorm at bay. But none believe me. Not yet.\n\n" +
            "The Death Glutch burns again. Find the Fire-Scorched Skeletons, bring me their remains. Or be consumed when the prophecy comes to pass.";

        public override object Refuse => 
            "Turn away, then. But know this: the next time the moon bleeds red, you'll wish you listened to old Orryn.";

        public override object Uncomplete => 
            "The skeletons wait beneath ash and smoke. The fragments must still smolder with essence. I cannot use ash. I need memory.";

        public override object Complete =>
            "They burn still... You feel it, yes? The heat? The *warning*?\n\n" +
            "This is no mere superstition. These bones remember the last time the Gate opened. " +
            "And they scream.\n\n" +
            "Here. Take this. It may help you survive what’s coming. And remember—when the fire sings, don’t listen. Run.";

        public AshesOfTheDamnedQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BurnedBoneFragment), "Burned Bone Fragment", 4, 0x0ECA, 1175));
            AddReward(new BaseReward(typeof(Gold), 6000, "6000 Gold"));
            AddReward(new BaseReward(typeof(FireWardedBand), 1, "Fire-Warded Band"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x21, "You feel a strange warmth linger around your fingertips...");
            Owner.PlaySound(0x208); // eerie hum
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
