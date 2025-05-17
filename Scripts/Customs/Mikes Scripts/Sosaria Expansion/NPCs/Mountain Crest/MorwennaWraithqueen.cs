using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class MatriarchsFallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Matriarch’s Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Morwenna Wraithqueen*, Occultist of Mountain Crest.\n\n" +
                    "Her fingers trace unseen glyphs in the air, eyes like frozen mirrors, her voice low as a whisper from beyond.\n\n" +
                    "“The Ice Wraith Matriarch stirs again. Her brood festers in the Cavern’s breath, and her phylactery still pulses in the southern crypt.”\n\n" +
                    "“I feel her song—cold and necrotic—spreading. If her line isn’t severed, death will claim these peaks.”\n\n" +
                    "**Slay the Ice Wraith Matriarch**, and destroy her phylactery before her dominion solidifies.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then her chill will deepen, and none of us shall dream warm again.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "She still sings, then? The ice grows crueler. I feel her claws beneath my skin.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The Matriarch falls, and her line is broken. Her song is stilled. You’ve done more than kill—you’ve **freed the frost from her curse**.\n\n" +
                       "Take this: *Cleric’s Sacred Sash*. Let it ward you, as you have warded us.";
            }
        }

        public MatriarchsFallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(IceWraithMatriarch), "Ice Wraith Matriarch", 1));
            AddReward(new BaseReward(typeof(ClericsSacredSash), 1, "Cleric’s Sacred Sash"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Matriarch’s Fall'!");
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

    public class MorwennaWraithqueen : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(MatriarchsFallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public MorwennaWraithqueen()
            : base("the Occultist", "Morwenna Wraithqueen")
        {
        }

        public MorwennaWraithqueen(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1150; // Pale skin
            HairItemID = 0x203C; // Long hair
            HairHue = 1153; // White hair
        }

        public override void InitOutfit()
        {
            AddItem(new DeathRobe() { Hue = 1154, Name = "Wraithbound Shroud" });
            AddItem(new HoodedShroudOfShadows() { Hue = 1154, Name = "Void-Hood of Frost" });
            AddItem(new Sandals() { Hue = 1109, Name = "Ice-Touched Sandals" });
            AddItem(new BodySash() { Hue = 1151, Name = "Sash of Severed Songs" });
            AddItem(new SpellWeaversWand() { Hue = 0, Name = "Phylactery Seeker" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1175; // Deep frost-blue
            backpack.Name = "Relic Satchel";
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
