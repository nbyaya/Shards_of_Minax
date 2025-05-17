using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BunnysBaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bunny's Bane"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Nissa Softpaw*, Mountain Crest’s beloved animal handler, her eyes red from tears and frost clinging to her thick fur coat.\n\n" +
                    "She cradles a trembling rabbit, its fur matted with ice and fear.\n\n" +
                    "“Please… I don’t know what else to do. The warren—it’s gone mad. Something’s cursed them. A monstrous bunny, white as the glacier, with eyes colder than death itself. It turned them into frenzied attackers, and I can’t... I just can’t...”\n\n" +
                    "She takes a breath, fighting back another sob.\n\n" +
                    "**Slay the Icy Bunny** in the Ice Cavern before more of her beloved pets succumb to its chilling influence.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the winds be merciful. I fear I shall have no warren left by sunrise.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? The others—they're scratching at the walls now. Please, don’t leave them like this...";
            }
        }

        public override object Complete
        {
            get
            {
                return "You... you did it? It's over? I heard the silence return... like the mountain itself sighed in relief.\n\n" +
                       "Thank you, brave soul. Take this: *BanishingOrb*. It's all I have, but it's said to ward off lingering curses. May it serve you well, as you have served me.";
            }
        }

        public BunnysBaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IcyBunny), "Icy Bunny", 1));
            AddReward(new BaseReward(typeof(BanishingOrb), 1, "BanishingOrb"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bunny's Bane'!");
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

    public class NissaSoftpaw : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BunnysBaneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer()); // Suitable for her role
        }

        [Constructable]
        public NissaSoftpaw()
            : base("the Animal Handler", "Nissa Softpaw")
        {
        }

        public NissaSoftpaw(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Frosty white-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FurSarong() { Hue = 1150, Name = "Frostfur Wrap" }); // Icy blue
            AddItem(new FurBoots() { Hue = 2301, Name = "Snowbound Boots" }); // Soft grey
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Pelt-Handler's Gloves" });
            AddItem(new BearMask() { Hue = 2306, Name = "Warden's Mask" }); // Pale white
            AddItem(new Cloak() { Hue = 1150, Name = "Warrenkeeper's Cloak" }); // Matching icy blue cloak

            AddItem(new ShepherdsCrook() { Hue = 1102, Name = "Softpaw's Crook" }); // Light blue-tinted

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Warren Satchel";
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
