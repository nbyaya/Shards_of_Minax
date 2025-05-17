using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class FungusAmongUsQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Fungus Among Us";

        public override object Description => 
            "Bah! You smell it, don’t you? That pungent *promise*—the breath of visions yet to come. " +
            "Only one brew can pull them into focus, and I’m missing the final spark: *Ambercap Spores.*\n\n" +
            "They bloom on beasts deeper in Catastrophe. Nasty things—half-rotted, half-thinking. You bring me six, and I’ll brew you something… enlightening.\n\n" +
            "Mind the spores. Breathe wrong and you might dream yourself inside out.";

        public override object Refuse =>
            "Feh! You’ll come crawling back. The future *wants* to be seen. And I’m the only one with the recipe.";

        public override object Uncomplete =>
            "Still alive, I see. Barely. The beasts down there don’t go down easy. And neither do their spores.";

        public override object Complete => 
            "Ha! I can smell them already. Fresh, pulsing with vision! Gods, you’ve done it. " +
            "This batch will turn dreams into prophecy. Here—have a sip, if you’ve the nerve. You might see what comes next… or what *shouldn’t*.";

        public FungusAmongUsQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(AmbercapSpores), "Ambercap Spores", 6, 0x0D16, 2213));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
            AddReward(new BaseReward(typeof(ClarityBrew), 1, "Clarity Brew"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x22, "You have completed the quest 'Fungus Among Us'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
