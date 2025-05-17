using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WyrmsBaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Wyrm's Bane"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Oran Deepmind*, the Lorekeeper of Grey’s forgotten histories.\n\n" +
                    "A tall figure robed in deep ocean blues, his eyes shimmer like polished obsidian. Ancient scrolls hang from his belt, and a scent of salt and parchment surrounds him.\n\n" +
                    "“Long have I chronicled the sagas of our isle, but now the past fights back.”\n\n" +
                    "**“A Necrotic Wyrm festers in the collapsed galleries of Exodus Dungeon. Its bile flows through stone, eroding the murals of old, poisoning the stories we live by.”**\n\n" +
                    "“You must slay this beast. I have read of its decay, its power to heal even as it rots, and the doom it brings to memory itself.”\n\n" +
                    "**Bring me proof of its death, and I shall honor you with the Wyrm’s Bane Trophy, a mark of one who defends not just life—but legacy.”**";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then let the past crumble, and its truths be swallowed by the Wyrm’s bile.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Wyrm still thrives? The decay grows faster. I feel its breath in these walls.";
            }
        }

        public override object Complete
        {
            get
            {
                return "**You have done what I feared could not be done. The Necrotic Wyrm is dead, and the murals may yet be saved.**\n\n" +
                       "**Take this TrophyAward, a symbol that you have safeguarded not just our land, but our very stories.**";
            }
        }

        public WyrmsBaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(NecroticWyrm), "Necrotic Wyrm", 1));
            AddReward(new BaseReward(typeof(TrophieAward), 1, "Wyrm’s Bane Trophy"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Wyrm's Bane'!");
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

    public class OranDeepmind : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WyrmsBaneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMapmaker()); // Lorekeeper / Cartography background
        }

        [Constructable]
        public OranDeepmind()
            : base("the Lorekeeper", "Oran Deepmind")
        {
        }

        public OranDeepmind(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 30);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2406; // Slightly pale, parchment-toned
            HairItemID = 0x2048; // Long Hair
            HairHue = 1150; // Midnight Blue
            FacialHairItemID = 0x203F; // Medium Long Beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1154, Name = "Tidewoven Shirt" }); // Deep blue
            AddItem(new LongPants() { Hue = 1150, Name = "Sable Trousers" }); // Midnight black
            AddItem(new Cloak() { Hue = 1175, Name = "Cloak of Forgotten Tides" }); // Storm grey
            AddItem(new BodySash() { Hue = 1289, Name = "Lorekeeper's Sash" }); // Seafoam green
            AddItem(new Boots() { Hue = 1109, Name = "Wave-Steppers" }); // Slate gray
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Archivist’s Plume" }); // Matches cloak

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scroll Satchel";
            AddItem(backpack);

            AddItem(new ScribeSword() { Hue = 2101, Name = "Inkfang" }); // A ceremonial weapon, inkwell blue
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
