using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Engines.Quests
{
    public class WailNoMoreQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wail No More"; } }

        public override object Description
        {
            get
            {
                return
                    "I am Nim Seeglow, keeper of harmony in the Ice Cavern's elemental winds.\n\n" +
                    "Lately, the Matriarchs of the Ice Wraiths scream in disharmony, and their howls shatter the balance I strive to maintain.\n\n" +
                    "Destroy **6 Ice Wraith Matriarchs** to still their shrieks and restore the song of frost.";
            }
        }

        public override object Refuse { get { return "Then tremble under the songless wind, traveler."; } }
        public override object Uncomplete { get { return "The Matriarchs still howl. My focus wavers..."; } }
        public override object Complete { get { return "At last... the silence sings true again. Take this shard—it resonates with the frost's will."; } }

        public WailNoMoreQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IceWraithMatriarch), "Ice Wraith Matriarchs", 6));
            AddReward(new BaseReward(typeof(FrostTunedShard), 1, "Frost-Tuned Shard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x480, "You've completed 'Wail No More'!");
            Owner.PlaySound(0x5C9);
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
