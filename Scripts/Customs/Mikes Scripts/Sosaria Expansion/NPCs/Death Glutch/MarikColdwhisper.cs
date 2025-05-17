using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class SilentSecretsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Silent Secrets"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Marik Coldwhisper*, the solemn Archivist of Death Glutch.\n\n" +
                    "Clad in tattered robes of deep obsidian, with silver glyphs glowing faintly at the hems, he stands amidst crumbling scrolls and scorched parchment.\n\n" +
                    "His voice is but a whisper: “The records fade… not by time, but by force. Something *walks* within the old halls of Malidor. It feeds on **memory**, devouring our truths, twisting what once was.”\n\n" +
                    "“The beast is called an **Enchantment Slooth**. It has *erased* our original town charter, and now claws at the heart of our history.”\n\n" +
                    "He lifts a fragment of a burned scroll. “This is all that remains. *Kill the Slooth*, and let our past speak once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then may silence fall upon us all. Each hour that passes, more of us are forgotten… and soon, we may never have been.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still consumes our past? I can feel it, gnawing at names, at dates—at *me*. I… I forget what I was called before…";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve returned—and with it, a fragment of who we were.\n\n" +
                       "The Slooth is gone. But its hunger… I still hear echoes.\n\n" +
                       "**Take this:** *MysticBowOfLight*. May its clarity guard you, as you have guarded the truth.";
            }
        }

        public SilentSecretsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(EnchantmentSlooth), "Enchantment Slooth", 1));
            AddReward(new BaseReward(typeof(MysticBowOfLight), 1, "MysticBowOfLight"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Silent Secrets'!");
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

    public class MarikColdwhisper : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(SilentSecretsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe(this)); // Archivist and lorekeeper
        }

        [Constructable]
        public MarikColdwhisper()
            : base("the Archivist", "Marik Coldwhisper")
        {
        }

        public MarikColdwhisper(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(60, 50, 40);

            Female = false;
            Body = 0x190; // Male human
            Race = Race.Human;

            Hue = 33770; // Pale, almost ashen skin
            HairItemID = 0x2047; // Long, parted hair
            HairHue = 1108; // White-silver
            FacialHairItemID = 0x203E; // Long beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1150, Name = "Whispering Robes" }); // Midnight blue-black
            AddItem(new HoodedShroudOfShadows() { Hue = 1109, Name = "Archivist’s Hood" });
            AddItem(new Sandals() { Hue = 1175, Name = "Silent Treads" });
            AddItem(new BodySash() { Hue = 1152, Name = "Sash of Forgotten Truths" });

            AddItem(new ScribeSword() { Hue = 1150, Name = "Inkblade" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Scrollkeeper’s Satchel";
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
