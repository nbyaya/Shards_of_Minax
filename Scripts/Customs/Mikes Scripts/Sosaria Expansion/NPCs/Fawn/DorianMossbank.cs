using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class PurgeThePestilenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Purge the Pestilence"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Dorian Mossbank*, Fawn’s reclusive apothecary. His shop is cluttered with herbs and vials, the air thick with the scent of alchemical mixtures.\n\n" +
                    "He looks up, weary and tense.\n\n" +
                    "“You smell that? The marsh air... it's wrong. My brews, they curdle too soon. My mentor, he warned me—*blight always finds a way back*.”\n\n" +
                    "“There’s something out there, something *sick*—the **Blightrun**. It spreads rot like breath, poisons the marsh and taints the ground.”\n\n" +
                    "“I've tried everything—wards, brews, flames. But this... this is beyond me. If I can't cleanse it, I can't heal anyone. And I won’t lose another mentor’s legacy to decay.”\n\n" +
                    "**Slay the Blightrun** and let the marsh breathe clean again.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then stay clear of my shop. I won't have your pity or your plague.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You haven't faced it yet? The rot thickens. I can't hold out much longer.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The marsh... it smells *clean*. You’ve done it.\n\n" +
                       "This brew, I made it from the first herbs to grow after your deed. Take it. *BeastmastersTanic*—may it fortify you as you’ve fortified our home.";
            }
        }

        public PurgeThePestilenceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Blightrun), "the Blightrun", 1));
            AddReward(new BaseReward(typeof(BeastmastersTanic), 1, "BeastmastersTanic"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Purge the Pestilence'!");
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

    public class DorianMossbank : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(PurgeThePestilenceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public DorianMossbank()
            : base("the Reclusive Apothecary", "Dorian Mossbank")
        {
        }

        public DorianMossbank(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2047; // Long Hair
            HairHue = 1108; // Moss green
            FacialHairItemID = 0x203F; // Short Beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1425, Name = "Sporespun Shirt" }); // Sickly green
            AddItem(new ElvenPants() { Hue = 1420, Name = "Blightwoven Pants" }); // Dark moss
            AddItem(new Sandals() { Hue = 1109, Name = "Marshwalker's Sandals" }); // Dull grey
            AddItem(new HoodedShroudOfShadows() { Hue = 1424, Name = "Fume Hood" }); // Pale green shroud
            AddItem(new HalfApron() { Hue = 1108, Name = "Alchemist's Wrap" }); // Darker green apron


            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Herbalist’s Pack";
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
