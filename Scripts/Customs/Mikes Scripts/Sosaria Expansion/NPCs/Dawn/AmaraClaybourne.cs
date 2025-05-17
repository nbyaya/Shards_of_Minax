using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class RageOfTheUrsineQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Rage of the Ursine"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Amara Claybourne*, a renowned artisan of Dawn. Her workshop smells of scorched clay and smoldering kiln smoke.\n\n" +
                    "Her hands are dusted with fine white powder, trembling slightly as she gestures to shattered sculptures at her feet.\n\n" +
                    "“I’ve spent years shaping this earth—giving it form, life, meaning. But now... now something *unholy* desecrates my work.”\n\n" +
                    "“It comes from the shadows of **Doom**—a beast unlike any I’ve known. A Demonic Grizzly. Its claws rend stone like parchment, its roars ignite the dust in my studio. Last night, I found more of my pieces crushed beneath its rage.”\n\n" +
                    "“I can no longer create while it still breathes. **Slay the Demonic Grizzly** that haunts me, and help restore peace to Dawn.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the clay will crumble, and my hands shall fall still. I only pray the beast does not set its sights beyond my workshop.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Its roars still echo. My kiln trembles, and the dust... the dust burns when I breathe.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It is done? Truly? Then the earth may rest, and so may I.\n\n" +
                       "Take this: *Driftcap of Renika*. A gift from the sea, to one who has quelled the fire. May it keep your thoughts clear, as you’ve brought clarity to mine.";
            }
        }

        public RageOfTheUrsineQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DemonicGrizzly), "Demonic Grizzly", 1));
            AddReward(new BaseReward(typeof(DriftcapOfRenika), 1, "Driftcap of Renika"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Rage of the Ursine'!");
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

    public class AmaraClaybourne : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(RageOfTheUrsineQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCarpenter()); // Closest to artisan/sculptor
        }

        [Constructable]
        public AmaraClaybourne()
            : base("the Artisan of Earth and Flame", "Amara Claybourne")
        {
        }

        public AmaraClaybourne(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long hair
            HairHue = 1153; // Ash-blonde
        }

        public override void InitOutfit()
        {
            AddItem(new FancyDress() { Hue = 2412, Name = "Kiln-Forged Robes" }); // Earthy red-brown hue
            AddItem(new HalfApron() { Hue = 2415, Name = "Clay-Stained Apron" }); // Burnt clay color
            AddItem(new Sandals() { Hue = 2305, Name = "Ashen Footwraps" }); // Dusty gray

            AddItem(new WizardsHat() { Hue = 2425, Name = "Crown of the Shaper" }); // Glazed earthen hue
            AddItem(new Scepter() { Hue = 2503, Name = "Rod of Molding Flames" }); // Decorative artisan tool, magical touch

            Backpack backpack = new Backpack();
            backpack.Hue = 1132;
            backpack.Name = "Artisan’s Satchel";
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
