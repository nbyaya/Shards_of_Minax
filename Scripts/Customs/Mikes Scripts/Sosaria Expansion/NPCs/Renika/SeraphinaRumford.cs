using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class HauntedGallopQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Haunted Gallop"; } }

        public override object Description
        {
            get
            {
                return
                    "You are greeted by *Seraphina Rumford*, the poised innkeeper of the **Nautical Nymph**.\n\n" +
                    "Her eyes flick to the window, then to you, voice firm but laced with worry:\n\n" +
                    "“You feel it too, don’t you? The tension in the air, the cold gallop at night. Guests won’t stay. They flee at the first sound of those *phantom hooves*. I once hosted royalty here... now I barely host anyone.”\n\n" +
                    "“A **BoneSteed**, they say—risen from the Stronghold yonder. Drawn to our finest breeds, breaking stalls, chasing dreams.”\n\n" +
                    "“I’ve seen the prints. Hoofmarks glowing faintly, even at dawn. It’s tethered by necromancy—I know it. Break its hold, and my stables may breathe again.”\n\n" +
                    "**Slay the BoneSteed** that rides from Mountain Stronghold. Help me save my home... and the souls of the steeds I cherish.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the hoofbeats that chill the air. I pray you don’t hear them close.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The hoofprints still glow. The guests still whisper. Please, end this.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So it’s done? The stables are silent. The hoofprints fade.\n\n" +
                       "You’ve not only saved my inn—you’ve honored the memory of noble steeds who once danced for kings.\n\n" +
                       "Take this: *The QuestWineRack.* May it hold the finest, just as my halls shall once again.";
            }
        }

        public HauntedGallopQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BoneSteed), "BoneSteed", 1));
            AddReward(new BaseReward(typeof(QuestWineRack), 1, "QuestWineRack"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Haunted Gallop'!");
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

    public class SeraphinaRumford : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(HauntedGallopQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBInnKeeper());
        }

        [Constructable]
        public SeraphinaRumford()
            : base("the Innkeeper", "Seraphina Rumford")
        {
        }

        public SeraphinaRumford(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1153; // Pale, moonlit skin
            HairItemID = 0x2048; // Long hair
            HairHue = 1150; // Silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2075, Name = "Moonlit Gown" }); // Deep sea blue
            AddItem(new Cloak() { Hue = 2101, Name = "Tidewoven Cloak" }); // Ocean teal
            AddItem(new FeatheredHat() { Hue = 2106, Name = "Mist-Crested Hat" }); // Pale mist
            AddItem(new Shoes() { Hue = 1150, Name = "Ghoststep Slippers" }); // Pale grey-blue
            AddItem(new BodySash() { Hue = 2107, Name = "Whispering Sash" }); // Soft seafoam

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Innkeeper’s Satchel";
            AddItem(backpack);
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
