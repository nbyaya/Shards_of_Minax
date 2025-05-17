using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ClearingTheTunnelsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Clearing the Tunnels"; } }

        public override object Description
        {
            get
            {
                return
                    "Name's Brava. I'm forewoman here at Devil Guard and those damn mines used to *belong* to us miners.\n\n" +
                    "Since the collapse, the tunnels are crawling with Ghoul Miners—twisted versions of the crew we lost down there. " +
                    "They’re strong, fast, and angry. I don’t need sympathy—I need results.\n\n" +
                    "**Slay 10 Ghoul Miners** down in the Mines of Minax, and maybe we’ll start reclaiming what’s ours.";
            }
        }

        public override object Refuse { get { return "Suit yourself. Just don’t come crying when those things crawl up in your dreams."; } }

        public override object Uncomplete { get { return "Still too many of those ghouls down there. Get back in the tunnels."; } }

        public override object Complete { get { return "Well, I'll be. You’re tougher than you look. Take this—it belonged to my brother."; } }

        public ClearingTheTunnelsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GhoulMiner), "Ghoul Miners", 10));

            AddReward(new BaseReward(typeof(Gold), 1000, "1000 Gold"));
            AddReward(new BaseReward(typeof(BrothersMiningBand), 1, "Brother’s Mining Band"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Clearing the Tunnels'!");
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
