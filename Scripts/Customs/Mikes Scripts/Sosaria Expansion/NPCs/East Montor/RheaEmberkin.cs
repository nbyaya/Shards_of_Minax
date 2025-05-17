using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GargoylesFallQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Gargoyle's Fall"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Rhea Emberkin*, Firewarden of East Montor.\n\n" +
                    "Her eyes flicker like embers beneath soot-streaked brows, and her armor smells faintly of ash and brimstone.\n\n" +
                    "\"I've patrolled the lava flows near Drakkon's Caves since I could wield a mace. The heat I can bear—the creatures, I can fight. But this gargoyle...\"\n\n" +
                    "\"It perched above the molten river, its wings casting shadows that *burn*. Each screech rattles the rocks, and my patrols end in flames and loss.\"\n\n" +
                    "\"If you are brave enough, slay the beast. Bring me peace, and I'll see you're rewarded as one who dares the fire.\"\n\n" +
                    "**Slay the DrakonsGargoyle** perched above the lava flow in the Caves of Drakkon.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then beware the lava paths. Its wings wait for another fool.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "It still clings to the rocks? Then I'll keep my mace close... and my hopes low.";
            }
        }

        public override object Complete
        {
            get
            {
                return "The gargoyle has fallen? Then the flows run free again.\n\n" +
                       "\"You've earned more than coin. You've earned the respect of every Firewarden. Take this from the MacingBonusChest—let it serve you as you have served us.\"";
            }
        }

        public GargoylesFallQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(DrakonsGargoyle), "DrakonsGargoyle", 1));
            AddReward(new BaseReward(typeof(MacingBonusChest), 1, "MacingBonusChest"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Gargoyle's Fall'!");
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

    public class RheaEmberkin : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GargoylesFallQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBMaceWeapon()); // As a Firewarden, she trains in blunt weapons.
        }

        [Constructable]
        public RheaEmberkin()
            : base("the Firewarden", "Rhea Emberkin")
        {
        }

        public RheaEmberkin(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 95, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1359; // Ember-red
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 1358, Name = "Emberplate Chest" }); // Lava-red
            AddItem(new PlateLegs() { Hue = 1358, Name = "Emberplate Greaves" });
            AddItem(new PlateArms() { Hue = 1358, Name = "Emberplate Pauldrons" });
            AddItem(new PlateGloves() { Hue = 1358, Name = "Firewarden's Gauntlets" });
            AddItem(new PlateHelm() { Hue = 1358, Name = "Firewarden's Helm" });
            AddItem(new Cloak() { Hue = 1372, Name = "Cloak of Cinders" }); // Sooty-gray

            AddItem(new WarMace() { Hue = 2101, Name = "Ashen Mace" }); // Charcoal-black weapon

            Backpack backpack = new Backpack();
            backpack.Hue = 1157;
            backpack.Name = "Firewarden's Pack";
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
