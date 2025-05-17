using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ChillOfTheSpecterQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Chill of the Specter"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Mira Windwhisper*, the Gale Caller of Pirate Isle.\n\n" +
                    "Her eyes shimmer with stormlight, hair braided with silver cords that seem to crackle faintly with static.\n\n" +
                    "“The winds, they used to sing. Now, they scream with icy wails.”\n\n" +
                    "“My instruments—my lutes, pipes, harps—they frost over, even near the hearth. The *ShiveringSpecter* has poisoned the breeze, and its chill silences my craft.”\n\n" +
                    "“This creature—this wraith—lurks in the shadows of **Exodus Dungeon**, freezing the very air with each mournful cry. I hear its dirges carried by the sea winds.”\n\n" +
                    "**Slay the ShiveringSpecter**, and let the winds of Pirate Isle be free to dance again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the winds grow colder, and may your nights be filled with silence.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the air howls, laden with frost. The Specter lingers, and so too does its curse.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The air... it’s warmer. The winds hum once more.\n\n" +
                       "You’ve slain the frost that stilled the songs. Take this **Exotic Ship in a Bottle**—its sails forever filled with the breeze of freedom, a token of the seas unshackled.";
            }
        }

        public ChillOfTheSpecterQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ShiveringSpecter), "ShiveringSpecter", 1));
            AddReward(new BaseReward(typeof(ExoticShipInABottle), 1, "Exotic Ship in a Bottle"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Chill of the Specter'!");
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

    public class MiraWindwhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ChillOfTheSpecterQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer()); // Bard-themed vendor for musical instruments and lore
        }

        [Constructable]
        public MiraWindwhisper()
            : base("the Gale Caller", "Mira Windwhisper")
        {
        }

        public MiraWindwhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 75, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Wind-swept silver-blue
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 1152, Name = "Stormweaver's Gown" }); // Deep azure
            AddItem(new Cloak() { Hue = 1260, Name = "Whispering Cloak" }); // Pale, misty gray
            AddItem(new Sandals() { Hue = 1109, Name = "Seafoam Sandals" });
            AddItem(new FeatheredHat() { Hue = 1266, Name = "Wind Dancer's Plume" }); // Silver-white
            AddItem(new BodySash() { Hue = 1151, Name = "Gale Sash" });

            AddItem(new ResonantHarp() { Hue = 2063, Name = "Frost-Laden Harp" }); // Special bard weapon prop
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
