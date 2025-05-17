using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TrollfallQuestB : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Trollfall"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Brom “Oakfist” Hendry*, the Stonewright of Grey Isle.\n\n" +
                    "Broad-shouldered, hands like carved granite, his eyes hold the fury of a man wronged.\n\n" +
                    "“You’ve heard the tales, haven’t you? Of caravans lost, routes smashed to rubble?”\n\n" +
                    "“It’s no rumor. The DecaybruteTroll’s real. Filthy beast ambushed us by the mountain pass, diseased club swinging like thunder. Smashed my transport—*my life’s work*. My apprentice, he barely escaped… and I’ve still got the scar to prove that night happened.”\n\n" +
                    "**I want that troll dead.** The routes can’t stay closed, and I won’t sleep till it’s over. Slay the beast, and bring me its cursed heart.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Cowardice feeds the beast. Mark my words, every lost caravan’s blood will be on your hands.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathing? Then the troll’s still out there. And I’ll not rest until you bring me proof it’s dead.";
            }
        }

        public override object Complete
        {
            get
            {
                return "**You’ve done it? The troll’s heart?** Gods above, I’ll carve this into stone myself. You’ve not just avenged my work—you’ve saved Grey Isle’s lifelines.\n\n" +
                       "Take this, forged from the wreck of my last cart: *TempestHammer*. Let it strike like the storm that should’ve ended that beast sooner.";
            }
        }

        public TrollfallQuestB() : base()
        {
            AddObjective(new SlayObjective(typeof(DecaybruteTroll), "DecaybruteTroll", 1));
            AddReward(new BaseReward(typeof(TempestHammer), 1, "TempestHammer"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Trollfall'!");
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

    public class BromOakfistHendry : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TrollfallQuestB) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBStoneCrafter()); // Stonewright profession
        }

        [Constructable]
        public BromOakfistHendry()
            : base("the Stonewright", "Brom “Oakfist” Hendry")
        {
        }

        public BromOakfistHendry(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 75, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2048; // Short hair
            HairHue = 1102; // Granite-grey
            FacialHairItemID = 0x203E; // Short beard
            FacialHairHue = 1102;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2406, Name = "Oakforged Plate" }); // Earthy grey-brown
            AddItem(new StuddedLegs() { Hue = 2419, Name = "Stonewright’s Greaves" });
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Chisel-Marked Gloves" });
            AddItem(new HornedTribalMask() { Hue = 1154, Name = "Greyhorn Mask" });
            AddItem(new HalfApron() { Hue = 1837, Name = "Mason's Apron" });
            AddItem(new Boots() { Hue = 1811, Name = "Rockstep Boots" });

            AddItem(new WarHammer() { Hue = 2425, Name = "Oakfist's Mallet" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Stonewright's Pack";
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
