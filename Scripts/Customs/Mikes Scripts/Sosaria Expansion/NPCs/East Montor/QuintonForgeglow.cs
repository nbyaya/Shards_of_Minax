using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DevourersDoomQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Devourer's Doom"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Quinton Forgeglow*, the renowned but clearly frazzled alchemist of East Montor.\n\n" +
                    "His laboratory robes are stained, his spectacles cracked, yet his eyes burn with desperate focus.\n\n" +
                    "“You’ve come at the worst—or perhaps the best—time! My reagents, stolen! Swallowed by some monstrous fiend lurking in the **Caves of Drakkon**.\n\n" +
                    "“This beast... they call it the **DrakonicDevourer**. A glutton for the arcane, its breath wilts herbs, its blood corrodes stone, and its hunger knows no end. The survivors who escaped its lair speak of **acid dripping from its jaws**, burning through anything foolish enough to stand close.”\n\n" +
                    "“I can’t finish my work—can’t save the town from the malaise creeping in—until that thing is dead. Will you slay it for me?”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we are all undone. My work, the town’s future—swallowed by that beast. Let us hope someone else can stand where I cannot.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still alive? Then so is the beast. I hear its hunger in my sleep, the gnashing, the dripping... bring me its silence!";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? Truly? The DrakonicDevourer lies dead?\n\n" +
                       "Bless you! With my reagents safe, I can resume my work—perhaps even craft something to fend off such horrors.\n\n" +
                       "Here, take this: **CrabBushel**. Odd, yes, but it’s part of something greater—just like you are now.";
            }
        }

        public DevourersDoomQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakonicDevourer), "DrakonicDevourer", 1));
            AddReward(new BaseReward(typeof(CrabBushel), 1, "CrabBushel"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Devourer's Doom'!");
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

    public class QuintonForgeglow : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DevourersDoomQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
        }

        [Constructable]
        public QuintonForgeglow()
            : base("the Alchemist", "Quinton Forgeglow")
        {
        }

        public QuintonForgeglow(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 25);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Faded violet
            FacialHairItemID = Race.RandomFacialHair(this);
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1157, Name = "Ether-Stained Robe" }); // Deep indigo
            AddItem(new LeatherGloves() { Hue = 2125, Name = "Fume-Touched Gloves" });
            AddItem(new WizardsHat() { Hue = 1150, Name = "Focus Funnel Cap" });
            AddItem(new Sandals() { Hue = 2051, Name = "Spill-Worn Sandals" });
            AddItem(new HalfApron() { Hue = 2075, Name = "Reagent Belt" });

            AddItem(new ArtificerWand() { Hue = 2118, Name = "Arcane Measure" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1165;
            backpack.Name = "Alchemical Satchel";
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
