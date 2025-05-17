using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class EndlessEchoQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Endless Echo"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Morwen Everstream*, a meticulous archivist whose midnight-blue robes shimmer faintly under candlelight.\n\n" +
                    "Her voice is calm, yet there’s an edge of urgency behind each word:\n\n" +
                    "“The **Preservation Vault 44** was designed to lock time in place—to preserve what was. But something has gone wrong in its library wing. **The Continuant**, a relic-bound entity, now loops endlessly, siphoning ambient magic to sustain its broken cycle.”\n\n" +
                    "“I’ve traced missing journal pages, warped sigils, and dimmed wards all back to that thing. If it is not destroyed, the Vault’s delicate wards will collapse, and worse—**its knowledge will be lost or corrupted forever**.”\n\n" +
                    "**Slay the Continuant**, end its loop, and restore balance to the Vault’s flow of time.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the echoes shall continue... and the Vault may be silenced forever.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The Continuant still endures? The wards flicker more violently now. We must act before it consumes all.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve silenced the loop? The Vault’s hum has softened...\n\n" +
                       "You’ve preserved more than knowledge today—you’ve **rescued time itself from unraveling**.\n\n" +
                       "Take this: *VeilOfTheDrownedVoice*. Woven from threads lost in the Vault’s cycles, it may protect you from whispers not meant for this world.";
            }
        }

        public EndlessEchoQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(Continuant), "Continuant", 1));
            AddReward(new BaseReward(typeof(VeilOfTheDrownedVoice), 1, "VeilOfTheDrownedVoice"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Endless Echo'!");
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

    public class MorwenEverstream : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(EndlessEchoQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this));
        }

        [Constructable]
        public MorwenEverstream()
            : base("the Archivist of Castle British", "Morwen Everstream")
        {
        }

        public MorwenEverstream(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1175, Name = "Twilight Archivist Robe" }); // Deep indigo
            AddItem(new BodySash() { Hue = 1150, Name = "Sash of Echoed Threads" }); // Silver-grey
            AddItem(new Boots() { Hue = 1109, Name = "Silent Steps" }); // Dust-grey
            AddItem(new WizardsHat() { Hue = 1175, Name = "Archivist’s Crest" }); // Matching the robe
            AddItem(new SpellWeaversWand() { Hue = 1260, Name = "Chronicle Wand" }); // Pale silver-blue

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Archive Satchel";
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
