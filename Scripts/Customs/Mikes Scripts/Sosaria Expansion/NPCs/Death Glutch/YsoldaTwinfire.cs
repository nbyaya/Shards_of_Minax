using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TwoFacedTerrorQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Two-Faced Terror"; } }

        public override object Description
        {
            get
            {
                return
                    "*Ysolda Twinfire*, the grizzled Beast Handler of Death Glutch, watches the horizon with a haunted gaze.\n\n" +
                    "Her leathers are singed and her arms bear fresh claw marks. She clutches a bundle of withered herbs and strange meat strips.\n\n" +
                    "“You seen a two-headed beast before? Didn’t think so. I raised **TwinHead** from a hatchling—thought I could tame it, thought wrong. It’s smarter than most, crueler too. It’s gone back to the *Malidor Witches Academy*, where it was born from the spells that broke that place.”\n\n" +
                    "“I can’t do it. I still bring it treats, hoping... but it’s beyond me now. It *needs* to be put down. Take these—**smell like home to it. Lure it out, end it quick.** Before it remembers who taught it to hunt.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "If you won’t do it, I’ll keep trying to feed it... till it feeds on me.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathing? Then TwinHead still lives. Don’t let it grow strong in that cursed place—it’ll find a way back.";
            }
        }

        public override object Complete
        {
            get
            {
                return "It's done, then? I... thank you. I raised it wrong. Too much love for a beast born of spells and hunger.\n\n" +
                       "Take this **Lamppost**—it’s old, sure, but it kept light on my doorstep every night I waited for TwinHead to come home.\n\n" +
                       "**Let it keep you safe now.**";
            }
        }

        public TwoFacedTerrorQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(TwinHead), "TwinHead", 1));
            AddReward(new BaseReward(typeof(LampPostA), 1, "Lamppost"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Two-Faced Terror'!");
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

    public class YsoldaTwinfire : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(TwoFacedTerrorQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public YsoldaTwinfire()
            : base("the Beast Handler", "Ysolda Twinfire")
        {
        }

        public YsoldaTwinfire(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(90, 95, 40);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = 8251; // Wild ponytail
            HairHue = 1359; // Fiery red-orange
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2117, Name = "Twinned Beast Harness" }); // Charcoal-black leather
            AddItem(new StuddedLegs() { Hue = 1365, Name = "Flame-Scorched Greaves" }); // Dark red
            AddItem(new LeatherGloves() { Hue = 1153, Name = "Clawmarked Gauntlets" }); // Blood-stained
            AddItem(new BearMask() { Hue = 1175, Name = "Twinhead's Mark" }); // Masked visage, tribute to her beast
            AddItem(new Cloak() { Hue = 1164, Name = "Ashen Beastcloak" }); // Smoky grey
            AddItem(new Boots() { Hue = 1109, Name = "Trailworn Boots" }); // Dusty black

            AddItem(new Pitchfork() { Hue = 2118, Name = "Beast Lurer" }); // Twisted iron

            Backpack backpack = new Backpack();
            backpack.Hue = 2101; // Dark brown
            backpack.Name = "Handler's Pack";
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
