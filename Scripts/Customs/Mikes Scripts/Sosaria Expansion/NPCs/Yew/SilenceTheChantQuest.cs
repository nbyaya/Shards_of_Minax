using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class SilenceTheChantQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title => "Silence the Chant";

        public override object Description =>
            "The cultists grow bolder. Their chant deepens beneath Doom. " +
            "If it is not stopped, Sosaria may fracture under their words.\n\n" +
            "I am Calyx Thorn, watcher in shadow. I ask that you slay **7 Cult Voicebearers** in Doom Dungeon. Their voices must be silenced.";

        public override object Refuse => "Then the chant continues. You may yet regret inaction.";

        public override object Uncomplete => "The voices still echo below. Return only when you have silenced them all.";

        public override object Complete => 
            "You’ve done it. The silence is... heavy, but pure. Take this charm—it glows when dark voices gather.";

        public SilenceTheChantQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CultVoicebearer), "Cult Voicebearers", 7));
            AddReward(new BaseReward(typeof(WatcherCharm), 1, "Watcher’s Charm"));
            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You’ve completed 'Silence the Chant'!");
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
