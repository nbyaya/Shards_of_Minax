using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class AntlersOfAfflictionQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Antlers of Affliction"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Frynn Meadowlark*, Yew’s dedicated Beastkeeper, standing at the edge of her enchanted grove.\n\n" +
                    "Her hands are stained with herbal tinctures, eyes wide with worry as she cradles a young, feverish calf.\n\n" +
                    "“Something’s wrong… terribly wrong. My herd—they’re changing. It started with this one, and now the forest itself seems uneasy.”\n\n" +
                    "“A foul hind has emerged from Catastrophe… its breath carries plague, its antlers drip with rot. I need those antlers. Only by studying their poison can I hope to craft an antidote.”\n\n" +
                    "“Will you help me? **Slay the PestilentHind**, bring me its virulent antlers, and perhaps we can save more than just my herd.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "I understand... but beware the woods. If the PestilentHind’s sickness spreads, Yew may not remain untouched.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You haven’t yet faced it? I feel its presence growing stronger. My calf worsens with each sunset.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The antlers… they reek of decay, but they hold answers. Thank you, truly.\n\n" +
                       "**With these, I may yet save my herd—and Yew itself.**\n\n" +
                       "Here, take these: *RootsingerLeggings*. They were passed to me by the forest’s guardians. May they shield you as you have shielded us.";
            }
        }

        public AntlersOfAfflictionQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PestilentHind), "Pestilent Hind", 1));
            AddReward(new BaseReward(typeof(RootsingerLeggings), 1, "RootsingerLeggings"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Antlers of Affliction'!");
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

    public class FrynnMeadowlark : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(AntlersOfAfflictionQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public FrynnMeadowlark()
            : base("the Beastkeeper", "Frynn Meadowlark")
        {
        }

        public FrynnMeadowlark(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 60);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x2049; // Long Hair
            HairHue = 1147; // Deep green
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2125, Name = "Meadowwoven Robe" }); // Forest-green
            AddItem(new LeatherGorget() { Hue = 2130, Name = "Thornbark Collar" });
            AddItem(new FurBoots() { Hue = 2413, Name = "Beastkeeper's Treads" });
            AddItem(new BearMask() { Hue = 2129, Name = "Watcher’s Mask" });

            AddItem(new ShepherdsCrook() { Hue = 2511, Name = "Grove’s Call" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Beastkeeper's Satchel";
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
