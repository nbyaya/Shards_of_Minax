using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WanderersWoeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wanderer's Woe"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Olympia Seabreeze*, songstress of Renika, her lute resting gently in her arms.\n\n" +
                    "Her sea-green eyes flicker with frustration as she sighs, adjusting a scarf that shimmers like morning tides.\n\n" +
                    "“The sea sings with me, but lately, something else drowns out my voice. The *MountainWanderer*, a beast whose howls roll down from the Mountain Stronghold.”\n\n" +
                    "“I’ve tried to lift spirits with my songs, yet my fans complain—they can hear nothing but its wailing.”\n\n" +
                    "“I once sang for the Duke of Yew beneath orchard blossoms, my voice unchallenged. Now... this creature shames me in my own home.”\n\n" +
                    "**Silence the MountainWanderer**, brave soul. Let Renika ring again with song, not sorrow.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I shall endure the howling winds and mournful mountains alone... but music suffers for it.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it howls? The notes fade before I can breathe them. The mountain steals my song.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve returned—and I hear the silence! No, I hear the sea again... and my own voice.\n\n" +
                       "**The MountainWanderer is no more, and the melody returns to Renika.**\n\n" +
                       "Take this: *ReindeerFurCap*, a token from my days performing under Yew’s snowy branches. May it warm you as you’ve warmed my heart.";
            }
        }

        public WanderersWoeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MountainWanderer), "MountainWanderer", 1));
            AddReward(new BaseReward(typeof(ReindeerFurCap), 1, "ReindeerFurCap"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wanderer's Woe'!");
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

    public class OlympiaSeabreeze : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WanderersWoeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBChainmailArmor());
        }

        [Constructable]
        public OlympiaSeabreeze()
            : base("the Coastal Songstress", "Olympia Seabreeze")
        {
        }

        public OlympiaSeabreeze(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 70, 70);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Fair-skinned
            HairItemID = 0x2047; // Long hair
            HairHue = 1150; // Seafoam green
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1367, Name = "Shirt of Soothing Tides" }); // Light ocean blue
            AddItem(new Skirt() { Hue = 1266, Name = "Pearl-Woven Skirt" }); // Pearl white
            AddItem(new Cloak() { Hue = 1372, Name = "Mistfall Cloak" }); // Soft sea mist green
            AddItem(new Sandals() { Hue = 1153, Name = "Shoes of the Shoreline" }); // Azure blue
            AddItem(new FeatheredHat() { Hue = 1260, Name = "Breeze-Catcher Hat" }); // Pale aqua
            AddItem(new Lute() { Hue = 1150, Name = "Seafoam Lute" }); // Musical instrument, optional for ambiance

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Songstress's Satchel";
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
