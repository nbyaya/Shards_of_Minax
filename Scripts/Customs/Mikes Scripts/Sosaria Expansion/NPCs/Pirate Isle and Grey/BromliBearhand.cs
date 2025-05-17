using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class GraveclawsRoarQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Graveclaw’s Roar"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Bromli 'Bearhand' Ulmar*, a rugged beastmaster of Pirate Isle.\n\n" +
                    "He grips a heavy staff, its head carved like a bear’s maw, his left hand wrapped in thick leather—scarred from years spent taming beasts.\n\n" +
                    "\"One of my prized bears… lost. The others? Hiding. Shaking in their pens like cubs in winter.\"\n\n" +
                    "\"A beast, bigger than any bear I’ve seen—*GraveclawUrsus*—come stomping out from Exodus’ cursed halls. Roared like thunder, sent 'em all scattering.\"\n\n" +
                    "\"I’ve trained these beasts since they could barely stand, and now they won't come near me, not while *that thing* is alive.\"\n\n" +
                    "\"You look the sort to face death and come back grinning. Bring me its head—or fur, even. Prove the beast is gone. I’ll bake you a pie like my old mum used to, as sweet as victory.\"\n\n" +
                    "**Slay GraveclawUrsus** and help Bromli reclaim his kin.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Bah. I’ll not force you to face it—but don’t blame me if you hear roars in the night, comin’ for the town.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still breathin', is it? Then my bears still hide, and I’ll be damned if I let that stand. Get back out there.";
            }
        }

        public override object Complete
        {
            get
            {
                return "Aye… that’s Graveclaw’s scent. My bears’ll rest easy now.\n\n" +
                       "Take this, fresh-baked—*BlueberryPie*. Baked with a calm hand now that the roars are gone.\n\n" +
                       "And here, a strand of its fur—keeps me sharp, reminds me that no beast, no matter how wild, stays untamed forever.";
            }
        }

        public GraveclawsRoarQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(GraveclawUrsus), "GraveclawUrsus", 1));
            AddReward(new BaseReward(typeof(BlueberryPie), 1, "Blueberry Pie"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Graveclaw’s Roar'!");
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

    public class BromliBearhand : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(GraveclawsRoarQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
        public override bool UsesRandomisedStock => false;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public BromliBearhand()
            : base("the Beastmaster", "Bromli 'Bearhand' Ulmar")
        {
        }

        public BromliBearhand(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(95, 85, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = 1108; // Bear-brown
            FacialHairItemID = 0x203C; // Full beard
            FacialHairHue = 1108;
        }

        public override void InitOutfit()
        {
            AddItem(new BearMask() { Hue = 1108, Name = "Grizzled Mask of the Wilds" });
            AddItem(new StuddedChest() { Hue = 1801, Name = "Beastmaster's Tunic" });
            AddItem(new StuddedLegs() { Hue = 1809, Name = "Pelt-Lined Trousers" });
            AddItem(new StuddedGloves() { Hue = 2301, Name = "Scarred Trainer’s Grips" });
            AddItem(new Boots() { Hue = 2306, Name = "Forest Walker’s Boots" });
            AddItem(new BodySash() { Hue = 1175, Name = "Blueberry-Dyed Sash" });

            AddItem(new ShepherdsCrook() { Hue = 2106, Name = "Bearhand’s Staff" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1107;
            backpack.Name = "Tamer's Pack";
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
