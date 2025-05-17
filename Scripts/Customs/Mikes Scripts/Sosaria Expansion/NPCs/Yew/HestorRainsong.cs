using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BoundDecayerQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "The Bound Decayer"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Hestor Rainsong*, a travelling minstrel turned reluctant witness to dark times in Yew.\n\n" +
                    "Clad in patchwork silks, his lute slung across his back, Hestor’s eyes dart nervously towards the shadowed treeline.\n\n" +
                    "“Once, I sang of brave hearts and haunted lands. Now I see them. The orcs, yes—but worse... **a Decaybound Mage**. An orc who dares twist life itself, binding decay like thread to needle.”\n\n" +
                    "“I penned laments for a warlock who walked this path, long ago. I thought them *songs*, not warnings. But the air... the trees... they wither.”\n\n" +
                    "“This mage is preparing something. A ritual. A blight that will consume Yew’s edges—and perhaps more. End him. Free the land. Let my song be of his *fall*, not his rise.”\n\n" +
                    "**Slay the Decaybound Orcish Mage** and disrupt the foul magic staining Yew’s borders.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "“Then I shall write of sorrow—and sing to trees too dead to hear me.”";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "“Still he chants? The earth *weeps*, stranger. The leaves twist. Yew won’t endure this long.”";
            }
        }

        public override object Complete
        {
            get
            {
                return "“It’s done? Truly?” Hestor clasps your hand, his eyes shining with both relief and lingering fear.\n\n" +
                       "“I shall weave your deed into song—a tale to warn and inspire.”\n\n" +
                       "“Take this, the **Wanderer’s Mantle of Mist**. May it carry you unseen, as I now travel freely again, unburdened by dread.”";
            }
        }

        public BoundDecayerQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DecayboundOrcishMage), "Decaybound Orcish Mage", 1));
            AddReward(new BaseReward(typeof(WanderersMantleOfMist), 1, "Wanderer's Mantle of Mist"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'The Bound Decayer'!");
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

    public class HestorRainsong : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BoundDecayerQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBowyer()); // Hestor as a bardic vendor
        }

        [Constructable]
        public HestorRainsong()
            : base("the Wandering Minstrel", "Hestor Rainsong")
        {
        }

        public HestorRainsong(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 35);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1002; // Pale, thoughtful hue
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Misty silver-blue
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1150, Name = "Mistweave Tunic" }); // Soft silver
            AddItem(new LongPants() { Hue = 1157, Name = "Balladeer's Breeches" }); // Midnight blue
            AddItem(new Cloak() { Hue = 1153, Name = "Shroud of Echoes" }); // Faint sky-blue
            AddItem(new FeatheredHat() { Hue = 1175, Name = "Minstrel's Plume" }); // Deep violet feather
            AddItem(new Sandals() { Hue = 1109, Name = "Wanderer’s Soles" }); // Weathered gray
            AddItem(new Lute() { Name = "Hestor’s Lament", Hue = 1147 }); // Pale wood tone, worn strings

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Minstrel's Satchel";
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
