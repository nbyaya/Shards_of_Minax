using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GhostsOfSteelQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Ghosts of Steel"; } }

        public override object Description
        {
            get
            {
                return
                    "*Maris Steelgut*, the master blacksmith of Grey, stands amid the clang of anvils and the hiss of quenching steel.\n\n" +
                    "Her armor bears old dents, polished but unhidden, and her hammer glows faintly, touched by forgotten fires.\n\n" +
                    "“I worked the forges of Moon once, before settling here in Grey. There, metal sang under my hands. Here... it weeps.”\n\n" +
                    "“The **AgedAlloyWraith** haunts the old paths to Exodus. It warps iron, rusts blades before they see battle. My best work, ruined before it’s wielded. The ghost *feeds* on the craft.”\n\n" +
                    "**Slay the AgedAlloyWraith** and bring me back its haunted scrap. Only then can I forge again without dread.”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then the forges will stay cold, and the steel will rot. Return if your heart hardens.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "The wraith still stalks the old roads? Then every blade I make is cursed to crumble.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... the forge breathes free again.\n\n" +
                       "Take this: **GuardianOfTheFey**. It’s light, swift, and pure—unspoiled by rust or shadow. May it protect you, as you’ve protected my craft.";
            }
        }

        public GhostsOfSteelQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(AgedAlloyWraith), "AgedAlloyWraith", 1));
            AddReward(new BaseReward(typeof(GuardianOfTheFey), 1, "GuardianOfTheFey"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Ghosts of Steel'!");
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

    public class MarisSteelgut : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GhostsOfSteelQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBBlacksmith());
        }

        [Constructable]
        public MarisSteelgut()
            : base("the Master Blacksmith", "Maris Steelgut")
        {
        }

        public MarisSteelgut(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 75, 50);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1001; // Tanned
            HairItemID = 0x2048; // Long Hair
            HairHue = 1108; // Steel-grey
        }

        public override void InitOutfit()
        {
            AddItem(new PlateChest() { Hue = 2210, Name = "Moon-Forged Breastplate" }); // Deep grey-blue
            AddItem(new PlateArms() { Hue = 2115, Name = "Smith’s Armguards" }); // Dark iron
            AddItem(new PlateLegs() { Hue = 2410, Name = "Forgewalkers" }); // Ashened steel
            AddItem(new PlateGloves() { Hue = 2101, Name = "Hammerhide Gloves" });
            AddItem(new SkullCap() { Hue = 1825, Name = "Cinder-Wrought Helm" });
            AddItem(new HalfApron() { Hue = 1830, Name = "Ashen Forge Apron" });

            AddItem(new SmithSmasher() { Hue = 2500, Name = "Steelgut’s Hammer" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Blacksmith’s Satchel";
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
