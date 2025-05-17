using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RoninsLastStandQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ronin’s Last Stand"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Merrin Tidefarer*, the master boatwright of Renika, her arms weathered by sea and forge alike.\n\n" +
                    "Clad in a sea-blue vest and iron-plated boots, her eyes glint with the steel of someone who’s both built and defended fleets.\n\n" +
                    "“I crafted Renika’s first ironclad longboat, braving the storm to tame the sea. Now, a man who once swore fealty to the Guild turns pirate and saboteur.”\n\n" +
                    "**The MountainRonin**, he calls himself—harassing our sea caravans from his stronghold in the peaks, driving up costs, and spilling blood.”\n\n" +
                    "“I don’t trust politics to end this. What I need is a sword, not words.”\n\n" +
                    "**Slay the MountainRonin** before Renika's trade fleet is broken by his vendetta.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If you won’t face him, then I’ll find someone who will. But don’t ask me to build ships for a future this Ronin will burn.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still he stalks the peaks? Every day he breathes is a hammer blow against Renika’s sails.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The sea sighs in relief, and so do I.\n\n" +
                       "The Ronin’s shadow no longer darkens our decks. Take this: *CharlemagnesWarAxe*—a weapon once wielded by our first naval guard.\n\n" +
                       "Let it remind you of the strength it takes to guard not just lives, but the livelihoods of a people bound to the waves.";
            }
        }

        public RoninsLastStandQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MountainRonin), "MountainRonin", 1));
            AddReward(new BaseReward(typeof(CharlemagnesWarAxe), 1, "CharlemagnesWarAxe"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ronin’s Last Stand'!");
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

    public class MerrinTidefarer : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RoninsLastStandQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBShipwright(this));
        }

        [Constructable]
        public MerrinTidefarer()
            : base("the Master Boatwright", "Merrin Tidefarer")
        {
        }

        public MerrinTidefarer(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Sun-kissed skin tone
            HairItemID = 8252; // Long, tied-back style
            HairHue = 1153; // Deep sea blue
        }

        public override void InitOutfit()
        {
            AddItem(new Tunic() { Hue = 1175, Name = "Wavecrest Tunic" }); // Deep blue
            AddItem(new LeatherLegs() { Hue = 1815, Name = "Stormbound Trousers" }); // Storm-gray
            AddItem(new LeatherGloves() { Hue = 1821, Name = "Caulked Deckhands" }); // Dark teak brown
            AddItem(new BodySash() { Hue = 1170, Name = "Guildmaster’s Sash" }); // Navy blue
            AddItem(new ThighBoots() { Hue = 2101, Name = "Ironclad Walkers" }); // Iron gray
            AddItem(new TricorneHat() { Hue = 1817, Name = "Tidefarer's Tricorne" }); // Weathered black

            AddItem(new Cutlass() { Hue = 2407, Name = "Keelbreaker Blade" }); // Steel-blue tint

            Backpack backpack = new Backpack();
            backpack.Hue = 1154; // Sea-green
            backpack.Name = "Shipwright's Satchel";
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
