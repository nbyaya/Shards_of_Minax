using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CoolTheLivingLavaQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Cool the LivingLava"; } }

        public override object Description
        {
            get
            {
                return
                    "Perrin Bellows, a brawny figure with soot-streaked arms, is hammering furiously at a warped metal pipe. His workshop crackles with heat, and sweat beads on his brow despite the cool mountain air.\n\n" +
                    "“It’s the damn **LivingLava**!” he growls. “Slithered right from the Gate of Hell itself and into my forge. The bellows clog, the fires rage too hot, and I can’t temper a blade without it melting down.”\n\n" +
                    "“I apprenticed in Devil Guard—you think I’d fear heat? But this... this isn’t heat. It’s alive. It mocks me, dances through the vents, ruins every ingot. I need it **dead**.”\n\n" +
                    "“Get down there, into the Gate of Hell, and **destroy the LivingLava**. Let me work in peace again, before the forge takes me with it.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then I'll try dousing the forge with spring water, but if I vanish in smoke, you’ll know why.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still no luck? It grows stronger each day... the forge’s breath is like a dragon’s now.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You did it? The fires have calmed... I can breathe again.\n\n" +
                       "You’ve done me a great service. Here, take these: **Gloves of Stonemasonry**. Made with care in Devil Guard, they’ll let your hands shape stone as mine shape steel.\n\n" +
                       "May your forge burn steady, and never again face a foe like that.";
            }
        }

        public CoolTheLivingLavaQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(LivingLava), "LivingLava", 1));
            AddReward(new BaseReward(typeof(GlovesOfStonemasonry), 1, "Gloves of Stonemasonry"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Cool the LivingLava'!");
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

    public class PerrinBellows : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CoolTheLivingLavaQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBSmithTools());
        }

        [Constructable]
        public PerrinBellows()
            : base("the Bellows Maker", "Perrin Bellows")
        {
        }

        public PerrinBellows(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 100, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2411; // Tanned, soot-darkened skin
            HairItemID = 0x203C; // Short hair
            HairHue = 1109; // Ash-grey
            FacialHairItemID = 0x204B; // Thick beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2101, Name = "Forge-Hardened Tunic" }); // Ember-red
            AddItem(new StuddedLegs() { Hue = 2407, Name = "Heat-Resistant Trousers" }); // Charcoal
            AddItem(new LeatherGloves() { Hue = 2105, Name = "Cinder-Kissed Gloves" }); // Dark burnt hue
            AddItem(new HalfApron() { Hue = 2309, Name = "Bellowsmaker’s Apron" }); // Soot-black
            AddItem(new Boots() { Hue = 2413, Name = "Fire-Tread Boots" }); // Deep earth-tone
            AddItem(new SmithSmasher() { Hue = 2117, Name = "Anvil-Breaker" }); // Glowing faintly with heat

            Backpack backpack = new Backpack();
            backpack.Hue = 1154;
            backpack.Name = "Tool Satchel";
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
