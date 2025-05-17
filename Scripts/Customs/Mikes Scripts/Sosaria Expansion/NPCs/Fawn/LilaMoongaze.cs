using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RootsOfRotQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Roots of Rot"; } }

        public override object Description
        {
            get
            {
                return
                    "Lila Moongaze, the seer of Fawn, stands amidst a circle of silver-bound roots, her eyes clouded with vision’s haze.\n\n" +
                    "“The orchard breathes with the memories of my kin... but the Fencreep comes to choke it.”\n\n" +
                    "“In dreams I see it: tendrils of rot, slithering from the old grove, strangling bark and bone. My grandmother once drove it away with silver and song, but it stirs again, bolder.”\n\n" +
                    "“Cut it down, before it claims Fawn’s heart. The trees remember kindness, but they suffer in silence.”\n\n" +
                    "**Destroy the Fencreep** in the Wilderness before its rot spreads.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the trees must wither, their roots lost to rot. May the winds carry my warnings further than you would walk.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Fencreep still slithers? I feel its breath on the wind, damp and cold. The orchard withers as we wait.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it? The rot recedes, and the orchard sighs in relief. You’ve given life back to the roots.\n\n" +
                       "Take this: *Melodious Muffler*. My grandmother wove it with threads of song and silver—may it keep you safe from whispers of rot.";
            }
        }

        public RootsOfRotQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Fencreep), "Fencreep", 1));
            AddReward(new BaseReward(typeof(MelodiousMuffler), 1, "Melodious Muffler"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Roots of Rot'!");
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

    public class LilaMoongaze : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RootsOfRotQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBaker());
        }

        [Constructable]
        public LilaMoongaze()
            : base("the Seer", "Lila Moongaze")
        {
        }

        public LilaMoongaze(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(50, 60, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 33770; // Pale, mystical hue
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Moonlight silver
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2101, Name = "Moonwoven Robe" }); // Soft lavender, almost glowing
            AddItem(new Cloak() { Hue = 1150, Name = "Silverroot Cloak" }); // Deep silver-blue
            AddItem(new Sandals() { Hue = 2500, Name = "Spiritstep Sandals" }); // Pale green
            AddItem(new FeatheredHat() { Hue = 2101, Name = "Orchard Seer's Circlet" }); // Matches the robe

            Backpack backpack = new Backpack();
            backpack.Hue = 1153;
            backpack.Name = "Warding Satchel";
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
