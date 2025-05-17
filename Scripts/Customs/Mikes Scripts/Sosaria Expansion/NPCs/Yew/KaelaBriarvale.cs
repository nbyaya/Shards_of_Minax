using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ShadowCavalryQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Shadow Cavalry"; } }

        public override object Description
        {
            get
            {
                return
                    "Kaela Briarvale, the **Portal Warden** of Yew, stands vigilant by a flickering gate, her hands trembling slightly as the arcane energy pulses around her.\n\n" +
                    "“Do you hear it? The hoofbeats in the distance?” she asks, her voice low and urgent.\n\n" +
                    "“A **PlagueRider** has crossed into our realm—a mounted specter, borne of rot and shadow. Each step it takes poisons the land, and the **portals I guard twist in its wake**. I fear if it is not stopped, the corruption will spread to every village through the very gates meant to protect us.”\n\n" +
                    "She clutches a ring of portal keys, each glowing dimly, as if drained by some unseen force.\n\n" +
                    "“You must slay the PlagueRider. It cannot be allowed to complete its march of death.”\n\n" +
                    "“Do this, and I will entrust you with **RiftReaver**, forged from the remnants of shattered gates, a blade that tears through the veil.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then watch the skies. If you see a crimson light through the portal, it is already too late.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The rider still marches? I feel its steps in the portals. My keys burn in my hands...";
            }
        }

        public override object Complete
        {
            get
            {
                return "The portals... they are calm again. You’ve broken its march.\n\n" +
                       "**RiftReaver** is yours, as promised. May it sever any darkness that dares to pass through our gates again.\n\n" +
                       "Thank you, brave one. Yew owes you more than you know.";
            }
        }

        public ShadowCavalryQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(PlagueRider), "PlagueRider", 1));
            AddReward(new BaseReward(typeof(RiftReaver), 1, "RiftReaver"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Shadow Cavalry'!");
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

    public class KaelaBriarvale : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ShadowCavalryQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic()); // Closest vendor type to her role as Portal Warden
        }

        [Constructable]
        public KaelaBriarvale()
            : base("the Portal Warden", "Kaela Briarvale")
        {
        }

        public KaelaBriarvale(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 75, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1153; // Midnight blue
        }

        public override void InitOutfit()
        {
            AddItem(new Robe() { Hue = 1150, Name = "Void-Touched Robe" }); // Dark purple
            AddItem(new WizardsHat() { Hue = 1175, Name = "Dimensional Hood" }); // Shimmering silver-blue
            AddItem(new Sandals() { Hue = 1109, Name = "Portalstrider Sandals" }); // Dark gray
            AddItem(new BodySash() { Hue = 1170, Name = "Keybearer's Sash" }); // Faintly glowing azure

            AddItem(new MagicWand() { Hue = 1161, Name = "Gatebinder's Wand" }); // Shadowy black with blue accents

            Backpack backpack = new Backpack();
            backpack.Hue = 1175;
            backpack.Name = "Warden's Satchel";
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
