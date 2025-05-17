using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class WebOfWoeQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Web of Woe"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Felwyn Bricklayer*, Master Architect of East Montor, standing beside a half-finished wall entangled in thick, silver-threaded webs.\n\n" +
                    "His eyes, bright with both brilliance and worry, glance frequently toward the mountains.\n\n" +
                    "“The quarry was to be our triumph—a bastion of stone, a defense against all storms. But now? Now it lies in ruinous silk.”\n\n" +
                    "“A spider, yes. But no ordinary thing. They call it the **Draconian Dreadspinner**, a monstrous weaver spawned from old drake cults. Its silk warps stone, bends beams, and ruins all I build.”\n\n" +
                    "“The silk... cursed, I tell you. Alive. My masons won't go near it. They hear voices when they sleep—*hissing*, like silk drawn taut.”\n\n" +
                    "**Destroy the Draconian Dreadspinner** that lurks in the Caves of Drakkon. Break the web’s curse and free the stone.”\n\n";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the walls shall crumble and the web will spread. I can only hope someone braver steps forward before my life's work is lost.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The web still clings to stone? I hear it growing stronger... The silk seeps into mortar, into dreams. Please, slay it before all is undone.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it! The web burns away and stone breathes once more.\n\n" +
                       "Take this—*Jester’s Mischievous Buckler*. Crafted long ago to ward off tricksters and fiends alike. It’s fitting, yes? A jest well-played against the spider’s curse.\n\n" +
                       "**May your hands build what none can break.**";
            }
        }

        public WebOfWoeQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DraconianDreadspinner), "Draconian Dreadspinner", 1));
            AddReward(new BaseReward(typeof(JestersMischievousBuckler), 1, "Jester's Mischievous Buckler"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Web of Woe'!");
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

    public class FelwynBricklayer : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(WebOfWoeQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBArchitect());
        }

        [Constructable]
        public FelwynBricklayer()
            : base("the Master Architect", "Felwyn Bricklayer")
        {
        }

        public FelwynBricklayer(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 85, 40);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 33770; // Earthy Stone hue
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Dusty brown
            FacialHairItemID = 0x2041; // Full beard
            FacialHairHue = 1153;
        }

        public override void InitOutfit()
        {
            AddItem(new StuddedDo() { Hue = 2401, Name = "Stonebound Vest" }); // Weathered Stone Grey
            AddItem(new StuddedLegs() { Hue = 1825, Name = "Mason’s Workpants" });
            AddItem(new LeatherGloves() { Hue = 1824, Name = "Grip of the Builder" });
            AddItem(new HalfApron() { Hue = 1175, Name = "Mortar-Stained Apron" });
            AddItem(new Boots() { Hue = 1811, Name = "Foundation Treaders" });
            AddItem(new Bandana() { Hue = 2213, Name = "Dustwrap of Focus" });

            AddItem(new HammerPick() { Hue = 2500, Name = "Felwyn’s Wall-Measure" });

            Backpack backpack = new Backpack();
            backpack.Hue = 2117;
            backpack.Name = "Architect's Satchel";
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
