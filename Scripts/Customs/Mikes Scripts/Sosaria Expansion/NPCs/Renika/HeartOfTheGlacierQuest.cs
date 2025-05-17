using System;
using Server;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class HeartOfTheGlacierQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Heart of the Glacier";

        public override object Description => 
            "Have you ever watched breath freeze midair? Ever carved crystal that *sings* beneath your blade?\n\n" +
            "I'm sculpting a piece unlike any other—a conduit for moonlight in the shape of memory. But I need Frostvein Crystals, and not just any... " +
            "only those birthed in the chambers of the Glacial Wraiths will resonate with the spellwork.\n\n" +
            "You’ll find them in the Ice Cavern, deep where the frost breathes. Bring me five, and I’ll give you more than coin—I’ll show you what art *remembers.*";

        public override object Refuse => 
            "No? I understand. Cold cuts deeper than blades. But if your heart ever thaws, I’ll be here... listening.";

        public override object Uncomplete => 
            "Still searching? The Ice Cavern doesn’t reveal its treasures to the faint-hearted. Listen closely—the frost hums when you're near a crystal.";

        public override object Complete => 
            "Yes! Look how they shimmer! Each one still hums with the chill of the wraiths. You’ve braved the heart of the glacier and returned unshattered—" +
            "that says something.\n\nTake this, and know: part of you is now etched into my next masterwork.";

        public HeartOfTheGlacierQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(FrostveinCrystal), "Frostvein Crystal", 5, 0x35DB, 1152));
            AddReward(new BaseReward(typeof(Gold), 9000, "9000 Gold"));
            AddReward(new BaseReward(typeof(RandomMagicJewelry), 1, "Pendant of Echoing Frost"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x3B2, "You have completed the 'Heart of the Glacier' quest!");
            Owner.PlaySound(0x20F);
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
