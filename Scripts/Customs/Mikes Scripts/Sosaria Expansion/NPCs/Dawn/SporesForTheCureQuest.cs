using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SporesForTheCureQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Spores for the Cure";

        public override object Description => 
            "Oh, thank the stars someone has come!\n\n" +
            "I’m Myla Tew, apothecary of Dawn. There’s a strange illness spreading through my livestock. Their eyes glaze, they shiver, and then... silence. No wound. No mark. Just stillness.\n\n" +
            "I’ve read about a rare fungus that thrives deep in the Catastrophe tunnels—*bioluminescent and potent.* I believe its spores could be the cure.\n\n" +
            "Please, bring me 8 Glowspore Clumps. I won’t forget your kindness.";

        public override object Refuse => 
            "I understand. The tunnels are... unkind. But if you change your mind, I’ll be here. Hoping.";

        public override object Uncomplete => 
            "Still searching? Be careful... the deeper you go, the more the fungus seems to *watch*. I can’t explain it.";

        public override object Complete =>
            "You found them? Oh... oh, thank you.\n\n" +
            "Even sealed, the clumps glow. Look at them... like little stars. With these, I can begin the brew. Maybe even a cure. Maybe...\n\n" +
            "Here—take this potion. It won’t make you invincible, but it should keep the worst of the spores at bay. You've done something good today. Truly.";

        public SporesForTheCureQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(GlowsporeClump), "Glowspore Clump", 8, 0x0D0C, 1278));
            AddReward(new BaseReward(typeof(SporewardPotion), 1, "Sporeward Potion"));
            AddReward(new BaseReward(typeof(Gold), 5000, "5000 Gold"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x2C, "You have completed 'Spores for the Cure'!");
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
            int version = reader.ReadInt();
        }
    }
}

