using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TrickstersEndQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Trickster’s End"; } }

        public override object Description
        {
            get
            {
                return
                    "*Fen Blackfeather*, Rookery Keeper of Pirate Isle, eyes you from beneath a wide-brimmed feathered hat. His cloak shimmers faintly, like oil on water, and a spectral rook perches on his shoulder.\n\n" +
                    "“Ye smell of courage... or madness. Good, we need both.”\n\n" +
                    "“The **Phantom Trickster** has haunted me birds. Twisted their minds, lured 'em into chasms with false skies and sweet lies. Where he passes, their feathers turn ghost-white. *Ghost-rooks*, I call 'em now.”\n\n" +
                    "“This foul specter nests in **Exodus Dungeon**, where time's broke and sorrow lives.”\n\n" +
                    "“Do what I can't. **Slay the Trickster.** Free me birds, and I'll see ye wear nature's bounty like a second skin.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Aye, not all are ready to face illusions made real. But know this—each rook lost to him dims the sky a little more.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still the Trickster dances? Me birds cry in ghostly tongues... and now, they start followin’ strangers, not just me.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So... the Trickster's song is ended. And me rooks fly free again.\n\n" +
                       "Ye've not just slain a beast, friend. Ye've cleared the skies and given back their will.\n\n" +
                       "Take this, *Bracelet of Nature’s Bounty*. May the winds favor ye, and the rooks watch over ye.";
            }
        }

        public TrickstersEndQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PhantomTrickster), "Phantom Trickster", 1));
            AddReward(new BaseReward(typeof(BraceletOfNaturesBounty), 1, "Bracelet of Nature’s Bounty"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Trickster’s End'!");
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

    public class FenBlackfeather : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TrickstersEndQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer()); // Keeps and trains birds.
        }

        [Constructable]
        public FenBlackfeather()
            : base("the Rookery Keeper", "Fen Blackfeather")
        {
        }

        public FenBlackfeather(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 60, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1317; // Pale, windswept look.
            HairItemID = 0x2049; // Long Hair
            HairHue = 1108; // Crow black
            FacialHairItemID = 0x203B; // Full beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new Cloak() { Hue = 1109, Name = "Feathered Shroud" }); // Dark raven feather hues
            AddItem(new LeatherCap() { Hue = 1108, Name = "Rookmaster’s Hat" }); // Styled like a bird’s crest
            AddItem(new FancyShirt() { Hue = 1150, Name = "Sky-Worn Shirt" }); // Faint sky-blue
            AddItem(new Kilt() { Hue = 2101, Name = "Stormbound Kilt" }); // Dark storm-grey
            AddItem(new Sandals() { Hue = 1175, Name = "Silent Step Sandals" }); // Misty white

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Rook Keeper’s Satchel";
            AddItem(backpack);

            Item ravenTotem = new GnarledStaff() { Hue = 1109, Name = "Totem of the Rookery" };
            AddItem(ravenTotem);
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
