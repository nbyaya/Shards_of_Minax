using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class CrimsonBeastsbaneQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Crimson Beastsbane"; } }

        public override object Description
        {
            get
            {
                return
                    "You find yourself before **Sylas Fangborne**, Yew’s seasoned Beastmaster. His eyes, fierce and wild, hold sorrow beneath their amber hue.\n\n" +
                    "“The woods ain't what they used to be,” he mutters, adjusting the crimson sash draped across his shoulder. “The balance is broken. This *PutridGorefiend*—a beast gone mad—tore through the glades, killed my best hound, and now it drinks deep from the blood of our wildlife.”\n\n" +
                    "“I’ve trained hunters all my life, but even the fiercest of ‘em won’t go near it. It’s not just a beast now—it’s *something worse*. And its bloodlust is callin’ other predators, stirrin’ things that should stay sleepin’.”\n\n" +
                    "“Bring me its head, and we might just save what’s left of these woods.”\n\n" +
                    "**Slay the PutridGorefiend** and restore balance to the wilds of Yew.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then we’ll all watch the wilds bleed. Just pray it don’t come knockin’ on your door next.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathin’? Then so’s that monster. And I swear, the trees whisper of its hunger.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It’s done? You did it, then… You’ve not just avenged my hound—you’ve given the wilds a chance to heal.\n\n" +
                       "Take this, *MantleOfBeastsWill*. It’s been worn by those who understand the wild heart of the forest. May it serve you as well as you’ve served us.";
            }
        }

        public CrimsonBeastsbaneQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PutridGorefiend), "PutridGorefiend", 1));
            AddReward(new BaseReward(typeof(MantleOfBeastsWill), 1, "MantleOfBeastsWill"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Crimson Beastsbane'!");
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

    public class SylasFangborne : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(CrimsonBeastsbaneQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBButcher());
        }
		public override bool UsesRandomisedStock => true;

        [Constructable]
        public SylasFangborne()
            : base("the Beastmaster", "Sylas Fangborne")
        {
        }

        public SylasFangborne(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 95, 70);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 0x203C; // Long hair
            HairHue = 1107; // Wildwood Brown
            FacialHairItemID = 0x204B; // Full beard
            FacialHairHue = 1107;
        }

        public override void InitOutfit()
        {
            AddItem(new BearMask() { Hue = 1821, Name = "Fangborne’s Hood" });
            AddItem(new LeatherDo() { Hue = 2001, Name = "Bloodfang Vest" });
            AddItem(new StuddedLegs() { Hue = 1811, Name = "Wildborn Greaves" });
            AddItem(new LeatherGloves() { Hue = 1801, Name = "Tracker’s Claws" });
            AddItem(new Cloak() { Hue = 1157, Name = "Crimson Beastcloak" });
            AddItem(new Boots() { Hue = 1109, Name = "Hunter’s Tread" });

            AddItem(new ShepherdsCrook() { Hue = 1154, Name = "Fangmaster’s Crook" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Beastmaster’s Pack";
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
