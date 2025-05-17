using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TunnelTerrorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Tunnel Terror"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Helga Deepdelve*, Death Glutch’s seasoned Mine Forewoman, standing amid cracked stone and smoldering lanterns.\n\n" +
                    "Her armor is dulled by soot, but her eyes gleam with iron resolve.\n\n" +
                    "“You hear that? That groan in the rock? That’s not just the mine settling—it’s something *alive*.”\n\n" +
                    "“A week back, we found supports chewed through like driftwood. Set traps. Didn’t work. I *saw* it—twice the size of a man, pale as bone, skittering near the Malidor Academy ruins. Damned Morlock.”\n\n" +
                    "“You kill it, and you’ll save more than my crew—you’ll save the whole town from a cave-in.”\n\n" +
                    "**Slay the Morlock** lurking near the Academy before it brings the mountain down on us all.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then pray the mountain holds, stranger. We’ve only got so many timbers left to prop it up.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive, is it? I can hear the tunnels crying out… We don’t have long.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So it’s dead? Good. That beast nearly cost me everything.\n\n" +
                       "Here. Take this—*ZorasFins*. It's more than you deserve, but I owe you more than words.\n\n" +
                       "And if you ever hear the tunnels groan again… come find me.";
            }
        }

        public TunnelTerrorQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Morlock), "Morlock", 1));
            AddReward(new BaseReward(typeof(ZorasFins), 1, "ZorasFins"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Tunnel Terror'!");
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

    public class HelgaDeepdelve : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TunnelTerrorQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMiner());
        }

        [Constructable]
        public HelgaDeepdelve()
            : base("the Mine Forewoman", "Helga Deepdelve")
        {
        }

        public HelgaDeepdelve(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 75, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Weathered skin tone
            HairItemID = 0x2048; // Braided ponytail
            HairHue = 1108; // Iron-gray
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 1811, Name = "Forewoman's Hauberk" }); // Ashen brown
            AddItem(new StuddedLegs() { Hue = 1815, Name = "Deepdelve Trousers" }); // Dark stone gray
            AddItem(new StuddedGloves() { Hue = 1109, Name = "Rockgrip Gloves" }); // Dusty charcoal
            AddItem(new PlateHelm() { Hue = 1820, Name = "Tunnelguard Helm" }); // Worn iron
            AddItem(new HalfApron() { Hue = 2101, Name = "Stoneworker's Apron" }); // Slate blue
            AddItem(new Boots() { Hue = 1812, Name = "Miner's Stompers" }); // Deep black

            AddItem(new Pickaxe() { Hue = 2507, Name = "Deepdelve's Pick" }); // Rust-streaked

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Mine Pack";
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
