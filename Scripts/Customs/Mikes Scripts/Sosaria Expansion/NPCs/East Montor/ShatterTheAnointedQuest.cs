using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class ShatterTheAnointedQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shatter the Anointed"; } }

        public override object Description
        {
            get
            {
                return
                    "You’re not the squeamish type, right?\n\n" +
                    "I'm Jothan Reeve—curator of forgotten things. Deep in the Pharaoh’s sanctum lie three **Anointed Bone Priests**—undead zealots who guard keys to a treasury sealed since the Age of Bones.\n\n" +
                    "Bring me their **Ceremonial Ankhs**, and I’ll show you relics that would make a paladin blush.";
            }
        }

        public override object Refuse { get { return "Then get out of my shadow before it curses you too."; } }

        public override object Uncomplete { get { return "Still breathing, huh? Then keep digging. The priests aren’t all down yet."; } }

        public override object Complete { get { return "Ah... the ankhs sing of ancient blood. You’ve done Sosaria a disservice—but me a favor. Here’s your reward, and perhaps access to... rarer curios."; } }

        public ShatterTheAnointedQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AnointedBonePriest), "Anointed Bone Priests", 3));
            AddReward(new BaseReward(typeof(Gold), 1500, "1500 Gold"));
            AddReward(new BaseReward(typeof(CeremonialAnkh), 1, "Ceremonial Ankh Replica"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shatter the Anointed'!");
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
