using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class OozeBeGoneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ooze Be Gone"; } }

        public override object Description
        {
            get
            {
                return
                    "Merek Sludgeheart, a grim-faced alchemist with frostbitten hands, clutches a flask that glows faintly blue.\n\n" +
                    "“The Ice Cavern’s cursed ooze has fouled my reagents, corrupted my brews. My hands ache from scrubbing acid off glass.”\n\n" +
                    "“A Glacial Ooze has lodged itself in the supply tunnels. It *splits* when struck—makes a mess of blades and bones alike.”\n\n" +
                    "“Burn its shards. End it. My work can’t continue until that thing’s gone.”\n\n" +
                    "**Slay the Glacial Ooze** to reopen the reagent lines, and you’ll have my gratitude... and MillStones for your trouble.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the ice will eat my work, and the ooze will spread. I hope your conscience bears that well.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still clings to the tunnels? Then the cold hasn’t claimed enough. I need that ooze gone.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The tunnels clear again... The frost won’t have me today. Take these **MillStones**. They won’t corrode like my work did.\n\n" +
                       "**You’ve not just slain a creature—you’ve kept the warmth of craft alive in this frozen spine.**";
            }
        }

        public OozeBeGoneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GlacialOoze), "Glacial Ooze", 1));
            AddReward(new BaseReward(typeof(MillStones), 1, "MillStones"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ooze Be Gone'!");
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

    public class MerekSludgeheart : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(OozeBeGoneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public MerekSludgeheart()
            : base("the Frostbitten Alchemist", "Merek Sludgeheart")
        {
        }

        public MerekSludgeheart(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 90);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2503; // Pale, frost-tinged skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1150; // Icy blue
            FacialHairItemID = 0x2041; // Medium beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new HoodedShroudOfShadows() { Hue = 1152, Name = "Chillweave Cloak" }); // Pale frost cloak
            AddItem(new Robe() { Hue = 2401, Name = "Alchemist’s Fume-Robe" });
            AddItem(new LeatherGloves() { Hue = 2413, Name = "Icebitten Gloves" });
            AddItem(new Boots() { Hue = 2506, Name = "Reagent-Stained Boots" });
            AddItem(new WizardsHat() { Hue = 1152, Name = "Frostcap of Distillation" });

            AddItem(new GnarledStaff() { Hue = 2502, Name = "Reagent Stirrer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Alchemist's Satchel";
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
