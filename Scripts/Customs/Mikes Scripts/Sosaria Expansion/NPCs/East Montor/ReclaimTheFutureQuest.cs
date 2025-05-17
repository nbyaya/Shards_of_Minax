using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ReclaimTheFutureQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Reclaim the Future";

        public override object Description => 
            "You there—yes, you. The Vault trembles again. I need something only you can fetch. " +
            "The Imperium once built constructs that ran on pure arcane will—Power Cores. They're still inside Vault 44, buried in those rusting husks.\n\n" +
            "Bring me five Imperium Power Cores. With them... well, I may be able to rekindle the past. Perhaps even speak to it.";

        public override object Refuse =>
            "So be it. Some fear what lies beneath. Others only fear what lies within.";

        public override object Uncomplete => 
            "The drones are dormant, mostly. Until they’re not. Don’t linger. The Vault watches.";

        public override object Complete => 
            "Yes... *yes*! The hum—do you feel that? The rhythm of memory and machinery. These Cores are fresh enough to resonate. " +
            "With time, I may awaken something truly monumental. You’ve done more than you know. And no, I won’t explain it—not yet.\n\n" +
            "Take this. Think of it as a down payment on the future.";

        public ReclaimTheFutureQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(ImperiumPowerCore), "Imperium Power Core", 5, 0x1F1C, 2966));
            AddReward(new BaseReward(typeof(Gold), 9000, "9000 Gold"));
            AddReward(new BaseReward(typeof(ClockworkAssembly), 1, "Prototype Assembly"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the quest 'Reclaim the Future'!");
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
