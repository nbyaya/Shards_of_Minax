using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DuelOfTheElementsQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Duel of the Elements"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Elric Snowmantle*, an Elemental Mage Apprentice clad in shimmering robes that flicker with both frost and flame.\n\n" +
                    "His breath hangs in the cold air, mingled with sparks of elemental tension.\n\n" +
                    "“I sense it now more than ever... deep beneath the Ice Cavern. A creature born of chaos—*Chillfire*, a fusion of ice and flame beyond control.”\n\n" +
                    "“It was my father who warned of such fusions. He fell to one near Devil Guard, trying to stabilize its fury. Now I must finish what he began.”\n\n" +
                    "**Slay the Chillfire Elemental** before it ruptures the cavern’s core and destabilizes the elemental balance of this region.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware, traveler. The cold may burn, and the fire may freeze, should Chillfire rise unchecked.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still lives? The balance tips. I can feel the cavern shudder. We are running out of time.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You have done it... I can feel the surge subsiding. Chillfire is no more, and the balance holds—for now.\n\n" +
                       "Take this: *NaginataOfTomoeGozen*. Let it serve as both weapon and symbol, a blade that dances between the elements, as you did.";
            }
        }

        public DuelOfTheElementsQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ChillfireElemental), "Chillfire Elemental", 1));
            AddReward(new BaseReward(typeof(NaginataOfTomoeGozen), 1, "NaginataOfTomoeGozen"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Duel of the Elements'!");
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

    public class ElricSnowmantle : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DuelOfTheElementsQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMage());
        }

        [Constructable]
        public ElricSnowmantle()
            : base("the Elemental Mage Apprentice", "Elric Snowmantle")
        {
        }

        public ElricSnowmantle(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 95, 75);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1152; // Pale frost hue
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Frosty white
            FacialHairItemID = 0; // Clean-shaven
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1157, Name = "Frostfire Robe" }); // Frost blue
            AddItem(new WizardsHat() { Hue = 1359, Name = "Hat of Elemental Focus" }); // Glowing ember hue
            AddItem(new Sandals() { Hue = 2101, Name = "Steps of Balance" }); // Subtle ice blue
            AddItem(new BodySash() { Hue = 1175, Name = "Sash of Shifting Tides" }); // Mixed frost & flame tone

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Elementalist's Pack";
            AddItem(backpack);

            AddItem(new GnarledStaff() { Hue = 2118, Name = "Staff of Equilibrium" }); // Ice and flame energy
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
