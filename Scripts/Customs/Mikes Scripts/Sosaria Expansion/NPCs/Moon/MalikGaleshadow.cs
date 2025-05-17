using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilentStormQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silent Storm"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Malik Galeshadow*, known among the scholars of Moon as a Windcaller.\n\n" +
                    "“Do you feel it? The currents twist unnaturally. The *Mystic Effervescence* rises, charged with storm and malice.\n\n" +
                    "I cannot call the winds, not while it sings in the air, drowning my incantations.\n\n" +
                    "End this tempest-borne being, and return stillness to Moon.”\n\n" +
                    "**Destroy the Mystic Effervescence** disrupting the skies above Moon.";
            }
        }

        public override object Refuse { get { return "Then I must brace myself... the winds grow louder, more restless."; } }

        public override object Uncomplete { get { return "The storm has not broken. You must find the Effervescence and silence its fury."; } }

        public override object Complete { get { return "The winds flow freely again... I can hear them whisper truths once more. Take this, woven from the breath of calmed storms."; } }

        public SilentStormQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(MysticEffervescence), "MysticEffervescence", 1));

            AddReward(new BaseReward(typeof(SilentwaveWrap), 1, "SilentwaveWrap"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silent Storm'!");
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

    public class MalikGaleshadow : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilentStormQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCobbler()); 
        }

        [Constructable]
        public MalikGaleshadow()
            : base("the Windcaller", "Malik Galeshadow")
        {
        }

        public MalikGaleshadow(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 70, 40);

            Female = false;
            Body = 0x190; // Male Body
            Race = Race.Human;

            Hue = Race.RandomSkinHue(); // Hair and beard
            HairItemID = 0x204C; // Long hair
            HairHue = 1150; // Pale silver-blue
            FacialHairItemID = 0x203E; // Short beard
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            // Unique Outfit reflecting wind magic & desert mysticism
            AddItem(new Robe() { Hue = 1151, Name = "Tempest Robe" }); // Light azure, almost translucent
            AddItem(new FeatheredHat() { Hue = 2418, Name = "Zephyr Crest" }); // Soft storm grey
            AddItem(new Cloak() { Hue = 1153, Name = "Whispercloak" }); // Deep sky blue with shimmer
            AddItem(new Sandals() { Hue = 2403, Name = "Duststep Sandals" }); // Sandy hue
            AddItem(new GnarledStaff() { Hue = 2407, Name = "Windbinder's Crook" }); // Pale wood, with gust motifs
            Backpack backpack = new Backpack();
            backpack.Hue = 44;
            backpack.Name = "Skywoven Pack";
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
