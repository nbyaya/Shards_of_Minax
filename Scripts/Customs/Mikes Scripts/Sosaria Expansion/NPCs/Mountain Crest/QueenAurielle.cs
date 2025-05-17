using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ThroneOfIceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Throne of Ice"; } }

        public override object Description
        {
            get
            {
                return
                    "Queen Aurielle, Dominion Princess of the Northern Courts, stands with regal poise, her cloak billowing like winter's breath.\n\n" +
                    "Her eyes, sharp as icicles, lock onto yours as she speaks:\n\n" +
                    "\"A queen's title is not merely her birthright—it is her destiny, her duty, her **command**. I have heard whispers of a false monarch within the Ice Cavern, claiming dominion over its beasts. Such impudence cannot stand.\"\n\n" +
                    "\"You, brave soul, will enter that frozen tomb and slay the **Icy Dominion Queen**. Let her death proclaim my sovereignty.\"\n\n" +
                    "**Slay the Icy Dominion Queen**, and prove there is no throne she may claim in my presence.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then know this: to stand idle while pretenders rise is to invite ruin. Leave my sight, and do not speak of loyalty again.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "She still reigns? Ice fractures, and I feel the chill of her defiance. Do not return until her crown lies shattered beneath your feet.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So... the cavern is silent. The beasts are masterless.\n\n" +
                       "**Well done.** You have served a true queen this day.\n\n" +
                       "Take this, a token of my esteem. Let it bind you to your strength, as ice binds the mountains.";
            }
        }

        public ThroneOfIceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IcyDominionQueen), "Icy Dominion Queen", 1));
            AddReward(new BaseReward(typeof(LeatherStrapBelt), 1, "Leather Strap Belt"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Throne of Ice'!");
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

    public class QueenAurielle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ThroneOfIceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBPlateArmor()); // Royal lineage, armored regality.
        }

        [Constructable]
        public QueenAurielle()
            : base("the Dominion Princess", "Queen Aurielle")
        {
        }

        public QueenAurielle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = 1152; // Ice-blue
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1150, Name = "Glacial Aegis" }); // Pale blue steel
            AddItem(new PlateArms() { Hue = 1150, Name = "Frostforged Sleeves" });
            AddItem(new PlateGloves() { Hue = 1150, Name = "Queen's Grasp" });
            AddItem(new PlateLegs() { Hue = 1150, Name = "Winterguard Greaves" });
            AddItem(new WingedHelm() { Hue = 1153, Name = "Aurielle's Diadem" }); // Slightly brighter
            AddItem(new Cloak() { Hue = 1153, Name = "Mantle of the Frozen Court" }); // Flowing icy cloak
            AddItem(new Boots() { Hue = 1109, Name = "Thronewalkers" });

            AddItem(new Kryss() { Hue = 1150, Name = "Shard of Dominion" }); // Ice-dagger styled weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Royal Satchel";
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
