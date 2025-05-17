using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DroneOfLightQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Drone of Light"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Oren Spark*, the Illumination Mage, amidst a circle of glowing crystals.\n\n" +
                    "He adjusts strange, lens-covered spectacles, light flickering across his angular face.\n\n" +
                    "“Ah, seeker of shadows and slayer of light-born nightmares... I trust you can see past the glow.”\n\n" +
                    "“In the depths of **Preservation Vault 44**, a construct—**VaultDrone**—has awakened. Not of this time, not of this realm, it pulses with concentrated flares. My own lantern orb nearly imploded trying to illuminate it.”\n\n" +
                    "“I crafted these glare-resistant lenses, but they only track its flight path—they cannot end its blaze.”\n\n" +
                    "“Destroy the **VaultDrone**. Before it blinds more explorers, or worse, overloads the vault’s delicate balances.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then avert your gaze when next you walk the halls of the Vault... for light, uncontrolled, can devour the very air you breathe.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "You’ve yet to dim its flare? The Vault glows brighter with each pulse. Soon, none will see the path within.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The Vault breathes easier, its light no longer a torment. You’ve snuffed out a sun-born specter.\n\n" +
                       "**Take this: the LilyveilKimono**. Woven from threads kissed by moonlight and shadow. May it shield you from future glares, both seen and unseen.";
            }
        }

        public DroneOfLightQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(VaultDrone), "VaultDrone", 1));
            AddReward(new BaseReward(typeof(LilyveilKimono), 1, "LilyveilKimono"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Drone of Light'!");
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

    public class OrenSpark : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(DroneOfLightQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMystic());
        }

        [Constructable]
        public OrenSpark()
            : base("the Illumination Mage", "Oren Spark")
        {
        }

        public OrenSpark(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(80, 85, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 1023; // Pale skin with a faint glow
            HairItemID = 0x203C; // Short Hair
            HairHue = 1150; // Platinum
            FacialHairItemID = 0x2041; // Goatee
            FacialHairHue = 1150;
        }

        public override void InitOutfit()
        {
            AddItem(new MaleKimono() { Hue = 1153, Name = "Lilyveil Robe" }); // Light azure
            AddItem(new BodySash() { Hue = 1151, Name = "Starwoven Sash" }); // Silver-blue
            AddItem(new WizardsHat() { Hue = 1175, Name = "Lensweaver Hat" }); // Luminous violet
            AddItem(new Sandals() { Hue = 1150, Name = "Moonstep Sandals" }); // Pale silver
            AddItem(new MagicWand() { Hue = 1260, Name = "Orb-Tuned Wand" }); // Light blue, glowing orb

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Lenswright's Pack";
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
