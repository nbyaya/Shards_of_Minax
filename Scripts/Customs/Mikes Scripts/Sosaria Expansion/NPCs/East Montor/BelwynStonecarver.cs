using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class FairScalesNoMoreQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Fair Scales No More"; } }

        public override object Description
        {
            get
            {
                return
                    "You find *Belwyn Stonecarver*, the town mason, amidst shattered statues and cracked marble.\n\n" +
                    "Her hands are dusted with stone, yet her gaze is sharp, burning with old grief and fresh anger.\n\n" +
                    "“The ground trembles, not with the pulse of the earth—but the rage of a beast that should’ve been slain long ago.”\n\n" +
                    "“That **BlondeDragon**... it nests in the eastern caverns of the Drakkon caves. Its tremors have shattered my latest works. Worse still, it took my mentor—he vanished years ago hunting it.”\n\n" +
                    "“I won’t see East Montor’s legacy crushed under claw and flame. Will you end this for us? For him?”\n\n" +
                    "**Slay the BlondeDragon** and let the earth rest easy once more.";
            }
        }

        public override object Refuse
        {
            get
            {
                return "Then these cracks will grow, and our past will fall to ruin with each tremor. I hope you change your mind before it's too late.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still it roars? The ground is weaker than ever. Statues topple in the night, and my dreams echo with my mentor’s last cries.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done it... The tremors are gone. The BlondeDragon's reign has ended.\n\n" +
                       "You’ve not only avenged my mentor—you’ve saved our craft. Our stone will stand tall again.\n\n" +
                       "Take this: *ExcalibursLegacy*. May it serve you as well as you’ve served East Montor.";
            }
        }

        public FairScalesNoMoreQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(BlondeDragon), "BlondeDragon", 1));
            AddReward(new BaseReward(typeof(ExcalibursLegacy), 1, "ExcalibursLegacy"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Fair Scales No More'!");
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

    public class BelwynStonecarver : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(FairScalesNoMoreQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBStoneCrafter());
        }

        [Constructable]
        public BelwynStonecarver()
            : base("the Town Mason", "Belwyn Stonecarver")
        {
        }

        public BelwynStonecarver(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(75, 80, 30);

            Female = true;
            Body = 0x191; // Female
            Race = Race.Human;

            Hue = 1002; // Light tan skin tone
            HairItemID = 0x2047; // Long hair
            HairHue = 1109; // Dusty grey
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherDo() { Hue = 2413, Name = "Mason's Smock" }); // Stone-grey
            AddItem(new StuddedLegs() { Hue = 1824, Name = "Carver's Greaves" }); // Slate
            AddItem(new LeatherGloves() { Hue = 2301, Name = "Dust-Touched Gloves" }); // Pale stone
            AddItem(new WideBrimHat() { Hue = 1811, Name = "Chiselbrim Hat" }); // Dark earth
            AddItem(new HalfApron() { Hue = 2105, Name = "Stoneweaver's Apron" }); // Marble white
            AddItem(new Boots() { Hue = 1102, Name = "Foundation Boots" }); // Charcoal

            AddItem(new SledgeHammer() { Hue = 2505, Name = "Masterwork Mallet" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1150;
            backpack.Name = "Stonecarver's Pack";
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
