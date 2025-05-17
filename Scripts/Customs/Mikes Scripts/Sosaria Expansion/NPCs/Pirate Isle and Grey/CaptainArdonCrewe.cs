using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RevenantsReturnQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Revenant's Return"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Captain Ardon Crewe*, Watch Commander of Grey.\n\n" +
                    "His sea-worn coat flutters as he sharpens a weathered cutlass, eyes like cold steel scanning the harbor's edge.\n\n" +
                    "“The **DireRevenant** walks again. Took one of mine—a good lad. Left him twisted, halberd through bone and soul.”\n\n" +
                    "“It comes from the old castle ruins—Exodus. Slips through the mist, strikes at night patrols. Gone before we draw breath.”\n\n" +
                    "“I’m no stranger to death, but this... this is something else. My men are losing faith.”\n\n" +
                    "“You’re no greenhorn. If you’ve the spine, **track the Revenant**, end its haunt, and bring peace to the watch.”\n\n" +
                    "**Slay the DireRevenant** that haunts our nights.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Aye, I won’t force your hand. But know this—if the Revenant lives, more good men will die.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it walks? Then the night grows darker, and my blade grows restless.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It's done? The Revenant’s no more?...\n\n" +
                       "You’ve done the work of ten men tonight. The watch owes you, and so do I.\n\n" +
                       "Take this: *AstartesShoulderGuard*. It’s saved many shoulders from many blades—may it serve you well.";
            }
        }

        public RevenantsReturnQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DireRevenant), "DireRevenant", 1));
            AddReward(new BaseReward(typeof(AstartesShoulderGuard), 1, "AstartesShoulderGuard"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Revenant's Return'!");
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

    public class CaptainArdonCrewe : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RevenantsReturnQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBRangedWeapon());
        }

        [Constructable]
        public CaptainArdonCrewe()
            : base("the Watch Commander", "Captain Ardon Crewe")
        {
        }

        public CaptainArdonCrewe(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 100, 100);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1325; // Weathered sailor tone
            HairItemID = 0x2048; // Long hair
            HairHue = 1109; // Salt-and-pepper grey
            FacialHairItemID = 0x203F; // Full beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new TricorneHat() { Hue = 1157, Name = "Commander’s Crest" });
            AddItem(new FancyShirt() { Hue = 1102, Name = "Sea-Worn Tunic" });
            AddItem(new LeatherDo() { Hue = 1107, Name = "Grey Watch Jerkin" });
            AddItem(new StuddedLegs() { Hue = 1109, Name = "Salt-Stained Trousers" });
            AddItem(new LeatherGloves() { Hue = 1108, Name = "Grasp of Grey" });
            AddItem(new ThighBoots() { Hue = 1105, Name = "Harborwalker’s Boots" });

            AddItem(new Cutlass() { Hue = 1109, Name = "Watcher’s Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Watch Commander’s Pack";
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
