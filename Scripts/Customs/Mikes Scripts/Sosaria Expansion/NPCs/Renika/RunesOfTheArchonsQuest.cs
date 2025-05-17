using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class RunesOfTheArchonsQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Runes of the Archons";

        public override object Description => 
            "History is not dead. It simply waits to be understood.\n\n" +
            "The Mountain Stronghold was not merely a fortress—it was a sanctuary of the Skyborne Archons. " +
            "Their language has no surviving translations, but I believe fragments remain, etched into runestones once embedded in their sentinels.\n\n" +
            "You must retrieve four such fragments from the Power Golems that still guard the sanctum. They will not yield them willingly.\n\n" +
            "If you return with the runes, I may finally break the Archon cipher... and perhaps, more.";

        public override object Refuse => 
            "Fear is a poor guide, adventurer. But it does keep people alive. I’ll wait. This Stronghold isn’t going anywhere.";

        public override object Uncomplete =>
            "You haven’t retrieved them yet? The Power Golems may seem lifeless until disturbed. But disturb them you must.";

        public override object Complete =>
            "By the stars... these are genuine. The runes are complex—layered like chords in a song. Each one pulses with residual energy. You feel that, don’t you?\n\n" +
            "You’ve given me more than stone. You’ve given me *hope.* Take this reward—and remember: knowledge like this often costs far more than gold.";

        public RunesOfTheArchonsQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(RunedStoneFragment), "Runed Stone Fragment", 4, 0x136A, 1109));
            AddReward(new BaseReward(typeof(Gold), 8500, "8500 Gold"));
            AddReward(new BaseReward(typeof(AncientRuneTalisman), 1, "Ancient Rune Talisman"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Runes of the Archons quest!");
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
