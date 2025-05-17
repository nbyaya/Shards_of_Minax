using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SteelAgainstDecayQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Steel Against Decay"; } }

        public override object Description
        {
            get
            {
                return
                    "You meet *Elinora Gleamstone*, Yew’s renowned jeweler, in her lantern-lit workshop tucked between ancient trees.\n\n" +
                    "She polishes a dark gem, her fingers nimble, her eyes sharp and knowing.\n\n" +
                    "“You’ve heard the tales, haven’t you? Of the *CorrodedArmour*, deep within Catastrophe’s fungal abyss. I’ve seen relics of its plating—twisted iron, alive with decay.”\n\n" +
                    "“I believe... no, I *know* that alloy holds secrets. My mentor once taught me the art of refining metals from cathedral armor—what if I could cleanse this corruption, forge beauty from rot?”\n\n" +
                    "**Slay the CorrodedArmour**, and bring me its plating. Let me turn ruin into radiance.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Decay wins, then? I understand... but I shall still dream of steel reborn from ruin.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The CorrodedArmour yet stalks? Every moment it breathes, that metal slips deeper into decay.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Ah, yes... *this* is it. The metal, though tainted, still sings beneath the rust. I can feel the potential.\n\n" +
                       "Here—take this, the **MirageblessedCollar**. Crafted with what little I’ve purified, it’s a token of transformation. You’ve helped me reclaim something forgotten.";
            }
        }

        public SteelAgainstDecayQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(CorrodedArmour), "CorrodedArmour", 1));
            AddReward(new BaseReward(typeof(MirageblessedCollar), 1, "MirageblessedCollar"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Steel Against Decay'!");
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

    public class ElinoraGleamstone : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SteelAgainstDecayQuest) }; } }

        [Constructable]
        public ElinoraGleamstone()
            : base("the Artisan Jeweler", "Elinora Gleamstone")
        {
        }

        public ElinoraGleamstone(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 85, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Silvery-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FormalShirt() { Hue = 1164, Name = "Starweave Tunic" }); // Subtle shimmer
            AddItem(new FancyKilt() { Hue = 1157, Name = "Gleamstone Wrap" }); // Deep emerald
            AddItem(new Sandals() { Hue = 2309, Name = "Silversmith's Soles" });
            AddItem(new FlowerGarland() { Hue = 1161, Name = "Twilight Bloom Circlet" });
            AddItem(new Cloak() { Hue = 1175, Name = "Veil of Reflection" }); // Deep iridescent cloak

            AddItem(new ScribeSword() { Hue = 2500, Name = "Etcher’s Blade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Jeweler's Satchel";
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
