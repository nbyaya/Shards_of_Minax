using System;
using Server;
using Server.Items;
using Server.Engines.Quests;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ArcaneSwineQuest : BaseQuest
    {
        public override bool DoneOnce { get { return true; } }

        public override object Title { get { return "Boar of Blight"; } }

        public override object Description
        {
            get
            {
                return
                    "Before you stands *Orin Blackhoof*, a broad-shouldered tamer with hands like leather and eyes sharp from years spent herding stubborn beasts.\n\n" +
                    "Clad in soot-streaked hides and heavy boots caked with dried mud, he waves you over, voice gruff but urgent.\n\n" +
                    "“I don’t deal in fancy words. I deal in meat, and right now, it’s going bad.”\n\n" +
                    "“One of my prize boars—strong, fierce, but never foul—turned. Started rooting near the old Academy, near *Malidor’s cursed grounds*. Now it's bloated with magic, belching out toxic muck, trampling pasture like a beast possessed.”\n\n" +
                    "**“Kill it.”**\n\n" +
                    "“I’ve heard tales... my granddad said some pigs are born wrong, shaped by spells no man should cast. Maybe that’s true. But what I know? If this *Arcane Swine* isn’t stopped, I lose my herd. Death Glutch loses its meat. And worse, that filth might spread.”\n\n" +
                    "“You up for slaying something that oinks like death?”";
            }
        }

        public override object Refuse
        {
            get
            {
                return "You leave the muck to fester, then. But don’t come crying when there’s no pork left in the Glutch.";
            }
        }

        public override object Uncomplete
        {
            get
            {
                return "Still stomping around? I hear it at night now... snarling in the wind. Don’t wait too long, friend. My herd’s skittish, and I’m running out of time.";
            }
        }

        public override object Complete
        {
            get
            {
                return "You’ve done what others wouldn’t.\n\n" +
                       "The herd’s safe, for now. The pastures’ll heal.\n\n" +
                       "Take these—*PickpocketsNimbleGloves*. They ain’t for thieving, but for quick hands. A tamer’s hands, if you ever fancy that life. Fast, sure, and firm.";
            }
        }

        public ArcaneSwineQuest() : base()
        {
            AddObjective(new SlayObjective(typeof(ArcaneSwine), "Arcane Swine", 1));
            AddReward(new BaseReward(typeof(PickpocketsNimbleGloves), 1, "PickpocketsNimbleGloves"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x23, "You've completed 'Boar of Blight'!");
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

    public class OrinBlackhoof : MondainQuester
    {
        public override Type[] Quests { get { return new Type[] { typeof(ArcaneSwineQuest) }; } }

        public override bool IsActiveVendor { get { return true; } }
		public override bool UsesRandomisedStock => true;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAnimalTrainer());
        }

        [Constructable]
        public OrinBlackhoof()
            : base("the Livestock Tamer", "Orin Blackhoof")
        {
        }

        public OrinBlackhoof(Serial serial) : base(serial) { }

        public override void InitBody()
        {
            InitStats(100, 80, 50);

            Female = false;
            Body = 0x190; // Male
            Race = Race.Human;

            Hue = 2101; // Weathered, sun-worn skin
            HairItemID = 0x2047; // Long hair
            HairHue = 1109; // Sooty black
            FacialHairItemID = 0x204B; // Thick beard
            FacialHairHue = 1109;
        }

        public override void InitOutfit()
        {
            AddItem(new LeatherChest() { Hue = 1837, Name = "Boarhide Jerkin" }); // Dark muddy brown
            AddItem(new StuddedLegs() { Hue = 1824, Name = "Mud-Stained Breeches" });
            AddItem(new LeatherGloves() { Hue = 1843, Name = "Tamer's Grip" });
            AddItem(new BearMask() { Hue = 1812, Name = "Boarsnout Mask" }); // Looks intimidating, fitting his style
            AddItem(new Boots() { Hue = 1815, Name = "Trample-Stompers" });

            AddItem(new ShepherdsCrook() { Hue = 2105, Name = "Blackhoof’s Crook" });

            Backpack backpack = new Backpack();
            backpack.Hue = 1109; // Dark gray
            backpack.Name = "Tamer's Gear Pack";
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
