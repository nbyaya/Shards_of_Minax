using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TurkeyTakedownQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Turkey Takedown"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before *Ivor Stonehelm*, Lead Miner of East Montor, his face lined with the dust of countless tunnels.\n\n" +
                    "A battered helm rests on his head, his beard tangled with flecks of stone. His eyes burn with urgency, his voice rough like gravel.\n\n" +
                    "“We’ve got a right mess in the **Caves of Drakkon**, and I don’t mean your usual rocks and rubble.”\n\n" +
                    "“Something foul—some twisted **DraconicTurkey**—has been tearing through our mining supports. Gobbles echo through the stone like curses, and if we don’t act, we’re all due for a cave-in.”\n\n" +
                    "“I’m no slayer, and my men won’t face the beast. But you… maybe you’ve the guts to bring it down before the tunnels bury us all.”\n\n" +
                    "**Kill the DraconicTurkey** before the mines collapse. Do that, and I’ll see to it you’re rewarded like a true cartographer of chaos.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may the stones hold for another day… but I fear they won’t. That creature’s still gobbling, still clawing. We won’t last long.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathing, is it? The supports are cracking louder than ever. Every step feels like the last.";
            }
        }

        public override object Complete
        {
            get
            {
                return "So, the beast’s been stuffed? Ha! You’ve done a miner’s work today, friend.\n\n" +
                       "The men will sleep easier tonight. Take this, the **CartographersHat**—not just a symbol, but a mark of someone who maps the world, and makes it safer.";
            }
        }

        public TurkeyTakedownQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DraconicTurkey), "DraconicTurkey", 1));
            AddReward(new BaseReward(typeof(CartographersHat), 1, "CartographersHat"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Turkey Takedown'!");
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

    public class IvorStonehelm : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TurkeyTakedownQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public IvorStonehelm()
            : base("the Lead Miner", "Ivor Stonehelm")
        {
        }

        public IvorStonehelm(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1102; // Soot-black
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1102;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2419, Name = "Stonehelm's Chestplate" }); // Ironstone-gray
            AddItem(new StuddedLegs() { Hue = 1824, Name = "Crag-Bound Leggings" });
            AddItem(new StuddedGloves() { Hue = 1812, Name = "Dust-Hardened Gloves" });
            AddItem(new OrcHelm() { Hue = 2301, Name = "Stonehelm's Visor" });
            AddItem(new HalfApron() { Hue = 1835, Name = "Miner’s Apron of the Depths" });
            AddItem(new Boots() { Hue = 1109, Name = "Rockwalker Boots" });

            AddItem(new Pickaxe() { Hue = 2413, Name = "Echo-Pick" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Ore-Laden Pack";
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
