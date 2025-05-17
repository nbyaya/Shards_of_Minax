using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShadowsAtDuskQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shadows at Dusk"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Torbin Oakhand*, a seasoned lumberjack of Fawn, standing by his stump-strewn yard. His axe is embedded deep into a split log, hands trembling slightly as twilight approaches.\n\n" +
                    "\"They're gone... my crew. Good men. They vanished at dusk, near the outer woods. One by one. I marked every camp they left... and still, no sign.\"\n\n" +
                    "\"Folk whisper about the *Duskwyr*—a beast that drags the lost into shadow. I never gave it thought until I heard the screams. Until I saw the clawed prints in the dirt, leading into nothing.\"\n\n" +
                    "\"I can't fight it, but you... you look like you can. End it before more vanish with the light. Follow my map, find where it hunts, and kill the **Duskwyr**.\"\n\n" +
                    "\"Do it, and these will be yours: *Stormforged Gauntlets*. My brother forged them to tame storms, but they’ll serve better in your hands now.\"";
            }
        }

        public override object Refuse
        {
            get
            {
                return "\"I’ll hold out hope a while longer. But when the dusk comes… it won’t be me out there. It'll be someone else, screaming.\"";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "\"Still out there? Each night it hunts. And each night I fear more will vanish. I marked the camps… follow them, and end this.\"";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s over, then. The woods feel lighter already.\n\n" +
                       "You’ve not only avenged my crew—you’ve saved others from vanishing into that cursed dusk.\n\n" +
                       "Here—take the *Stormforged Gauntlets*. May they strike as true for you as you did for them.";
            }
        }

        public ShadowsAtDuskQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Duskwyr), "the Duskwyr", 1));
            AddReward(new BaseReward(typeof(StormforgedGauntlets), 1, "Stormforged Gauntlets"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shadows at Dusk'!");
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

    public class TorbinOakhand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShadowsAtDuskQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter());
        }

        [Constructable]
        public TorbinOakhand()
            : base("the Lumberjack", "Torbin Oakhand")
        {
        }

        public TorbinOakhand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(95, 100, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1052; // Weathered, tan
            HairItemID = 0x203B; // Short hair
            HairHue = 1154; // Ash-brown
            FacialHairItemID = 0x2041; // Full beard
            FacialHairHue = 1154;
        }

        public override void InitOutfit()
        {
            AddItem(new FurSarong() { Hue = 2207, Name = "Oak-Bound Wrap" }); // Deep forest green
            AddItem(new Shirt() { Hue = 2124, Name = "Windswept Flannel" }); // Weathered red
            AddItem(new LeatherGloves() { Hue = 2305, Name = "Grain-Worn Grips" });
            AddItem(new Boots() { Hue = 2406, Name = "Rootstrider Boots" }); // Bark brown
            AddItem(new WideBrimHat() { Hue = 2419, Name = "Sunshade Hat" }); // Dusk-grey
            AddItem(new HalfApron() { Hue = 2117, Name = "Oakhand's Apron" }); // Deep green with a branded oak emblem
            AddItem(new DoubleAxe() { Hue = 1820, Name = "Duskcleaver" }); // Well-used axe, slightly rusted but deadly sharp

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Earthy tone
            backpack.Name = "Torbin's Supply Pack";
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
