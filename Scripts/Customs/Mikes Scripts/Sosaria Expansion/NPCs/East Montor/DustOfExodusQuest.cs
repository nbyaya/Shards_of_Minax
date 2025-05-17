using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;
using System.Collections.Generic;

namespace Server.Engines.Quests
{
    public class DustOfExodusQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Dust of Exodus"; } }

        public override object Description
        {
            get
            {
                return "Ah, another bold adventurer. I require something rare... spectral dust from the Fallen Knights that haunt Exodus Castle. " +
                       "Three samples should suffice—bring them to me, and perhaps we'll both walk away richer... and wiser.";
            }
        }

        public override object Refuse { get { return "Cowardice is understandable. The dead are unkind to the living."; } }

        public override object Uncomplete { get { return "Still no dust? The knights won’t vanquish themselves."; } }

        public override object Complete { get { return "You’ve done it! The dust... yes, I can feel the weight of history within it. Take this, as promised."; } }

        public DustOfExodusQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(CursedDust), "Cursed Dust Relic", 3, 0x0F8B, 1152)); // Hue 1152 is eerie purple-blue
            AddReward(new BaseReward(typeof(Gold), 7500, "7500 Gold"));
            AddReward(new BaseReward(typeof(RelicPouch), 1, "Alric’s Relic Pouch"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You have completed the Dust of Exodus quest!");
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
