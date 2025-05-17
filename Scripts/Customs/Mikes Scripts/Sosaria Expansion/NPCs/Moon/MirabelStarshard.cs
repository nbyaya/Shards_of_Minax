using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EchoesOfTheAncientsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Echoes of the Ancients"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Mirabel Starshard*, Moon's renowned Crystal Merchant.\n\n" +
                    "Her eyes shimmer like starlight as she speaks:\n\n" +
                    "“There’s a disturbance... whispers from the catacombs beneath Moon. A *Mummified Yomotsu*, long thought sealed, now prowls our sacred vaults.\n\n" +
                    "Its presence halts time, freezing all who draw near. This relic of the ancients must be laid to rest—before the catacombs become a tomb for us all.”\n\n" +
                    "**Slay the Mummified Yomotsu** lurking in the depths, and restore the flow of time.";
            }
        }

        public override object Refuse { get { return "Then time remains twisted... May the stars guide another to act before it is too late."; } }

        public override object Uncomplete { get { return "The Yomotsu still walks? The crystals flicker—I beg you, finish this."; } }

        public override object Complete { get { return "The echoes have ceased... and time flows once more. You have my eternal gratitude. Take this—the *Oathcarver of the Silent Guard*. May it serve you well."; } }

        public EchoesOfTheAncientsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MummifiedYomotsu), "MummifiedYomotsu", 1));

            AddReward(new BaseReward(typeof(OathcarverOfTheSilentGuard), 1, "OathcarverOfTheSilentGuard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Echoes of the Ancients'!");
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

    public class MirabelStarshard : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EchoesOfTheAncientsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpets()); 
        }

        [Constructable]
        public MirabelStarshard()
            : base("the Crystal Merchant", "Mirabel Starshard")
        {
        }

        public MirabelStarshard(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 90, 50);

            Female = true;
            Body = 0x191; // Female Body
            Race = Race.Human;

            Hue = 1002; // Pale lunar tone
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Deep moonlight blue
            FacialHairItemID = -1; // No facial hair
        }

        public override void InitOutfit()
        {
            // Unique Lunar-Crystal Inspired Outfit
            AddItem(new ElvenShirt() { Hue = 1154, Name = "Starwoven Tunic" }); // Soft silver-blue
            AddItem(new FancyKilt() { Hue = 1150, Name = "Moonshadow Wrap" }); // Pale silver
            AddItem(new Sandals() { Hue = 1153, Name = "Crystalstep Sandals" }); // Deep blue
            AddItem(new Cloak() { Hue = 1157, Name = "Veil of Starlight" }); // Midnight with shimmer
            AddItem(new FlowerGarland() { Hue = 1151, Name = "Circlet of Falling Stars" }); // Light blue circlet
            AddItem(new MagicWand() { Hue = 2406, Name = "Shardbinder Wand" }); // Luminous crystal wand
            Backpack backpack = new Backpack();
            backpack.Hue = 38; // Cool dark hue
            backpack.Name = "Crystal Satchel";
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
