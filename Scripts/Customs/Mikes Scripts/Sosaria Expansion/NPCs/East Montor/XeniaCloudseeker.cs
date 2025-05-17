using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class VerdantVenomQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Verdant Venom"; } }

        public override object Description
        {
            get
            {
                return
                    "You stand before *Xenia Cloudseeker*, Geomancer of East Montor, her robes flickering with faint green runes, eyes fixed on a crystal that hums with seismic whispers.\n\n" +
                    "“The land speaks, if you know how to listen. And lately, it cries in pain.”\n\n" +
                    "“Beneath the Caves of Drakkon, there stirs a beast—**a GreenDragon**, vile and ancient. It poisons our springs with sap that clouds the mind, warping dream and thought alike. I’ve watched its lair shift in the stone, drawn by something deeper… darker.”\n\n" +
                    "**“You must hunt this serpent. End its venom. Free the waters before our roots wither.”**\n\n" +
                    "“Beware, traveler. Its scales drip madness.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the roots of Montor will rot, and the dreams of our people will turn to nightmares.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You’ve not yet stilled the beast? The waters run darker by the hour, and even the stone forgets its strength.";
            }
        }

        public override object Complete
        {
            get
            {
                return "**The earth sighs with relief. The springs clear.**\n\n" +
                       "“The dragon’s madness no longer seeps into our soil. You’ve done well, champion. Take these—*NottinghamStalkersLeggings*. May they carry you silently, as the roots now grow.”";
            }
        }

        public VerdantVenomQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GreenDragon), "GreenDragon", 1));
            AddReward(new BaseReward(typeof(NottinghamStalkersLeggings), 1, "NottinghamStalkersLeggings"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Verdant Venom'!");
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

    public class XeniaCloudseeker : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(VerdantVenomQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBWeaponSmith());
        }

        [Constructable]
        public XeniaCloudseeker()
            : base("the Geomancer", "Xenia Cloudseeker")
        {
        }

        public XeniaCloudseeker(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(70, 75, 90);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Pale
            HairItemID = 0x2048; // Long hair
            HairHue = 1272; // Verdant green
        }

        public override void InitOutfit()
        {
            AddItem(new FancyShirt() { Hue = 1411, Name = "Seismweave Tunic" }); // Earthy green
            AddItem(new Skirt() { Hue = 1420, Name = "Verdant Flow Skirt" }); // Deep forest green
            AddItem(new BodySash() { Hue = 1260, Name = "Runesash of Montor" }); // Mystic teal
            AddItem(new WizardsHat() { Hue = 1267, Name = "Crown of Echoing Stone" }); // Crystal grey
            AddItem(new Sandals() { Hue = 2101, Name = "Rootwalkers" }); // Bark-brown

            AddItem(new GnarledStaff() { Hue = 1260, Name = "Sapcaller's Rod" }); // Twisted wood

            Backpack backpack = new Backpack();
            backpack.Hue = 1272;
            backpack.Name = "Geomancer's Satchel";
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
