using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WidowsWebQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Widow's Web"; } }

        public override object Description
        {
            get
            {
                return
                    "You find **Gundra Webbane**, an expert in all things venomous, tending to frostbitten silk samples near a brazier.\n\n" +
                    "Her eyes flicker with a cold fire as she speaks:\n\n" +
                    "**\"I lost more than love to that spider... I lost purpose. But its poison still burns me, even now.\"**\n\n" +
                    "**\"The Frosted Widow.** It lairs deep in the Ice Cavern’s eastern chasm, spinning webs that shimmer with death. Its venom doesn’t just kill—it *burns cold*. Like the void swallowing you, slowly... silently...\"\n\n" +
                    "**\"I need it dead. Not for peace, but for justice. Will you be the blade that severs its web?\"**\n\n" +
                    "**Slay the Frosted Widow Spider** and return with proof. Do this, and honor a bond broken by frost and fangs.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "**\"Then tread lightly, wanderer. The Widow weaves for all who pass... even those who turn away.\"**";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "**\"Still alive? Still weaving? The cold thickens, and the chasm yawns wider. Do not tarry.\"**";
            }
        }

        public override object Complete
        {
            get
            {
                return "**\"The Widow is dead... and yet, my heart still aches.\"**\n\n" +
                       "**\"But you've done what I could not. This cap, once worn by the War Herons, is yours. Wear it as a hunter of fates. And beware—the cold remembers.\"**";
            }
        }

        public WidowsWebQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(FrostedWidowSpider), "Frosted Widow Spider", 1));
            AddReward(new BaseReward(typeof(WarHeronsCap), 1, "WarHeronsCap"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Widow's Web'!");
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

    public class GundraWebbane : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WidowsWebQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBFurtrader());
        }

        [Constructable]
        public GundraWebbane()
            : base("the Arachnid Specialist", "Gundra Webbane")
        {
        }

        public GundraWebbane(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 75, 20);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Pale, frost-touched skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1150; // Icy blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1153, Name = "Web-Touched Robe" }); // Deep frost-blue
            AddItem(new LeatherGloves() { Hue = 2401, Name = "Venomweave Gloves" });
            AddItem(new ThighBoots() { Hue = 2407, Name = "Froststep Boots" });
            AddItem(new FeatheredHat() { Hue = 1154, Name = "Widow's Plume" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herbalist's Pouch";
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
