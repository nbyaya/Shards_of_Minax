using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShiverTheColossusQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shiver the Colossus"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Elric Frostbrook*, master ice carver of Yew, chiseling at a delicate sculpture of frost-touched lilies.\n\n" +
                    "His hands tremble not from cold, but from frustration, as he turns to you.\n\n" +
                    "\"They stole them! The blooms I need for the final touch, the essence of winter itself!\"\n\n" +
                    "\"The *IcyFrostTroll*—it lumbers through Catastrophe’s frozen depths, hoarding the winter blooms. But I need them—not just the blooms, the *horns*, too. For pigment, for texture. My patron demands perfection, and I promised him ice that holds the soul of frost.\"\n\n" +
                    "\"I cannot carve without them. I cannot *breathe* without them. Slay the beast, reclaim what it hoards, and the reward shall be yours: the *Hauberk of Silent Paths*, a gift once worn by those who tread unseen through the snow.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Without the blooms, the sculpture dies... and with it, my name, my craft. Let me hope you change your mind.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You have not yet bested the IcyFrostTroll? The cold deepens, and my patrons grow impatient.";
            }
        }

        public override object Complete
        {
            get
            {
                return "*Elric’s eyes widen as he sees the troll’s horns and frozen blooms.*\n\n" +
                       "\"You’ve done it. The frost sings in these horns, I can feel it. The blooms, still kissed by the troll’s hoarfrost... perfect.\"\n\n" +
                       "\"You’ve given me my masterpiece. And I give you this: *Hauberk of Silent Paths*. May it guard you, as you guarded my craft.\"";
            }
        }

        public ShiverTheColossusQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IcyFrostTroll), "IcyFrostTroll", 1));
            AddReward(new BaseReward(typeof(HauberkOfSilentPaths), 1, "Hauberk of Silent Paths"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shiver the Colossus'!");
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

    public class ElricFrostbrook : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShiverTheColossusQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter()); // Ice Carver linked to Carpenter for tools/supplies
        }

        [Constructable]
        public ElricFrostbrook()
            : base("the Ice Carver", "Elric Frostbrook")
        {
        }

        public ElricFrostbrook(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 60);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1152; // Pale, frost-bitten hue
            HairItemID = 0x203C; // Medium Long Hair
            HairHue = 1150; // Ice-blue
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new FurSarong() { Hue = 1150, Name = "Frostwoven Wrap" }); // Frost-blue
            AddItem(new FancyShirt() { Hue = 1153, Name = "Icecarver's Tunic" });
            AddItem(new LeatherGloves() { Hue = 1102, Name = "Carver’s Mitts" });
            AddItem(new Cloak() { Hue = 1150, Name = "Cloak of Shivering Winds" });
            AddItem(new ThighBoots() { Hue = 1151, Name = "Snowstompers" });
            AddItem(new FeatheredHat() { Hue = 1153, Name = "Winter’s Crest" });

            AddItem(new Pickaxe() { Hue = 1150, Name = "Ice Chisel" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1152;
            backpack.Name = "Frostcraft Satchel";
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
