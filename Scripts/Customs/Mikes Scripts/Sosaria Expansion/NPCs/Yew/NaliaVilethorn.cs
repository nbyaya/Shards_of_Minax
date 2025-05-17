using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class BonesOfPestilenceQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Bones of Pestilence"; } }

        public override object Description
        {
            get
            {
                return
                    "You approach *Nalia Vilethorn*, the Occultist of Yew.\n\n" +
                    "She stands beneath twisted branches, her robe hemmed with symbols of decay, eyes glowing faintly green as she stirs vials of noxious reagents.\n\n" +
                    "“The balance of death and life here in Yew is fragile, child. But there’s a blight that threatens it: the **PestilentBoneMagi**.”\n\n" +
                    "“It’s not content with the stillness of graves. It seeks to *wake the dead*, twist their bones, pollute our soil. If it isn't stopped, the graveyards will no longer be sanctuaries—but battlefields.”\n\n" +
                    "“I have sealed the worst of my reagents away, but I sense the *call* growing stronger. **Slay the PestilentBoneMagi** before the rot spreads beyond the Catastrophe.”\n\n" +
                    "“Bring decay to the decayer, and you shall walk with lightness once more.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Decay comes to all things, eventually... but now it comes too soon. I only hope we are not already too late.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it breathes pestilence? The wind carries whispers of bone and rot. I feel it... stirring...";
            }
        }

        public override object Complete
        {
            get
            {
                return "So the blight has been cut away. Good.\n\n" +
                       "Take these *TumblesparkSoles*. May they ward your steps from the clutches of decay, and carry you swiftly should the dead rise again.";
            }
        }

        public BonesOfPestilenceQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PestilentBoneMagi), "PestilentBoneMagi", 1));
            AddReward(new BaseReward(typeof(TumblesparkSoles), 1, "TumblesparkSoles"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Bones of Pestilence'!");
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

    public class NaliaVilethorn : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(BonesOfPestilenceQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBNecromancer());
        }

        [Constructable]
        public NaliaVilethorn()
            : base("the Occultist", "Nalia Vilethorn")
        {
        }

        public NaliaVilethorn(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 25);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1157; // Pale, sickly hue
            HairItemID = Race.RandomHair(this);
            HairHue = 2207; // Deep green
        }

        public override void InitOutfit()
        {
            AddItem(new MonkRobe() { Hue = 1266, Name = "Vilethorn Shroud" }); // Deep sickly green
            AddItem(new WizardsHat() { Hue = 1150, Name = "Plaguepeak Hat" }); // Dark grey-blue
            AddItem(new Sandals() { Hue = 1175, Name = "Ashen Soles" }); // Dusty grey
            AddItem(new BodySash() { Hue = 1153, Name = "Bile-Stained Sash" }); // Pale yellow-green
            AddItem(new LeatherGloves() { Hue = 1272, Name = "Toxin-Touched Gloves" }); // Putrid green

            Backpack backpack = new Backpack();
            backpack.Hue = 1109;
            backpack.Name = "Occultist's Pack";
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
