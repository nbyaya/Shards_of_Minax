using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class VoicesOfTheVeinQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Voices of the Vein";

        public override object Description => 
            "The mines... they’re *not silent*. They scream in the dark. Whispers cling to the walls, and I hear them clearer each day.\n\n" +
            "The dead miners left more than bones behind—they left thoughts. Imprints. Echoes. " +
            "I need someone brave, or foolish, to go into the Mines of Minax and bring me five **Echo Stones**. They glimmer faintly and whisper warnings—" +
            "especially when near dark magic.\n\n" +
            "I believe one of them knows the name of a cultist... someone from the Doom Dungeon. Please—help me speak to them.";

        public override object Refuse => 
            "*sigh* I understand. The dead don't make easy company, do they? Still... I fear what happens if they remain unheard.";

        public override object Uncomplete => 
            "You haven’t found them yet? The Echo Stones won’t show themselves unless the darkness calls to them. Find the Lost Miners. They’re... holding on.";

        public override object Complete =>
            "You found them. You *found them*. Listen... just listen.\n\n" +
            "‘Warn them... beneath the pit… the heart beats again...’\n\n" +
            "One of these stones—it whispered a name. **Lemorin.** That name echoes in the Doom Dungeon. I’m sure of it.\n\n" +
            "Thank you. This might change everything. Take this—it’s more than just a reward. It’s protection.";

        public VoicesOfTheVeinQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(EchoStone), "Echo Stone", 5, 0x1422, 1154));
            AddReward(new BaseReward(typeof(Gold), 8000, "8000 Gold"));
            AddReward(new BaseReward(typeof(WhisperingPendant), 1, "Whispering Pendant"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x482, "You feel the weight of many souls lift... and settle.");
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
}
